using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

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
        private BindingList<TextItemModel> textItems = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();
            _filePath = _config.GetValue<string>("FilePath");

            TextItemModel textItem1 = new() { Description = "Test", Text = "Some text to copy." };
            textItems.Add(textItem1);
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
    }
}
