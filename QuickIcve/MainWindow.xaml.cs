using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        public void setImg(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource =
                new Uri("https://thirdqq.qlogo.cn/g?b=sdk&k=viavTehqhp6o5IHX7YMqMdQ&kti=Y-mklQAAAAA&s=140&t=1673866616",
                    UriKind.Absolute);
            bitmapImage.EndInit();
            Image tx = (Image)FindName("tx");
            tx.Source = bitmapImage;
        }
    }
}