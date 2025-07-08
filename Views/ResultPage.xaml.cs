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
using TestMaster.Models.App;
using TestMaster.ViewModels;

namespace TestMaster.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        public ResultPage(Result result)
        {
            InitializeComponent();
            DataContext = new ResultViewModel(result);

            FullNameTextBox.Text = result.FullName;
            PersonnelNumberTextBox.Text = result.PersonnelNumber;
            TestTitleTextBox.Text = result.ComplatedTest.Title;
            TestDescriptionTextBox.Text = result.ComplatedTest.Description;
            TestCategoryTextBox.Text = result.ComplatedTest.Category;
            TestCountQuestionsTextBox.Text = result.CountQuestions.ToString();
            TestCountCorrectAnswerTextBox.Text = result.CountCorrectAnswer.ToString();
            TestPrecentTextBox.Text = result.CountQuestions > 0
                ? ((double)result.CountCorrectAnswer / result.CountQuestions * 100).ToString("F2") + "%"
                : "0%";

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
