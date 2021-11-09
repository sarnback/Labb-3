using Caliburn.Micro;
using QuizApp.DataHandler;
using QuizApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuizApp.ViewModels
{
    //KLASS FÖR EDIT QUIZ VIEW MODEL
    class EditQuizViewModel : Screen
    {  //ObservableColleciton av objekt (Question) som ör privat
        private ObservableCollection<Questions> _quizQuestions { get; set; }
        //Obserablecollection som hämtar ovan och set på NotifyPropertyChanged, representerar dynamisk data som uppdateras när man ändrar något i listna QUizQuesitons
        public ObservableCollection<Questions> QuizQuestions { get => _quizQuestions; set { _quizQuestions = value; NotifyOfPropertyChange(); } }
        //Används för komma åt sqlDataHandler class för kunna Loada quiz deleta quiz 
        private SqlDataHandler sqlDataHandler;

        //Konstruktor  med referens för sqlDatahandler, och QUizQuestions, och metodanrop för Ladda quiz
        public EditQuizViewModel()
        {
            sqlDataHandler = new SqlDataHandler();
            QuizQuestions = new ObservableCollection<Questions>();
            LoadQuiz();
        }

        //LAdda quiz asynkront
        private void LoadQuiz()
        {
            Task.Run(async () =>
            {
                var data = await sqlDataHandler.LoadQuiz();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var tempdat in data)
                    {
                        //Instansierar en ny referens av BitmapImage klass för bilder
                        tempdat.ImgSource = new BitmapImage(new Uri(@$"{tempdat.QuizImage}"));
                        //Lägg till i quizquestions
                        QuizQuestions.Add(tempdat);
                    }
                });

            });
        }//
        //Metod för att delta quiz frågor eller hela quiz
        public void DeleteQuizCommand(object obj)
        {
            var Btn = (Button)obj;
            var container = (Grid)Btn.Parent;
            var textblock = container.Children[2] as TextBlock;
            var tempID = Int32.Parse(textblock.Text);
            QuizQuestions.Remove(QuizQuestions.FirstOrDefault(obj => obj.id == tempID));
            sqlDataHandler.DelteQuiz(tempID);
        }
        //Metod för Edietera quiz, ersätter gammla värden med de nya man ändrar till
        public void EditQuizCommand(object obj)
        {
            var Btn = (Button)obj;
            var container = (Grid)Btn.Parent;
            var textblock = container.Children[2] as TextBlock;
            var tempID = Int32.Parse(textblock.Text);
            var tempQuiz = QuizQuestions.FirstOrDefault(obj => obj.id == tempID);

            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new CreateQuizViewModel(tempQuiz));

        }
    }
}
