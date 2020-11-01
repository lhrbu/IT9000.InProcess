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

namespace PV6900.Wpf.Controls
{
    /// <summary>
    /// StatusBar.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBar : Grid
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        public string Header { get=>(string)GetValue(HeaderProperty); set=>SetValue(HeaderProperty,value); }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(StatusBar));

        public string CurrentValue { get => (string)GetValue(CurrentValueProperty); set => SetValue(CurrentValueProperty, value); }
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(string), typeof(StatusBar));

        public string TotalValue { get => (string)GetValue(TotalValueProperty); set => SetValue(TotalValueProperty, value); }
        public static readonly DependencyProperty TotalValueProperty =
            DependencyProperty.Register(nameof(TotalValue), typeof(string), typeof(StatusBar));
    }
}
