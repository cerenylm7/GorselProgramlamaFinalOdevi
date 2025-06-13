using System;
using Microsoft.Maui.Controls;
using GorselProgramlamaFinalOdevi.Models;
namespace GorselProgramlamaFinalOdevi;

public partial class TodoDetailPage : ContentPage
{
    public TodoDetailPage(MyTask task)
    {
        InitializeComponent();

        this.Content.BindingContext = task;
    }
}