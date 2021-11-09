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
    {    //Obserablecollection r private
        private ObservableCollection<Questions> _quizQuestions { get; set; }
        //Obserablecollection som hämtar ovan och set på NotifyPropertyChanged, representerar dynamisk data som uppdateras när man ändrar något i listna QUizQuesitons
        public ObservableCollection<Questions> QuizQuestions { get => _quizQuestions; set { _quizQuestions = value; NotifyOfPropertyChange(); } }

        private string _quizInfo { get; set; }
        public string QuizInfo { get => _quizInfo; set { _quizInfo = value; NotifyOfPropertyChange(); } }
        //Konstruktor för QuizPlayviewModel 
        public QuizPlayViewModel()
        {
            QuizQuestions = new ObservableCollection<Questions>();
            LoadQuiz();
        }
        //Metod ladda quiz här visar programmet all sina frågor/svar ,korrekt svar osv. 
        private void LoadQuiz()
        {
            var sqlDataHandler = new SqlDataHandler();

            Task.Run(async () =>
            {
                var data = await sqlDataHandler.LoadQuiz();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    //Kollar hur många frågor det finns
                    QuizInfo = $"{data.Count} Quiz Questions";
                    //Visar data från frågor/Svar/bilder
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
