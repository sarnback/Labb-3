using Caliburn.Micro;
using QuizApp.DataHandler;
using QuizApp.Views;

namespace QuizApp.ViewModels
{
    public class ShellViewModel : Conductor<object>.Collection.OneActive
    {
        public ShellViewModel()
        {
            ActivateItemAsync(new MenuViewModel());
            SqlDataHandler sqlDataHandler = new SqlDataHandler();
        }

        public void GOHomeCommand()
        {
            if (Items.Count > 1)
            {
                ActivateItemAsync(new MenuViewModel());
                var homePage = Items[0];
                Items.Clear();
                ActivateItemAsync(homePage);
            }
        }

        public void GOBackCommand()
        {
            if (Items.Count > 1)
            {
                Items.RemoveAt(Items.Count - 1);
                var index = Items.Count - 1;
                ActivateItemAsync(Items[index]);
            }
        }

    }
}
