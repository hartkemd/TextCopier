using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace TextCopier
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dataAccess = new SqliteCrud(GetConnectionString());
            Application.Current.MainWindow = new MainWindow(dataAccess);
            Application.Current.MainWindow.Show();
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
