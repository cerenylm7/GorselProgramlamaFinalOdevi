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
        // JSON dosyas�n�n tam yolu (uygulama veri klas�r� i�inde)
        string fileName => Path.Combine(FileSystem.AppDataDirectory, "myTasks.json");

        public TodoList()
        {
            InitializeComponent();


            // Uygulama a��ld���nda daha �nce kaydedilmi� g�revleri JSON dosyas�ndan y�kle
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
        // Her g�rev i�in detay butonuna t�klan�nca �al��an olay
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
        // Yeni g�rev ekleme butonuna bas�ld���nda �al���r
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
        // Kaydet butonuna bas�ld���nda g�rev listesini dosyaya kaydeder
        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            var task = (sender as Button).CommandParameter as MyTask;
            var res = await DisplayAlert("Sil", "G�revi silmek istedi�inize emin misiniz?", "Evet", "Hay�r");

            if (res == true)
            {
                myTasks.Remove(task);
            }

        }
        // Sil butonuna t�klan�nca se�ilen g�revi siler (kullan�c� onaylarsa)
        private async void Share_Click(object sender, EventArgs e)
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "G�revlerim",
                File = new ShareFile(fileName)
            });
        }
        // Her g�revi ayr� ayr� metin olarak payla�mak i�in buton
        private async void ButtonShare_Clicked(object sender, EventArgs e)
        {
            var task = (sender as Button).CommandParameter as MyTask;
            await Share.Default.RequestAsync(new ShareTextRequest()
            {
                Title = "G�revlerim",
                Text = task.ToString(),
            });
        }
    }
}
