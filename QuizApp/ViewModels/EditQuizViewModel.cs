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
    class EditQuizViewModel : Screen
    {
        private ObservableCollection<Questions> _quizQuestions { get; set; }
        public ObservableCollection<Questions> QuizQuestions { get => _quizQuestions; set { _quizQuestions = value; NotifyOfPropertyChange(); } }

        private SqlDataHandler sqlDataHandler;

        public EditQuizViewModel()
        {
            sqlDataHandler = new SqlDataHandler();
            QuizQuestions = new ObservableCollection<Questions>();
            LoadQuiz();
        }

        private void LoadQuiz()
        {
            Task.Run(async () =>
            {
                var data = await sqlDataHandler.LoadQuiz();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var tempdat in data)
                    {
                        tempdat.ImgSource = new BitmapImage(new Uri(@$"{tempdat.QuizImage}"));
                        QuizQuestions.Add(tempdat);
                    }
                });

            });
        }//

        public void DeleteQuizCommand(object obj)
        {
            var Btn = (Button)obj;
            var container = (Grid)Btn.Parent;
            var textblock = container.Children[2] as TextBlock;
            var tempID = Int32.Parse(textblock.Text);
            QuizQuestions.Remove(QuizQuestions.FirstOrDefault(obj => obj.id == tempID));
            sqlDataHandler.DelteQuiz(tempID);
        }

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
