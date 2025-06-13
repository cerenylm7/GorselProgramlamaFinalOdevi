using GorselProgramlamaFinalOdevi.Models;
using System.Text.Json;

namespace GorselProgramlamaFinalOdevi;

public partial class Exchange : ContentPage
{
    public Exchange()
    {
        InitializeComponent();
    }
    // Singleton pattern uygulamasý: sayfanýn sadece bir örneðinin olmasý için
    private static Exchange instance;
    public static Exchange Page
    {
        get
        {
            if (instance == null)
                instance = new Exchange();
            return instance;
        }
    }
    // Sayfa görünür olduðunda otomatik olarak döviz verilerini yükler
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await Load();
    }

    // Döviz kurlarýný API'den çekip arayüze yerleþtiren metot
    async Task Load()
    {
        try
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;


            // API'den güncel döviz kurlarýný JSON formatýnda çek
            string jsondata = await getCurrentExchangeRates();

            //büyük küçük harf duyarlýlýðý(jsonda)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using JsonDocument doc = JsonDocument.Parse(jsondata);
            JsonElement root = doc.RootElement;

            var currencies = new List<Currency>();

            // JSON içindeki her döviz kuru bilgisini okuduk ve listeye ekledik.
            foreach (JsonProperty property in root.EnumerateObject())
            {
                if (property.Name != "Update_Date")
                {
                    // Döviz alýþ,satýþ,deðiþim oraný ayrý ayrý varsa al, yoksa "N/A" olarak ata kýsmý.
                    string purchase = property.Value.TryGetProperty("Alýþ", out var purchaseValue)
                                      ? purchaseValue.GetString() : "N/A";

                    string sale = property.Value.TryGetProperty("Satýþ", out var saleValue)
                                  ? saleValue.GetString() : "N/A";

                    string difference = property.Value.TryGetProperty("Deðiþim", out var differenceValue)
                                        ? differenceValue.GetString() : "N/A";

                    var currency = new Currency
                    {
                        Name = property.Name,
                        Purchase = purchase,
                        Sale = sale,
                        Difference = difference
                    };
                    currencies.Add(currency);
                }
            }

            currency_list.ItemsSource = currencies;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            // Yükleniyor göstergesini kapat
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }
    // API'den döviz kurlarýný JSON olarak çekenmetot
    async Task<string> getCurrentExchangeRates()
    {
        string url = "https://finans.truncgil.com/today.json";
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string jsondata = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsondata);
        return jsondata;
    }
    //Güncelle butonuna týkladýðýmýzda çaðrýlan meetotdur.
    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await Load();
    }

}
