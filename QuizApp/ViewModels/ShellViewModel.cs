using Caliburn.Micro;
using QuizApp.DataHandler;
using QuizApp.Views;

namespace QuizApp.ViewModels
{
    //Använder Caliburn micro

    
    //implementering av Conductor som bara håller fast vid och aktiverar ett objekt åt gången
    public class ShellViewModel : Conductor<object>.Collection.OneActive
    {
        //KOnstruktor som aktiverar viewmodel menu - detta händer i start av program
        //gör en Referens av SQLdatahanter
        public ShellViewModel()
        {
            ActivateItemAsync(new MenuViewModel());
            SqlDataHandler sqlDataHandler = new SqlDataHandler();
        }
        //HOME MENU COMMAND Går bakåt till menuviewmodel och uppdatar ändrignar
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
        //Går tillbaka command går tillbaka ett steg där duv ar
        public void GOBackCommand()
        {
            //Kollar vilka vyer du var i
            if (Items.Count > 1)
            {
                //Tar bort den som är aktuell(-1) för att gå tillbka
                Items.RemoveAt(Items.Count - 1);
                var index = Items.Count - 1;
                ActivateItemAsync(Items[index]);
            }
        }

    }
}
