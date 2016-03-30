using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POF.ViewModels
{
    public class AlarmPageViewModel : ViewModelBase
    {
        public ICommand OpenSoundPopUp { get; set; }

        private bool _isSoundPoUpOpen;

        public bool IsSoundPopUpOpen
        {
            get { return _isSoundPoUpOpen; }
            set { _isSoundPoUpOpen = value; OnPropertyChanged(); }
        }


        protected override void OnDataLoaded()
        {
           
        }


        public AlarmPageViewModel()
        {
            OpenSoundPopUp = new RelayCommand(openSoundPopUp);

           // TestCommand = new RelayCommand(testMethod);
        }


        private  void openSoundPopUp()
        {
           IsSoundPopUpOpen = true;

        }



        //public ICommand TestCommand { get; set; }

        //private void testMethod()
        //{
        //    SoundSelectViewModel test = new SoundSelectViewModel();
        
        //}

    }
}
