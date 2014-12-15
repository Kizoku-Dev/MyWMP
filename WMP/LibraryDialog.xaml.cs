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
    /// Logique d'interaction pour LibraryDialog.xaml
    /// </summary>
    public partial class LibraryDialog : Window
    {
        TreeView listLib;

        public LibraryDialog()
        {
            InitializeComponent();
        }

        public LibraryDialog(TreeView listLibToSet)
        {
            InitializeComponent();
            this.listLib = listLibToSet;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (this.libraryWithEditNameExists() == false)
            {
                this.listLib.Items.Add(new TreeViewItem());
                ((TreeViewItem)this.listLib.Items.GetItemAt(this.listLib.Items.Count - 1)).Header = this.edit.Text;
                if (System.IO.File.Exists("C:\\Users\\ovoyan_s\\Desktop\\WMP\\config.ini") == true)
                {
                    MessageBox.Show("TEST REUSSI");
                }
                this.uploading_of_medias_list();
            }
            else
            {
                MessageBox.Show("The library " + this.edit.Text + " arleady exists");
            }
        }

        private bool libraryWithEditNameExists()
        {
            int counter = 0;
            string str = this.edit.Text;
            string ll = "";
            while (counter < this.listLib.Items.Count)
            {
                ll = (string)((TreeViewItem)this.listLib.Items.GetItemAt(this.listLib.Items.Count - 1)).Header;
                if (str == ll)
                    return (true);
                ++counter;
            }
            return (false);
        }

        private void uploading_of_medias_list()
        {

        }
    }
}
