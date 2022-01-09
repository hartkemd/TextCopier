using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TextCopier
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dataAccess = new TextFileDataAccess();
            Application.Current.MainWindow = new MainWindow(dataAccess);
            Application.Current.MainWindow.Show();
        }
    }
}
