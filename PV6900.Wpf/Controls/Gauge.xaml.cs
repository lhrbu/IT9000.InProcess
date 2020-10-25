﻿using System;
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
    /// Gauge.xaml 的交互逻辑
    /// </summary>
    public partial class Gauge : UserControl
    {
        public Gauge()
        {
            InitializeComponent();
        }

        public string Header
        { 
            get=>(string)GetValue(HeaderProperty); 
            set=>SetValue(HeaderProperty,value); 
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(Gauge));


        public double Value
        {
            get=>(double)GetValue(ValueProperty);
            set=>SetValue(ValueProperty,value);
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(Gauge));
    }
}
