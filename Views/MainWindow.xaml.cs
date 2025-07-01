using System;
using System.Windows;
using System.Windows.Navigation;
using TestMaster.ViewModels;

namespace TestMaster.Views
{
    public partial class MainWindow : Window
    {
        private bool isLight = true;

        public MainWindow()
        {
            InitializeComponent();
           
            MainFrame.Navigate(new TestMasterPage());
        }
        private void OpenTestCreator_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TestCreatorPage());
        }
    }
}
