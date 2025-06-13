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
    // Payla� butonuna t�kland���nda �al��an olay metodu
    private async void ShareNews_Clicked(object sender, EventArgs e)
    {
        await ShareUri(haberUrl, Share.Default);
    }
    // Payla�ma i�lemini ger�ekle�tiren metot
    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Haberi Payla�",
            Text = "Bu haberi ilgin� buldum, sen de bir g�z at: " + uri
        });
    }
}
