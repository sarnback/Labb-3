using Caliburn.Micro;

namespace QuizApp.ViewModels
{
    public class MenuViewModel : Screen
    {
        public MenuViewModel()
        {
        }

        public void PlayCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new QuizPlayViewModel());
        }

        public void CreateCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new CreateQuizViewModel());
        }

        public void EditCommand()
        {
            var conductor = this.Parent as IConductor;
            conductor.ActivateItemAsync(new EditQuizViewModel());
        }
    }
}
