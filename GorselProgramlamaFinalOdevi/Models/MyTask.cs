using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace GorselProgramlamaFinalOdevi.Models
{
    public class MyTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string id, title, detail, firstName, lastName;
        private bool isCompleted;
        public DateTime startDate;
        public DateTime endDate;

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                    id = Guid.NewGuid().ToString();

                return id;
            }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                NotifyPropertyChanged();
            }
        }

        public string Detail
        {
            get => detail;
            set
            {
                detail = value;
                NotifyPropertyChanged();
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(FullName));
            }
        }

        [JsonIgnore]
        public string FullName => FirstName + " " + LastName;

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DateTimeInterval));
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DateTimeInterval));
            }
        }

        [JsonIgnore]
        public string DateTimeInterval =>
            "📅 " + StartDate.ToString("dd/MM/yyyy HH:mm") + " - " + EndDate.ToString("dd/MM/yyyy HH:mm");

        public override string ToString()
        {
            return $"Task: {Title}\n" +
                   $"Details: {Detail}\n" +
                   $"Assigned To: {FullName}\n" +
                   $"Interval: {DateTimeInterval}\n" +
                   $"Completed: {IsCompleted}\n";
        }
    }
}
