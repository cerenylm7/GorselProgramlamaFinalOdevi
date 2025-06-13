using GorselProgramlamaFinalOdevi.Models;
namespace GorselProgramlamaFinalOdevi
{

    // kullanıcı giriş ve kayıt işlemlerini yönetir
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
        // Giriş ve kayıt ekranları arasında geçiş yapmayı sağlayan buton olay metodu
        private void ShowScreen(object sender, EventArgs e)
        {
            if (loginStack.IsVisible)
            {
                loginStack.IsVisible = false;
                registerStack.IsVisible = true;
            }
            else
            {
                loginStack.IsVisible = true;
                registerStack.IsVisible = false;
            }
        }

        // Kayıt ol butonuna tıklanıldığında çalışan metot
        private async void RegisterClicked(object sender, EventArgs e)
        {
            var user = await FBService.Register(r_username.Text, r_email.Text, r_password.Text);
            if (user != null)
            {
                if (Application.Current.MainPage is AppShell shell)
                {
                    shell.SetFlyoutBehavior(FlyoutBehavior.Flyout);
                    shell.OpenFlyout();
                }

                await DisplayAlert("Başarılı", $"Hoş Geldin {user.Info.DisplayName}", "OK");
                await Shell.Current.GoToAsync("//Home");
            }
            else
            {
                await DisplayAlert("Hata", "Kayıt Başarısız", "OK");
            }
        }
        // Giriş yap butonuna tıklanıldığında çalışan metot
        private async void LoginClicked(object sender, EventArgs e)
        {
            var user = await FBService.Login(l_email.Text, l_password.Text);
            if (user != null)
            {
                if (Application.Current.MainPage is AppShell shell)
                {
                    shell.SetFlyoutBehavior(FlyoutBehavior.Flyout);
                    shell.OpenFlyout();
                }

                await DisplayAlert("Başarılı", $"Hoş Geldin {user.Info.DisplayName}", "OK");

                await Shell.Current.GoToAsync("//Home");


            }
            else
            {
                await DisplayAlert("Hata", "Kullanıcı Adı Veya Şifre Yanlış", "OK");
            }
        }

    }
}
