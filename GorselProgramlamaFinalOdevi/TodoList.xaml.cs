using GorselProgramlamaFinalOdevi.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace GorselProgramlamaFinalOdevi
{
    public partial class TodoList : ContentPage
    {
        ObservableCollection<MyTask> myTasks = new(){
         
        };
        // JSON dosyasýnýn tam yolu (uygulama veri klasörü içinde)
        string fileName => Path.Combine(FileSystem.AppDataDirectory, "myTasks.json");

        public TodoList()
        {
            InitializeComponent();


            // Uygulama açýldýðýnda daha önce kaydedilmiþ görevleri JSON dosyasýndan yükle
            if (File.Exists(fileName))
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        var loadedTasks = JsonSerializer.Deserialize<ObservableCollection<MyTask>>(json);
                        if (loadedTasks != null)
                            myTasks = loadedTasks;
                    }
                }
            }

            listTasks.ItemsSource = myTasks;
        }
        // Her görev için detay butonuna týklanýnca çalýþan olay
        private void ButtonDetail_Clicked(object sender, EventArgs e)
        {
            var task = (sender as Button).CommandParameter as MyTask;

            var page = new TodoDetailPage(task);
            Navigation.PushAsync(page);
        }

        private void AddNewTask_Click(object sender, EventArgs e)
        {
            var task = new MyTask();
            var page = new TodoDetailPage(task);
            Navigation.PushAsync(page);
            myTasks.Add(task);

        }
        // Yeni görev ekleme butonuna basýldýðýnda çalýþýr
        private void Save_Click(object sender, EventArgs e)
        {
            //save myTask list to json file
            var json = JsonSerializer.Serialize(myTasks);
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                }
            }

        }
        // Kaydet butonuna basýldýðýnda görev listesini dosyaya kaydeder
        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            var task = (sender as Button).CommandParameter as MyTask;
            var res = await DisplayAlert("Sil", "Görevi silmek istediðinize emin misiniz?", "Evet", "Hayýr");

            if (res == true)
            {
                myTasks.Remove(task);
            }

        }
        // Sil butonuna týklanýnca seçilen görevi siler (kullanýcý onaylarsa)
        private async void Share_Click(object sender, EventArgs e)
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Görevlerim",
                File = new ShareFile(fileName)
            });
        }
        // Her görevi ayrý ayrý metin olarak paylaþmak için buton
        private async void ButtonShare_Clicked(object sender, EventArgs e)
        {
            var task = (sender as Button).CommandParameter as MyTask;
            await Share.Default.RequestAsync(new ShareTextRequest()
            {
                Title = "Görevlerim",
                Text = task.ToString(),
            });
        }
    }
}
