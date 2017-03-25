using Xamarin.Forms;

namespace Clima.View
{
    public partial class ClimaView : ContentPage
    {
        ViewModels.ClimaViewModel vm;
        public ClimaView()
        {
            InitializeComponent();
            vm = new ViewModels.ClimaViewModel();
            BindingContext = vm;
        }
    }
}
