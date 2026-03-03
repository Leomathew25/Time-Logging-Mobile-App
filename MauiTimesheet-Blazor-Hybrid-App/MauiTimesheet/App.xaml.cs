using MauiTimesheet.Services;

namespace MauiTimesheet
{
    public partial class App : Application
    {
        public App(AuthService authService)
        {
            InitializeComponent();

            authService.Initialize();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MauiTimesheet" };
        }
    }
}
