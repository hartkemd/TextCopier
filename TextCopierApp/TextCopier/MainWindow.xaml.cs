using DataAccessLibrary;
using DataAccessLibrary.Models;
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
        private static TextFileDataAccess db = new();
        private ObservableCollection<TextItemModel> textItems = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();
            _filePath = _config.GetValue<string>("FilePath");

            textItems.Add(new TextItemModel { Description = "Test", Text = "Some sample text to copy." });
            textItems.Add(new TextItemModel { Description = "Test2", Text = "Some more sample text to copy." });

            textItemsDataGrid.Items.Clear();
            textItemsDataGrid.ItemsSource = textItems;
        }

        private static void GetAllTextItems()
        {
            var textItems = db.ReadAllRecords(_filePath);

            foreach (var textItem in textItems)
            {
                //$"{textItem.Description} {textItem.Text}";
            }
        }

        private static void CreateTextItem(TextItemModel textItem)
        {
            var textItems = db.ReadAllRecords(_filePath);
            textItems.Add(textItem);
            db.WriteAllRecords(textItems, _filePath);
        }

        private static void UpdateFirstTextItemsDescription(string description)
        {
            var textItems = db.ReadAllRecords(_filePath);
            textItems[0].Description = description;
            db.WriteAllRecords(textItems, _filePath);
        }

        private static void RemoveDescriptionFromTextItem()
        {
            var textItems = db.ReadAllRecords(_filePath);
            textItems[0].Description = "";
            db.WriteAllRecords(textItems, _filePath);
        }

        private static void RemoveTextItem()
        {
            var textItems = db.ReadAllRecords(_filePath);
            textItems.RemoveAt(0);
            db.WriteAllRecords(textItems, _filePath);
        }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = builder.Build();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var textItem = (TextItemModel)button.DataContext;
            Clipboard.SetText(textItem.Text);
        }

        private void AddTextItemButton_Click(object sender, RoutedEventArgs e)
        {
            bool descriptionIsInvalid = string.IsNullOrWhiteSpace(textItemDescriptionTextBox.Text);
            bool textIsInvalid = string.IsNullOrWhiteSpace(textItemTextTextBox.Text);

            if (descriptionIsInvalid == false && textIsInvalid == false)
            {
                textItems.Add(new TextItemModel { Description = textItemDescriptionTextBox.Text, Text = textItemTextTextBox.Text });
                textItemDescriptionTextBox.Clear();
                textItemTextTextBox.Clear();
            }
        }

        private void RemoveTextItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (textItemsDataGrid.SelectedIndex != -1)
            {
                textItems.RemoveAt(textItemsDataGrid.SelectedIndex);
            }
        }
    }
}
