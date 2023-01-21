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
    public partial class GamePage : Page
    {
        List<Button> UsedButtons = new List<Button>();
        Char[] Answer = new string(' ', 40).ToCharArray();
        char[] letters = new char[40];
        int Count = 0;
        int NumberOfHints = 2;

        int Lifes;
        Question CurrentQuestion;
        List<int> Sequence;
        int Pointer;

        public GamePage(int lifes, List<int> sequence, int pointer)
        {
            if(pointer == 9)
            {
                MessageBox.Show("ПОЗДРАВЛЕЯМ ВЫ ПОБЕДИЛИ!");
                NavigationService.Navigate(new MenuPage());
            }
            InitializeComponent();

            Lifes = lifes;

            Pointer = pointer;
            Sequence = sequence;
            CurrentQuestion = MongoDbConnection.Find(Pointer);
            Pointer++;
            QuestionTextBlock.Text = CurrentQuestion.Quaere;
            LifesTextBlock.Text = Lifes.ToString();

            AddAnswerButtons();
            FillLettersArray();
            AddLettersButtons();
        }

        public GamePage()
        {
            InitializeComponent();
        }

        private void LetterButtonClick(object sender, RoutedEventArgs e)
        {
            UsedButtons.Add(sender as Button);
            Answer[Count] = (sender as Button).Content.ToString().ToCharArray()[0];
            Count++;

            (sender as Button).IsEnabled = false;

            UpdateAnswersBtns();
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            char[] me = Answer.Take(CurrentQuestion.Answer.Length).ToArray();
            char[] re = CurrentQuestion.Answer.ToUpper().ToCharArray();

            if (Enumerable.SequenceEqual(me, re))
            {
                if (Lifes < 5)
                {
                    Lifes++;
                    LifesTextBlock.Text = Lifes.ToString();
                }

                NavigationService.Navigate(new GamePage(Lifes, Sequence, Pointer));
            }
            else
            {
                if(Lifes != 1)
                {
                    Lifes--;
                    LifesTextBlock.Text = Lifes.ToString();
                    MessageBox.Show("Неправильно, попробуйте по другому!");
                }
                else
                {
                    MessageBox.Show("Вы проиграли, попробуйте еще раз!");
                    NavigationService.GoBack();
                }
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if(Count > 0)
            {
                Answer[Count - 1] = ' ';
                Count--;
                UsedButtons.Last().IsEnabled = true;
                UsedButtons.Remove(UsedButtons.Last());
                UpdateAnswersBtns();
            }
        }

        private void UpdateAnswersBtns()
        {
            int newCount = 0;
            foreach (StackPanel stack in AnswerStackPanel.Children)
            {
                foreach (var btn in stack.Children)
                {
                    (btn as Button).Content = Answer[newCount];
                    newCount++;
                }
            }

        }

        private void AddAnswerButtons()
        {
            for (int i = 0; i < CurrentQuestion.Answer.Length; i++)
            {
                var button = new Button();
                button.Content = Answer[i];
                button.Width = 70;
                button.Height = 70;
                button.FontSize = 16;
                button.Margin = new Thickness(0, 0, 10, 0);
                
                if(i < 10)
                {
                    AnswerStackPanel1.Children.Add(button);
                }
                else if(i < 20)
                {
                    AnswerStackPanel2.Children.Add(button);
                }
                else if(i < 30)
                {
                    AnswerStackPanel3.Children.Add(button);
                }
                else
                    AnswerStackPanel4.Children.Add(button);

            }
        }

        private void AddLettersButtons()
        {
            int newCount = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var button = new Button();
                    button.Click += LetterButtonClick;
                    button.Width = 70;
                    button.Height = 70;
                    button.FontSize = 16;
                    button.Content = letters[newCount].ToString();
                    button.Margin = new Thickness(0, 0, 10, 0);
                    switch (i)
                    {
                        case 0:
                            StackLetters1.Children.Add(button);
                            break;

                        case 1:
                            StackLetters2.Children.Add(button);
                            break;

                        case 2:
                            StackLetters3.Children.Add(button);
                            break;

                        case 3:
                            StackLetters4.Children.Add(button);
                            break;
                    }
                    newCount++;
                }
            }
        }

        private void FillLettersArray()
        {
            Random random = new Random();

            for (int i = 0; i < CurrentQuestion.Answer.Length; i++)
            {
                int num = random.Next(0, 40);
                if (letters[num] == '\0')
                {
                    letters[num] = Char.ToUpper(CurrentQuestion.Answer[i]);
                }
                else
                    i--;
            }

            for (int i = 0; i < 40; i++)
            {
                if (letters[i] == '\0')
                {
                    letters[i] = (char)random.Next('А', 'Я');
                }
            }
        }
    }
}
