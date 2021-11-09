using Caliburn.Micro;
using QuizApp.DataHandler;
using QuizApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QuizApp.ViewModels
{
    public class QuizPlayViewModel : Screen
    {
        private ObservableCollection<Questions> _quizQuestions { get; set; }
        public ObservableCollection<Questions> QuizQuestions { get => _quizQuestions; set { _quizQuestions = value; NotifyOfPropertyChange(); } }

        private string _quizInfo { get; set; }
        public string QuizInfo { get => _quizInfo; set { _quizInfo = value; NotifyOfPropertyChange(); } }

        public QuizPlayViewModel()
        {
            QuizQuestions = new ObservableCollection<Questions>();
            LoadQuiz();
        }

        private void LoadQuiz()
        {
            var sqlDataHandler = new SqlDataHandler();

            Task.Run(async () =>
            {
                var data = await sqlDataHandler.LoadQuiz();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    QuizInfo = $"{data.Count} Quiz Questions";

                    foreach (var tempdat in data)
                    {
                        tempdat.AnswerList = new ObservableCollection<string>(tempdat.Answers.Split(',').ToList<string>());
                        tempdat.ImgSource = new BitmapImage(new Uri(@$"{tempdat.QuizImage}"));
                        tempdat.Answer = tempdat.AnswerList[tempdat.CorrectAnswer - 1];
                        QuizQuestions.Add(tempdat);
                    }
                });

            });
        }//



    }
}
