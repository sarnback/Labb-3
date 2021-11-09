using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace QuizApp.Models
{
    public class Questions
    {
        public int id { get; set; }
        public string QuestionText { get; set; }
        public string Answers { get; set; }
        public int CorrectAnswer { get; set; }
        public ObservableCollection<string> AnswerList { get; set; }
        public string Answer { get; set; }
        public string QuizImage { get; set; }
        public ImageSource ImgSource { get; set; }
    }

}
