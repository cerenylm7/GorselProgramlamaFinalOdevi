using GorselProgramlamaFinalOdevi.Models;
using System.Text.Json;

namespace GorselProgramlamaFinalOdevi;

public partial class Exchange : ContentPage
{
    public Exchange()
    {
        InitializeComponent();
    }
    // Singleton pattern uygulamas�: sayfan�n sadece bir �rne�inin olmas� i�in
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
    // Sayfa g�r�n�r oldu�unda otomatik olarak d�viz verilerini y�kler
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await Load();
    }

    // D�viz kurlar�n� API'den �ekip aray�ze yerle�tiren metot
    async Task Load()
    {
        try
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;


            // API'den g�ncel d�viz kurlar�n� JSON format�nda �ek
            string jsondata = await getCurrentExchangeRates();

            //b�y�k k���k harf duyarl�l���(jsonda)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using JsonDocument doc = JsonDocument.Parse(jsondata);
            JsonElement root = doc.RootElement;

            var currencies = new List<Currency>();

            // JSON i�indeki her d�viz kuru bilgisini okuduk ve listeye ekledik.
            foreach (JsonProperty property in root.EnumerateObject())
            {
                if (property.Name != "Update_Date")
                {
                    // D�viz al��,sat��,de�i�im oran� ayr� ayr� varsa al, yoksa "N/A" olarak ata k�sm�.
                    string purchase = property.Value.TryGetProperty("Al��", out var purchaseValue)
                                      ? purchaseValue.GetString() : "N/A";

                    string sale = property.Value.TryGetProperty("Sat��", out var saleValue)
                                  ? saleValue.GetString() : "N/A";

                    string difference = property.Value.TryGetProperty("De�i�im", out var differenceValue)
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
            // Y�kleniyor g�stergesini kapat
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }
    // API'den d�viz kurlar�n� JSON olarak �ekenmetot
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
    //G�ncelle butonuna t�klad���m�zda �a�r�lan meetotdur.
    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        await Load();
    }

}
