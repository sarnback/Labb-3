using Caliburn.Micro;
using QuizApp.ViewModels;
using System.Windows;

namespace QuizApp
{
    public class BootStrapper : BootstrapperBase
    {
        public BootStrapper()
        {
            Initialize();
        }
        //
        //startar ditt program.När du behöver distribuera ändringar i ditt program är det praktiskt att använda en bootstrapper
        //eftersom det är ett program som letar efter en uppdatering,
        //laddar ner den innan programmet startas
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

    }
}
