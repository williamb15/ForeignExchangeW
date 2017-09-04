

namespace ForeignExchangeW.Services
{
    using ForeignExchangeW.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class DialogService
    {
        public async Task ShowMessage (string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                Lenguages.Accept);
        }
    }
}
