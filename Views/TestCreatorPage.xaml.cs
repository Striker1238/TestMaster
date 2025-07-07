using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TestMaster.ViewModels;

namespace TestMaster.Views
{
    /// <summary>
    /// Логика взаимодействия для TestCreatorWindow.xaml
    /// </summary>
    public partial class TestCreatorPage : Page
    {
        public TestCreatorViewModel ViewModel { get; private set; }

        public TestCreatorPage()
        {
            InitializeComponent();
            ViewModel = new TestCreatorViewModel();
            DataContext = ViewModel;
            TestCreator.Navigate(new TestCreatorHomePage(ViewModel));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика возврата на предыдущую страницу
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Нет предыдущей страницы для возврата.");
            }
        }
    }
}
