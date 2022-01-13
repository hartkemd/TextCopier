using DataAccessLibrary;
using DataAccessLibrary.Models;
using TextCopierLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;

namespace TextCopier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ObservableCollection<TextItemModel> textItems = new();
        private readonly ISqlCrud _sqlCrud;
        private bool useCustomSort = false; // default of false will sort A -> Z by Description
        private const string configFilePath = "config.txt";

        public MainWindow(ISqlCrud sqlCrud)
        {
            InitializeComponent();
            _sqlCrud = sqlCrud;
            IfConfigFileDoesNotExistCreateIt();
            ReadConfigFile();
            ReadAllTextItemsDependingOnCustomSort();

            textItemsDataGrid.Items.Clear();
            textItemsDataGrid.ItemsSource = textItems;
        }

        // Methods that talk to DataAccessLibrary:
        private void IfConfigFileDoesNotExistCreateIt()
        {
            if (TextFileIO.FileExists(configFilePath) == false)
            {
                WriteConfigFile();
            }
        }

        private void ReadConfigFile()
        {
            string fileContents = TextFileIO.ReadTextFromFile(configFilePath);

            bool isValidBool = bool.TryParse(fileContents, out bool customSortBool);

            if (isValidBool == true)
            {
                useCustomSort = customSortBool;
            }
        }

        private void WriteConfigFile()
        {
            TextFileIO.WriteTextToFile(configFilePath, useCustomSort.ToString());
        }

        private void ReadAllTextItemsDependingOnCustomSort()
        {
            if (useCustomSort == true)
            {
                ReadAllTextItemsOrderedBySortPosition();
            }
            else
            {
                ReadAllTextItemsOrderedByDescription();
            }
        }
        
        private void ReadAllTextItemsOrderedBySortPosition()
        {
            var records = _sqlCrud.ReadAllTextItemsOrderedBySortPosition();

            foreach (var record in records)
            {
                textItems.Add(record);
            }
        }

        private void ReadAllTextItemsOrderedByDescription()
        {
            var records = _sqlCrud.ReadAllTextItemsOrderedByDescription();

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

        private void MoveTextItemUpOrDownInDataGrid(int selectedIndex, int directionToMove)
        {
            TextItemModel selectedTextItem = (TextItemModel)textItemsDataGrid.SelectedItem;
            int indexOfOtherItem;

            if (directionToMove == -1 || directionToMove == 1)
            {
                indexOfOtherItem = selectedIndex + directionToMove;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(directionToMove));
            }

            TextItemModel otherTextItem = (TextItemModel)textItemsDataGrid.Items[indexOfOtherItem];

            HelperMethods.SwapTextItemSortPositions(selectedTextItem, otherTextItem);

            UpdateTextItem(selectedTextItem);
            UpdateTextItem(otherTextItem);

            textItems.Move(selectedIndex, indexOfOtherItem);
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
            useCustomSort = true;
            WriteConfigFile();

            int selectedIndex = textItemsDataGrid.SelectedIndex;

            if (selectedIndex > 0)
            {
                MoveTextItemUpOrDownInDataGrid(selectedIndex, -1);
            }

            ClearTextBoxes();
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            useCustomSort = true;
            WriteConfigFile();

            int selectedIndex = textItemsDataGrid.SelectedIndex;

            if ((selectedIndex != -1) && (selectedIndex < textItemsDataGrid.Items.Count - 1))
            {
                MoveTextItemUpOrDownInDataGrid(selectedIndex, 1);
            }

            ClearTextBoxes();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            useCustomSort = false;
            WriteConfigFile();

            textItems.Clear();
            ReadAllTextItemsOrderedByDescription();
            ClearTextBoxes();
        }
    }
}
