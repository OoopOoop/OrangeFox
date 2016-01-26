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
        }

        public SoundSelectViewModel SoundSelectViewModel => SimpleIoc.Default.GetInstance<SoundSelectViewModel>();
    }
}
