using Caliburn.Micro;
using Microsoft.Win32;
using QuizApp.DataHandler;
using QuizApp.Models;
using System;
using System.Windows;

namespace QuizApp.ViewModels
{
    public class CreateQuizViewModel : Screen
    {
        private string _imageSource { get; set; }
        public string ImageSource { get => _imageSource; set { _imageSource = value; NotifyOfPropertyChange(); } }

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

        public void OpenDialogCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF| All files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                ImageSource = dialog.FileName;
            }
        }

        public void CreateCommand()
        {
            if (string.IsNullOrEmpty(Question) || string.IsNullOrEmpty(Answers) ||
               string.IsNullOrEmpty(CorrectAnswer) || string.IsNullOrEmpty(ImageSource))
            {
                MessageBox.Show("one or more fields not set", "Error");
                return;
            }

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

            var sqlDataHandler = new SqlDataHandler();

            if (!Edit)
            {
                sqlDataHandler.AddNewQuiz(questions);
                MessageBox.Show("New Question Created");
                Answers = string.Empty;
                CorrectAnswer = string.Empty;
                Question = string.Empty;
                ImageSource = string.Empty;
            }

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
        public static bool AreDigitsOnly(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;

            int num = 0;
            return Int32.TryParse(text, out num);
        }
    }
}
