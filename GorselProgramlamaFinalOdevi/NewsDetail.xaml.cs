namespace GorselProgramlamaFinalOdevi;

public partial class NewsDetail : ContentPage
{
    string haberUrl;

    public NewsDetail(string url)
    {
        InitializeComponent();
        haberUrl = url;
        webView.Source = url;
    }
    // Paylaþ butonuna týklandýðýnda çalýþan olay metodu
    private async void ShareNews_Clicked(object sender, EventArgs e)
    {
        await ShareUri(haberUrl, Share.Default);
    }
    // Paylaþma iþlemini gerçekleþtiren metot
    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Haberi Paylaþ",
            Text = "Bu haberi ilginç buldum, sen de bir göz at: " + uri
        });
    }
}
