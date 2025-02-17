using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace First_Demo_WebAPI_Client.Utilities
{
    public static class Jeeves
    {
        //For Local API
        public static Uri DBUri = new Uri("https://localhost:7282/");

        //For API on the Internet

        internal static async void ShowMessage(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "Ok",
                DefaultButton = ContentDialogButton.Primary
            };
            _ = await diag.ShowAsync();
        }
        internal static async Task<ContentDialogResult> ConfirmDialog(string strTitle, string Msg)
        {
            ContentDialog diag = new ContentDialog()
            {
                Title = strTitle,
                Content = Msg,
                PrimaryButtonText = "No",
                SecondaryButtonText = "Yes",
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await diag.ShowAsync();
            return result;
        }

    }
}
