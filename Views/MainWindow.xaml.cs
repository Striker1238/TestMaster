using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using TestMaster.ViewModels;

namespace TestMaster.Views
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _isOnTestCreatorPage;
        public bool IsOnTestCreatorPage
        {
            get => _isOnTestCreatorPage;
            set { _isOnTestCreatorPage = value; OnPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigate(new TestMasterPage());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            IsOnTestCreatorPage = e.Content is TestCreatorPage;
        }

        private void OpenTestCreator_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TestCreatorPage());
        }

        private void BackToTests_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
