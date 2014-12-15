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

        public NameEntering(String labelName, String editName, String buttonName, MainWindow mainWindow)
        {
            InitializeComponent();
            this.buttonToUse.Content = buttonName;
            this.editToUse.Text = editName;
            this.labelToUse.Content = labelName;
            this.buttonToUse.Click += new RoutedEventHandler(mainWindow.renaming);
        }

        public void setNames(String labelName, String editName, String buttonName)
        {
            this.buttonToUse.Content = buttonName;
            this.editToUse.Text = editName;
            this.labelToUse.Content = labelName;
        }
    }
}
