using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using POF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

           // ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = this.createNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

        //    SimpleIoc.Default.Register<IDialogService, DialogService>();

            
            SimpleIoc.Default.Register<SoundSelectViewModel>();
            SimpleIoc.Default.Register<AlarmPageViewModel>();
            SimpleIoc.Default.Register<DaySelectViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
        }




        public SoundSelectViewModel SoundSelectViewModel => SimpleIoc.Default.GetInstance<SoundSelectViewModel>(Guid.NewGuid().ToString());
        // public SoundSelectViewModel SoundSelectViewModel => SimpleIoc.Default.GetInstance<SoundSelectViewModel>();
        public AlarmPageViewModel AlarmPageViewModel => SimpleIoc.Default.GetInstance<AlarmPageViewModel>();
        public DaySelectViewModel DaySelectViewModel => SimpleIoc.Default.GetInstance<DaySelectViewModel>(Guid.NewGuid().ToString());
        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();




        private INavigationService createNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("MainPage", typeof(MainPage));
            navigationService.Configure("AddPage", typeof(AlarmPage));
            return navigationService;
        }
    }
}
