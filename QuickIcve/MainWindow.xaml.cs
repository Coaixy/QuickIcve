using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickIcve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string cookie = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        public void recevieInfo(string data)
        {
            this.cookie = data;
        }
        private void initInfo(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.SendMessage = recevieInfo;
            l.Show();
        }
    }
}