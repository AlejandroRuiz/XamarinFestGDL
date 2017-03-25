using Clima.View;
using Clima.ViewModels;
using Xamarin.Forms;
using static System.Diagnostics.Debug;

namespace Clima
{
    public class App : Application
    {
        public App()
        {
            MainPage = new ClimaView();
            //new NavigationPage(new ClimaView())
            //{
            //    BarBackgroundColor = Color.FromHex("3498db"),
            //    BarTextColor = Color.Black
            //};
        }

        protected override void OnStart()
        {
            base.OnStart();
            WriteLine("Application OnStart");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            WriteLine("Application OnSleep");
        }

        protected override void OnResume()
        {
            base.OnResume();
            WriteLine("Application OnResume");
        }
    }
}

