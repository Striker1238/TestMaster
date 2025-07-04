using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для TestCreatorEditPage.xaml
    /// </summary>
    public partial class TestCreatorEditPage : Page
    {
        private static readonly Regex _regex = new Regex("^[0-9][0-9]*$");

        public TestCreatorEditPage(Test? SelectTest = null)
        {
            InitializeComponent();
            DataContext = new TestCreatorEditPageViewModel(SelectTest);
            if(SelectTest is not null)
            {
                TestNameTextBox.AppendText(SelectTest.Title);
                CategoryTextBox.AppendText(SelectTest.Category);
                TestDescriptionTextBox.AppendText(SelectTest.Description);
                QuestionsCountTextBox.AppendText(SelectTest.NumberQuestions.ToString());
                CorrectAnswersCountTextBox.AppendText(SelectTest.NumberQuestions.ToString());
                ShuffleQuestionsCheckBox.IsChecked = SelectTest.IsShuffleQuestions;
                ShuffleAnswersCheckBox.IsChecked = SelectTest.IsShuffleAnswers;

            }
        }

        private void CorrectAnswersCountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(((TextBox)sender).Text, e.Text);
        }

        private void CorrectAnswersCountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private bool IsTextAllowed(string currentText, string newText)
        {
            string result = currentText.Insert(CorrectAnswersCountTextBox.SelectionStart, newText);
            return string.IsNullOrEmpty(result) || _regex.IsMatch(result);
        }
    }
}
