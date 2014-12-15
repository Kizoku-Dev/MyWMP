using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WMP
{
    /// <summary>
    /// Logique d'interaction pour NameEntering.xaml
    /// </summary>
    public partial class NameEntering : Window
    {
        public NameEntering()
        {
            InitializeComponent();
        }

        public NameEntering(String labelName, String editName, String buttonName)
        {
            InitializeComponent();
            this.buttonToUse.Content = buttonName;
            this.editToUse.Text = editName;
            this.labelToUse.Content = labelName;
        }

        public void setNames(String labelName, String editName, String buttonName, MainWindow mainWindow, int option)
        {
            this.buttonToUse.Content = buttonName;
            this.editToUse.Text = editName;
            this.labelToUse.Content = labelName;
            if (option == 0)
                this.buttonToUse.Click += new RoutedEventHandler(mainWindow.renaming_playlist);
            if (option == 1)
                this.buttonToUse.Click += new RoutedEventHandler(mainWindow.renaming_media);
        }
    }
}
