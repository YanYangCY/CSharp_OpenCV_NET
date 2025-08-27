using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CSharp_OpenCV_NET.Model;

namespace CSharp_OpenCV_NET.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _newTitle = "";

        public ObservableCollection<Todo> Items { get; } = new();

        [RelayCommand]
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(NewTitle)) 
                return;
            Items.Add(new Todo { Title = NewTitle });
            NewTitle = string.Empty;
        }

        [RelayCommand]
        private void Remove(Todo item) => Items.Remove(item);
    }
}
