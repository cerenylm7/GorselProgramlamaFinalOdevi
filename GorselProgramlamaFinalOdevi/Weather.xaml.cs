using System.Text.Json;

namespace GorselProgramlamaFinalOdevi;

public partial class Weather : ContentPage
{
    // �ehirleri tutan liste
    private List<string> sehirler = new List<string>();

    // JSON dosya yolu (uygulama klas�r�nde "sehirler.json" olarak)
    private string jsonFilePath;

    public Weather()
    {
        InitializeComponent();

        // JSON dosya yolunu belirle
        jsonFilePath = Path.Combine(FileSystem.AppDataDirectory, "sehirler.json");

        // Uygulama a��l�rken JSON dosyas�ndan verileri y�kle
        LoadSehirlerFromJson();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        string sehir = textBox.Text?.Trim();

        if (string.IsNullOrEmpty(sehir))
            return;

        sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        sehir = sehir.Replace('�', 'C');
        sehir = sehir.Replace('�', 'G');
        sehir = sehir.Replace('�', 'I');
        sehir = sehir.Replace('�', 'O');
        sehir = sehir.Replace('�', 'U');
        sehir = sehir.Replace('�', 'S');

        if (sehirler.Contains(sehir))
        {
            DisplayAlert("Uyar�", "Bu �ehir zaten ekli.", "Tamam");
            return;
        }

        // Listeye ekle
        sehirler.Add(sehir);

        // G�r�n�m� g�ncelle
        AddCityToView(sehir);

        // JSON dosyas�na kaydet
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

        // Silinen �ehir ad�
        Label label = (Label)imageLayout.Children[0];
        string sehir = label.Text;

        // Listeden kald�r
        sehirler.Remove(sehir);

        // G�r�n�mden kald�r
        imageStackLayout.Children.Remove(imageLayout);

        // G�ncel listeyi JSON dosyas�na kaydet
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

                // Y�klenen �ehirleri g�r�n�mde g�ster
                foreach (var sehir in sehirler)
                {
                    AddCityToView(sehir);
                }
            }
        }
        catch (Exception ex)
        {
            // Hata varsa g�stermek i�in
            DisplayAlert("Hata", "�ehirler y�klenirken hata olu�tu: " + ex.Message, "Tamam");
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
            DisplayAlert("Hata", "�ehirler kaydedilirken hata olu�tu: " + ex.Message, "Tamam");
        }
    }
}
