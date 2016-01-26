using POF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.ViewModels
{
  public static class POFContextFactory
    {
        private static POFContext _pofContext;

        public static async Task<POFContext> GetPofContextAsync()
        {
            if(_pofContext==null)
            {
                _pofContext = new POFContext();
                await _pofContext.InitializeContextAsync();
            }
            return _pofContext;
        }
    }
}
