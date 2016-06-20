using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Games
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var navigationService = this.createNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            SimpleIoc.Default.Register<NumbersGameViewModel>();

        }

        public NumbersGameViewModel NumbersGameViewModel => SimpleIoc.Default.GetInstance<NumbersGameViewModel>(Guid.NewGuid().ToString());

        private INavigationService createNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("NumbersGamePage", typeof(NumbersGame));
            return navigationService;
        }
    }
}
