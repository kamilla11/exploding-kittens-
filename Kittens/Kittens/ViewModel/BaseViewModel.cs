using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kittens.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}

// public class BaseViewModel: INotifyPropertyChanged
// {
//     private bool isBusy;
//     private string title;
//     
//     public bool IsBusy
//     {
//         get => isBusy;
//         set
//         {
//             if (isBusy == value)
//                 return;
//             isBusy = value;
//             OnPropertyChanged();
//         }
//     }
//     
//     public string Title
//     {
//         get => title;
//         set
//         {
//             if (title == value)
//                 return;
//             title = value;
//             OnPropertyChanged();
//             OnPropertyChanged(nameof(IsNotBusy));
//         }
//     }
//
//     public bool IsNotBusy => !IsBusy;
//     public event PropertyChangedEventHandler PropertyChanged;
//
//     protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//     {
//         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//     }
//
//     protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
//     {
//         if (EqualityComparer<T>.Default.Equals(field, value)) return false;
//         field = value;
//         OnPropertyChanged(propertyName);
//         return true;
//     }
// }
