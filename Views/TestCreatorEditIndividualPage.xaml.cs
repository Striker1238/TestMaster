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
    /// Логика взаимодействия для TestCreatorEditIndividualPage.xaml
    /// </summary>
    public partial class TestCreatorEditIndividualPage : Page
    {
        public TestCreatorEditIndividualPage() : this(null,null) { }
        public TestCreatorEditIndividualPage(Test SelectTest) : this(SelectTest, null) { }
        public TestCreatorEditIndividualPage(Test SelectTest, IndividualTest? SelectIndividualTest)
        {
            InitializeComponent();
            DataContext = new EditIndividualPageViewModel(SelectTest, SelectIndividualTest);

            //if (SelectIndividualTest != null)
            //{
                //FullNameTextBox.Text = SelectIndividualTest.UserName;
                //PersonnelNumberTextBox.AppendText(SelectIndividualTest.PersonnelNumber);
                //CountQuestionTextBox.AppendText(SelectIndividualTest.CountQuestions.ToString());

           // }
        }
    }
}
