using System.Text.Json;

namespace GorselProgramlamaFinalOdevi;

public partial class Weather : ContentPage
{
    // Þehirleri tutan liste
    private List<string> sehirler = new List<string>();

    // JSON dosya yolu (uygulama klasöründe "sehirler.json" olarak)
    private string jsonFilePath;

    public Weather()
    {
        InitializeComponent();

        // JSON dosya yolunu belirle
        jsonFilePath = Path.Combine(FileSystem.AppDataDirectory, "sehirler.json");

        // Uygulama açýlýrken JSON dosyasýndan verileri yükle
        LoadSehirlerFromJson();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        string sehir = textBox.Text?.Trim();

        if (string.IsNullOrEmpty(sehir))
            return;

        sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        sehir = sehir.Replace('Ç', 'C');
        sehir = sehir.Replace('Ð', 'G');
        sehir = sehir.Replace('Ý', 'I');
        sehir = sehir.Replace('Ö', 'O');
        sehir = sehir.Replace('Ü', 'U');
        sehir = sehir.Replace('Þ', 'S');

        if (sehirler.Contains(sehir))
        {
            DisplayAlert("Uyarý", "Bu þehir zaten ekli.", "Tamam");
            return;
        }

        // Listeye ekle
        sehirler.Add(sehir);

        // Görünümü güncelle
        AddCityToView(sehir);

        // JSON dosyasýna kaydet
        SaveSehirlerToJson();

        // TextBox temizle
        textBox.Text = string.Empty;
    }

    private void AddCityToView(string sehir)
    {
        string imageUrl = $"https://www.mgm.gov.tr/sunum/tahmin-klasik-5070.aspx?m={sehir}&basla=1&bitir=5&rC=111&rZ=fff";

        ImageButton newImage = new ImageButton
        {
            Source = imageUrl,
            HeightRequest = 200,
            WidthRequest = 400
        };

        Label text = new Label
        {
            Text = sehir,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 30,
        };

        newImage.Clicked += DeleteButton_Clicked;

        Microsoft.Maui.Controls.StackLayout imageLayout = new Microsoft.Maui.Controls.StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Spacing = 10
        };
        imageLayout.Children.Add(text);
        imageLayout.Children.Add(newImage);

        imageStackLayout.Children.Add(imageLayout);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        ImageButton deleteButton = (ImageButton)sender;
        Microsoft.Maui.Controls.StackLayout imageLayout = (Microsoft.Maui.Controls.StackLayout)deleteButton.Parent;

        // Silinen þehir adý
        Label label = (Label)imageLayout.Children[0];
        string sehir = label.Text;

        // Listeden kaldýr
        sehirler.Remove(sehir);

        // Görünümden kaldýr
        imageStackLayout.Children.Remove(imageLayout);

        // Güncel listeyi JSON dosyasýna kaydet
        SaveSehirlerToJson();
    }

    private void LoadSehirlerFromJson()
    {
        try
        {
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                sehirler = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

                // Yüklenen þehirleri görünümde göster
                foreach (var sehir in sehirler)
                {
                    AddCityToView(sehir);
                }
            }
        }
        catch (Exception ex)
        {
            // Hata varsa göstermek için
            DisplayAlert("Hata", "Þehirler yüklenirken hata oluþtu: " + ex.Message, "Tamam");
        }
    }

    private void SaveSehirlerToJson()
    {
        try
        {
            string json = JsonSerializer.Serialize(sehirler);
            File.WriteAllText(jsonFilePath, json);
        }
        catch (Exception ex)
        {
            DisplayAlert("Hata", "Þehirler kaydedilirken hata oluþtu: " + ex.Message, "Tamam");
        }
    }
}
