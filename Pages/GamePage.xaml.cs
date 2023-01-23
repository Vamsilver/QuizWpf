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
using System.Windows.Threading;
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
        DispatcherTimer timer = new DispatcherTimer();

        Core.Complexity Complexity;
        int Lifes;
        Question CurrentQuestion;
        List<int> Sequence;
        int Pointer;

        public GamePage(int lifes, List<int> sequence, int pointer, Complexity complexity)
        {
            InitializeComponent();

            Lifes = lifes;
            Pointer = pointer;
            Sequence = sequence;
            Complexity = complexity;
            CurrentQuestion = MongoDbConnection.Find(Sequence.ElementAt(Pointer));
            QuestionTextBlock.Text = CurrentQuestion.Quaere;
            LifesTextBlock.Text = Lifes.ToString();
            AnswerCountTextBlock.Text = Pointer.ToString();

            Pointer++;

            if (Complexity == Complexity.Normal)
                Timer.Content = "";

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
            if (Count < CurrentQuestion.Answer.Length)
            {
                UsedButtons.Add(sender as Button);
                Answer[Count] = (sender as Button).Content.ToString().ToCharArray()[0];
                Count++;

                (sender as Button).IsEnabled = false;

                UpdateAnswersBtns();
            }
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            char[] me = Answer.Take(CurrentQuestion.Answer.Length).ToArray();
            char[] re = CurrentQuestion.Answer.ToUpper().ToCharArray();

            if (Enumerable.SequenceEqual(me, re))
            {
                if (Pointer == 10)
                {
                    MessageBox.Show("ПОЗДРАВЛЯЕМ ВЫ ПОБЕДИЛИ!");
                    timer.Stop();
                    NavigationService.Navigate(new MenuPage());
                }
                else
                {
                    if (Complexity == Complexity.Hard)
                        timer.Stop();
                    NavigationService.Navigate(new GamePage(Lifes, Sequence, Pointer, Complexity));
                }
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
                    timer.Stop();
                    NavigationService.Navigate(new MenuPage());
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

        private void GamePageLoaded(object sender, RoutedEventArgs e)
        {
            int TotalSeconds = 90;
            if (Complexity == Complexity.Hard)
            {
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (object? sender, EventArgs e) => 
                {
                    TotalSeconds--;
                    Timer.Content = (TotalSeconds / 60).ToString() + ":" + ((TotalSeconds % 60 < 10)? "0" : "") + (TotalSeconds % 60).ToString();
                    if(TotalSeconds == 0)
                    {
                        if(MessageBox.Show("Будте быстрее!", "Закончилось время", MessageBoxButton.OK) == MessageBoxResult.OK)
                        {
                            Lifes--;
                            timer.Stop();
                            NavigationService.Navigate(new GamePage(Lifes, Sequence, Pointer, Complexity));
                        }
                    }
                };
                timer.Start();
            }
        }

        private void SurrenderClick(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы уверенны?", "Подвердите поражение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                timer.Stop();
                NavigationService.Navigate(new MenuPage());
            }
        }
    }
}
