using DataAccessLibrary;
using DataAccessLibrary.Models;
using UIHelperLibrary;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TextCopier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IConfiguration _config;
        private static string _filePath;
        private static ObservableCollection<TextItemModel> textItems = new();
        private readonly IDataAccess _db;

        public MainWindow(IDataAccess db)
        {
            InitializeComponent();
            _db = db;
            InitializeConfiguration();
            _filePath = _config.GetValue<string>("FilePath");

            ReadAllTextItems();

            textItemsDataGrid.Items.Clear();
            textItemsDataGrid.ItemsSource = textItems;
            
        }

        // Methods that talk to DataAccessLibrary:
        private void ReadAllTextItems()
        {
            var records = _db.ReadAllRecords(_filePath);

            foreach (var record in records)
            {
                textItems.Add(record);
            }
        }

        private void WriteAllTextItems()
        {
            _db.WriteAllRecords(textItems, _filePath);
        }

        private void CreateTextItem(TextItemModel textItem)
        {
            var records = _db.ReadAllRecords(_filePath);
            records.Add(textItem);
            _db.WriteAllRecords(records, _filePath);
        }

        private void UpdateTextItemsDescription(string description, int index)
        {
            var records = _db.ReadAllRecords(_filePath);
            records[index].Description = description;
            _db.WriteAllRecords(records, _filePath);
        }

        private void UpdateTextItemsText(string text, int index)
        {
            var records = _db.ReadAllRecords(_filePath);
            records[index].Text = text;
            _db.WriteAllRecords(records, _filePath);
        }

        private void DeleteTextItem(int index)
        {
            var textItems = _db.ReadAllRecords(_filePath);
            textItems.RemoveAt(index);
            _db.WriteAllRecords(textItems, _filePath);
        }

        private void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = builder.Build();
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

                UpdateTextItemsDescription(textItems[index].Description, index);
                UpdateTextItemsText(textItems[index].Text, index);

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
                DeleteTextItem(index);
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
                WriteAllTextItems();
            }

            ClearTextBoxes();
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = textItemsDataGrid.SelectedIndex;

            if (selectedIndex < textItemsDataGrid.Items.Count - 1)
            {
                textItems.Move(selectedIndex, selectedIndex + 1);
                WriteAllTextItems();
            }

            ClearTextBoxes();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            textItems.Sort();
            WriteAllTextItems();
            ClearTextBoxes();
        }
    }
}
