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

namespace QuizWpf.Pages
{
    /// <summary>
    /// Interaction logic for AddQuestionPage.xaml
    /// </summary>
    public partial class AddQuestionPage : Page
    {
        public AddQuestionPage()
        {
            InitializeComponent();
        }

        private void CreateBtnClick(object sender, RoutedEventArgs e)
        {
            if (AnswerTextBox.Text.Length > 40)
            {
                MessageBox.Show("Ответ превышет допустимую длину!");
                return;
            }
            else
            {
                Core.MongoDbConnection.AddToDatabse(new Core.Question(QuestionTextBox.Text, AnswerTextBox.Text));
                AnswerTextBox.Text = "";
                QuestionTextBox.Text = "";
                MessageBox.Show("Успешно");
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
