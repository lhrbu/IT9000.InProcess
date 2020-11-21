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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IT9000.Wpf.ViewModels;
using Panuon.UI.Silver;

namespace IT9000.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public MainWindow(MainWindowVM mainWindowVM)
        {
            InitializeComponent();
            DataContext = mainWindowVM;
            throw new ArgumentException("Test Global exception handle");
        }
    }
}
