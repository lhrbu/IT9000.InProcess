using IT9000.Wpf.ViewModels;
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

namespace IT9000.Wpf.Views
{
    /// <summary>
    /// DisconnectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DisconnectWindow : Window
    {
        public DisconnectWindow(DisconnectWindowVM disconnectWindowVM)
        {
            InitializeComponent();
            DataContext = disconnectWindowVM;
        }
    }
}
