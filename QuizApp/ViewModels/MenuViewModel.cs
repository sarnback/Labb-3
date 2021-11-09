using Caliburn.Micro;

namespace QuizApp.ViewModels
{
    public class MenuViewModel : Screen
    {
        public MenuViewModel()
        {
        }
        //Metod för komma in i Quizplayview och spela quiz
        public void PlayCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new QuizPlayViewModel());
        }
        //Metod för komma in i Quizplayview och skapa quiz
        public void CreateCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new CreateQuizViewModel());
        }
        //MEtod för komma in i Quizplayview och editera quiz
        public void EditCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new EditQuizViewModel());
        }
    }
}
