using System.Windows.Forms;

namespace MyWMPv2.View
{
    class MyDialog
    {
        public enum Size
        {
            Normal,
            Big
        }

        public static string Prompt(string title, string text, Size size)
        {
            if (size == Size.Normal)
                return Prompt_Normal(title, text);
            else
                return Prompt_Big(title, text);
        }

        private static string Prompt_Normal(string title, string text)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 170;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = title;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 200 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 80 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }

        private static string Prompt_Big(string title, string text)
        {
            Form prompt = new Form();
            prompt.Width = 600;
            prompt.Height = 250;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = title;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 400, Height = 300 };
            TextBox textBox = new TextBox() { Left = 50, Top = 100, Width = 500 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 150 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }

        public static void Show(string title, string text)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 200;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.Text = title;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 10, Top = 10, Text = text, Width = 480, Height = 180 };
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();
        }
    }
}
