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

namespace PV6900.Wpf.Views
{
    /// <summary>
    /// TextDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TextDialog : Window,IDisposable
    {
        public TextDialog()
        {
            InitializeComponent();
            Closing += (sender, e) =>
            {
                this.Hide();
                e.Cancel = true;
            };
        }

        public string Value 
        { 
            get=>(string)GetValue(ValueProperty); 
            set=>SetValue(ValueProperty,value); 
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(TextDialog));

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => this.Hide();

        private void ResetButton_Click(object sender, RoutedEventArgs e) =>
            Value = string.Empty;

        private bool _disposed=false;
        public void Dispose()
        {
            if(!_disposed)
            {
                this.Close();
            }
            GC.SuppressFinalize(this);
        }
    }
}
