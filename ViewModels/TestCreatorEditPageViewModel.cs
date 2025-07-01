using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestMaster.Commands;
using TestMaster.Models;

namespace TestMaster.ViewModels
{
    class TestCreatorEditPageViewModel
    {
        public ICommand AddNewQuestionCommand { get; set; }
        public ICommand EditQuestionCommand { get; set; }
        public Test CreatedTest { get; set; }

        public TestCreatorEditPageViewModel(Test? selectTest)
        {
            AddNewQuestionCommand = new RelayCommand(_ => CreatedNewQuestion(), _ => true);
            EditQuestionCommand = new RelayCommand(_ => EditedQuestion(), _ => true);

            CreatedTest = selectTest ?? new Test();
        }

        public void OnCreatedNewTest()
        {
            CreatedTest = new Test();
        }
        public void CreatedNewQuestion()
        {

        }
        public void EditedQuestion()
        {

        }
    }
}
