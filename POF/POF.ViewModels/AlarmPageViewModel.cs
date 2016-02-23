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

       private SoundSelectViewModel soundPopUp;

        protected override void OnDataLoaded()
        {
            //new SoundSelectViewModel();
        }


        public AlarmPageViewModel()
        {
            soundPopUp = new SoundSelectViewModel();
            OpenSoundPopUp = new RelayCommand(openSoundPopUp);
        }

        private void openSoundPopUp()
        {
            soundPopUp.IsPopUpOpen = true;
           
        }
    }
}
