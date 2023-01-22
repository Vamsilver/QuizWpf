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
using QuizWpf.Core;

namespace QuizWpf.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        List<int> SequenceOfQuestions;

        public MenuPage()
        {
            InitializeComponent();
        }

        private void CreateQuestionClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddQuestionPage());
        }

        private void PlayNormalBtnClick(object sender, RoutedEventArgs e)
        {
            SequenceOfQuestions = (List<int>)RandomStirrer.RandomShuffle(Enumerable.Range(0, MongoDbConnection.GetQuantity()));

            NavigationService.Navigate(new GamePage(5, SequenceOfQuestions, 0, Core.Complexity.Normal));
        }

        private void PlayHardBtnClick(object sender, RoutedEventArgs e)
        {
            SequenceOfQuestions = (List<int>)RandomStirrer.RandomShuffle(Enumerable.Range(0, MongoDbConnection.GetQuantity()));

            NavigationService.Navigate(new GamePage(3, SequenceOfQuestions, 0, Core.Complexity.Hard));
        }
    }
}
