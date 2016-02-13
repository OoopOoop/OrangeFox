using POF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace POF.ViewModels
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        protected POFContext Repository { get; private set;}

        public ViewModelBase()
        {
            LoadData();
        }

        private async void LoadData()
        {
            Repository = await POFContextFactory.GetPofContextAsync();
            OnDataLoaded();
        }

        protected abstract void OnDataLoaded();
        protected abstract void Save();
    }
}
