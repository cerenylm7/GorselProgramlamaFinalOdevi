using Microsoft.Maui.Controls;

namespace GorselProgramlamaFinalOdevi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public void SetFlyoutBehavior(FlyoutBehavior behavior)
        {
            this.FlyoutBehavior = behavior;
        }

        public void OpenFlyout()
        {
            this.FlyoutIsPresented = true;
       
        }
        // kullanıcı sol üst menüden hesaptan çıkış yapmak isterse:
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            bool confirm = await Shell.Current.DisplayAlert("Çıkış", "Oturumu kapatmak istiyor musunuz?", "Evet", "Hayır");
            if (confirm)
            {
               

               
                await Shell.Current.GoToAsync("//MainPage");

                
                if (Application.Current.MainPage is AppShell shell)
                {
                    shell.SetFlyoutBehavior(FlyoutBehavior.Disabled);
                }
            }
        }
    }
}
