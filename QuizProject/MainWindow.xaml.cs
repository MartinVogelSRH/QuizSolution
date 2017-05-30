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

namespace QuizProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        int intCorrectAnswers;
        int intQuestionsCount;
        List<Question> questions;
        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //List<Question> questions = new List<Question>()
            //{
            //    new Question()
            //    {
            //    QuestionID = 1,
            //    QuestionText = "Why are you not using Intellisense?",
            //    Answers = new List<Answer>()
            //    {
            //        new Answer()
            //        {
            //            AnswerID = 1,
            //            AnswerText = "I am too lazy",
            //            CorrectAnswer = true
            //        },
            //        new Answer()
            //        {
            //            AnswerID = 2,
            //            AnswerText = "I do not know how",
            //            CorrectAnswer = false
            //        },
            //        new Answer()
            //        {
            //            AnswerID = 3,
            //            AnswerText = "I am not used to",
            //            CorrectAnswer = true
            //        }
            //    }
            //    }
            //};
            //MyStorage.storeXML<List<Question>>(questions, "Questions.xml");
            intCorrectAnswers = 0;
            questions = MyStorage.readXML<List<Question>>("Questions.xml");
            intQuestionsCount = questions.Count;
            span_Questions.DataContext = questions[rand.Next(0, questions.Count - 1)];
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (lbx_Answers.SelectedItem != null)
            {
                Answer givenAnswer = (Answer)lbx_Answers.SelectedItem;
                if (givenAnswer.CorrectAnswer == true)
                {
                    MessageBox.Show("Correct");
                    intCorrectAnswers++;
                }
                else
                {
                    List<Answer> correctAnswers = (from a in (List<Answer>)lbx_Answers.ItemsSource where a.CorrectAnswer.Equals(true) select a).ToList();
                    StringBuilder strb_Message = new StringBuilder();
                    strb_Message.AppendLine("The correct answer(s) is/are:");
                    foreach (Answer item in correctAnswers)
                    {
                        strb_Message.AppendLine(item.AnswerText);
                    }
                    MessageBox.Show(strb_Message.ToString(), "WRONG!");
                }
                if (questions.Count > 1)
                {
                    //currentQuestion++;
                    questions.Remove((Question)span_Questions.DataContext);
                    span_Questions.DataContext = questions[rand.Next(0, questions.Count - 1)];
                }
                else
                {
                    if(MessageBox.Show("You completed this Quiz \r\nCorrect Answers: " + intCorrectAnswers.ToString() + "/" + intQuestionsCount.ToString() + "\r\nDo you want to start again?", "Done",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Window_Loaded(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Ok, Bye!");
                        this.Close();
                    }

                    
                }
            }
            else
            {
                MessageBox.Show("Please select an answer");
            }
        }

        private void lbx_Answers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_OK_Click(null, null);
            }
        }
    }
}
