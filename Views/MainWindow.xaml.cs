using System;
using System.Windows;
using TestMaster.ViewModels;

namespace TestMaster.Views
{
    public partial class MainWindow : Window
    {
        private bool isLight = true;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void ThemeSwitch_Click(object sender, RoutedEventArgs e)
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri(isLight ? "/Themes/DarkColorTheme.xaml" : "/Themes/ColorTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
            isLight = !isLight;
        }
    }
}
