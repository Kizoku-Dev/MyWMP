using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MyWMPv2.View;
using MyWMPv2.ViewModel;

namespace MyWMPv2
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApplicationView app = new ApplicationView();
            app.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            app.Show();
        }
    }
}
