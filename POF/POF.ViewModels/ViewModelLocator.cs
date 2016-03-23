using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.ViewModels
{
   public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<SoundSelectViewModel>();
            SimpleIoc.Default.Register<AlarmPageViewModel>();
            SimpleIoc.Default.Register<DaySelectViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
        }

        public SoundSelectViewModel SoundSelectViewModel => SimpleIoc.Default.GetInstance<SoundSelectViewModel>();
        public AlarmPageViewModel AlarmPageViewModel => SimpleIoc.Default.GetInstance<AlarmPageViewModel>();
        public DaySelectViewModel DaySelectViewModel => SimpleIoc.Default.GetInstance<DaySelectViewModel>();
        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();
    }
}
