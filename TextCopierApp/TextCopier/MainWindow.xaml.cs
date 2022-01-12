using DataAccessLibrary;
using DataAccessLibrary.Models;
using UIHelperLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TextCopier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ObservableCollection<TextItemModel> textItems = new();
        private readonly ISqlCrud _sqlCrud;

        public MainWindow(ISqlCrud sqlCrud)
        {
            InitializeComponent();
            _sqlCrud = sqlCrud;
            ReadAllTextItems();

            textItemsDataGrid.Items.Clear();
            textItemsDataGrid.ItemsSource = textItems;
        }

        // Methods that talk to DataAccessLibrary:
        private void ReadAllTextItems()
        {
            var records = _sqlCrud.ReadAllTextItems();

            foreach (var record in records)
            {
                textItems.Add(record);
            }
        }

        private void CreateTextItem(TextItemModel textItem)
        {
            _sqlCrud.CreateTextItem(textItem);
        }

        private void UpdateTextItem(TextItemModel textItem)
        {
            _sqlCrud.UpdateTextItem(textItem);
        }

        private void DeleteTextItem(int index)
        {
            _sqlCrud.DeleteTextItem(index);
        }

        // UI helper methods:
        private void ClearTextBoxes()
        {
            textItemDescriptionTextBox.Clear();
            textItemTextTextBox.Clear();
        }

        // UI events:
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var textItem = (TextItemModel)button.DataContext;
            Clipboard.SetText(textItem.Text);
        }

        private void CreateTextItemButton_Click(object sender, RoutedEventArgs e)
        {
            bool descriptionIsInvalid = string.IsNullOrWhiteSpace(textItemDescriptionTextBox.Text);
            bool textIsInvalid = string.IsNullOrWhiteSpace(textItemTextTextBox.Text);

            if (descriptionIsInvalid == false && textIsInvalid == false)
            {
                TextItemModel itemToAdd = new();
                itemToAdd.Description = textItemDescriptionTextBox.Text;
                itemToAdd.Text = textItemTextTextBox.Text;
                textItems.Add(itemToAdd);
                CreateTextItem(itemToAdd);
                ClearTextBoxes();
            }
        }

        private void UpdateTextItemButton_Click(object sender, RoutedEventArgs e)
        {
            int index = textItemsDataGrid.SelectedIndex;

            if (index != -1)
            {
                textItems[index].Description = textItemDescriptionTextBox.Text;
                textItems[index].Text = textItemTextTextBox.Text;

                UpdateTextItem((TextItemModel)textItemsDataGrid.SelectedItem);

                textItemsDataGrid.Items.Refresh();
                textItemsDataGrid.SelectedItem = null;
                ClearTextBoxes();
            }
        }

        private void DeleteTextItemButton_Click(object sender, RoutedEventArgs e)
        {
            int index = textItemsDataGrid.SelectedIndex;
            
            if (index != -1)
            {
                TextItemModel itemToDelete = (TextItemModel)textItemsDataGrid.SelectedItem;
                DeleteTextItem(itemToDelete.Id);
                textItems.RemoveAt(index);
                ClearTextBoxes();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            textItemsDataGrid.SelectedItem = null;
            ClearTextBoxes();
        }

        private void TextItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textItemsDataGrid.SelectedItem != null)
            {
                textItemDescriptionTextBox.Text = ((TextItemModel)textItemsDataGrid.SelectedItem).Description;
                textItemTextTextBox.Text = ((TextItemModel)textItemsDataGrid.SelectedItem).Text;
            }
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = textItemsDataGrid.SelectedIndex;

            if (selectedIndex > 0)
            {
                textItems.Move(selectedIndex, selectedIndex - 1);
                //WriteAllTextItems();
            }

            ClearTextBoxes();
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = textItemsDataGrid.SelectedIndex;

            if ((selectedIndex != -1) && (selectedIndex < textItemsDataGrid.Items.Count - 1))
            {
                textItems.Move(selectedIndex, selectedIndex + 1);
                //WriteAllTextItems();
            }

            ClearTextBoxes();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            textItems.Sort();
            //WriteAllTextItems();
            ClearTextBoxes();
        }
    }
}
