using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Estudos.NSE.WebApp.MVC.Extensions
{
    public static class ViewDataExtension
    {
        public static void SetReturnUrl(this ViewDataDictionary viewData, string returnUrl)
        {
            viewData["ReturnUrl"] = returnUrl;
        }

        public static string GetReturnUrl(this ViewDataDictionary viewData)
        {
            return viewData["ReturnUrl"] as string;
        }
    }
}