using Caliburn.Micro;
using Microsoft.Win32;
using QuizApp.DataHandler;
using QuizApp.Models;
using System;
using System.Windows;

namespace QuizApp.ViewModels
{
    //Klass createQUizviewmodel
    public class CreateQuizViewModel : Screen
    {
        //Field för bild
        private string _imageSource { get; set; }
        //Sätter ovan field till denna publika prop och när NotifyPropertyChanged
        public string ImageSource { get => _imageSource; set { _imageSource = value; NotifyOfPropertyChange(); } }

        //Korrekt svar 
        private string _correctAnswer { get; set; }
        public string CorrectAnswer
        {
            get => _correctAnswer; set
            {
                if (value.AreDigitsOnly())
                    _correctAnswer = value;
                NotifyOfPropertyChange();
            }
        }

        private string _question { get; set; }
        public string Question { get => _question; set { _question = value; NotifyOfPropertyChange(); } }

        private string _answers { get; set; }
        public string Answers { get => _answers; set { _answers = value; NotifyOfPropertyChange(); } }

        private bool Edit = false;
        int ID = 0;
        //konstruktor för klassen som kollar om questions = null, om den är det så sker det nedan.
        public CreateQuizViewModel(Questions questions = null)
        {
            if (questions != null)
            {
                Answers = questions.Answers;
                CorrectAnswer = questions.CorrectAnswer.ToString();
                ImageSource = questions.QuizImage;
                Question = questions.QuestionText;
                Edit = true;
                ID = questions.id;
            }
        }
        //öppnar en dialog box som låter användaren välja en film av file r nedan OM sant sätt Imagesourse till användarens fil
        public void OpenDialogCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF| All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                ImageSource = dialog.FileName;
            }
        }
        //Kollar så alla fält är sätta, då jag valt att man måste ha alla dessa krav för skapa en fråga annar skommer messagebox show
        public void CreateCommand()
        {
            if (string.IsNullOrEmpty(Question) || string.IsNullOrEmpty(Answers) ||
               string.IsNullOrEmpty(CorrectAnswer) || string.IsNullOrEmpty(ImageSource))
            {
                MessageBox.Show("one or more fields not set", "Error");
                return;
            }
            //splittar svar efter ett ","
            var tempString = Answers.Trim().Split(',');

            if (CorrectAnswer.Trim() == "0" || !Answers.Contains(",") || Int32.Parse(CorrectAnswer) > tempString.Length)
            {
                MessageBox.Show("invalid Correct answer");
                return;
            }

            var questions = new Questions
            {
                Answer = Answers,
                CorrectAnswer = Int32.Parse(CorrectAnswer),
                QuizImage = ImageSource,
                QuestionText = Question
            };

            //Referens till sqlDatahandler för adda quiz
            var sqlDataHandler = new SqlDataHandler();
            //OM INTE EDIT SÅ sätter vi Answers,correctanswe,quesiton och bild till tomt värde
            if (!Edit)
            {
                sqlDataHandler.AddNewQuiz(questions);
                MessageBox.Show("New Question Created");
                Answers = string.Empty;
                CorrectAnswer = string.Empty;
                Question = string.Empty;
                ImageSource = string.Empty;
            }
            //ANNARS anropa metod UpdateQUiz och säg med messagebox att quiz är uppdaterat
            else
            {
                questions.id = ID;
                sqlDataHandler.UpdateQuiz(questions);
                MessageBox.Show("Quiz Updated");
            }

        }//
    }

    public static class StringExtensions
    {
        //Kollar så ens svarsalternativ är utav heltal
        public static bool AreDigitsOnly(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            int num = 0;
            return Int32.TryParse(text, out num);
        }
    }
}
