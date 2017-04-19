using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VrManager.Pages;

namespace VrManager.Helpers
{
    public class PermissionsHelper
    {
        public static void PermissionsAdmin(params FrameworkElement[] elements)
        {
            if (true)
            {
                foreach (FrameworkElement element in elements)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (FrameworkElement element in elements)
                {
                    element.Visibility = Visibility.Collapsed;
                }
            }
        }

        public static void PermissionsLicense(params FrameworkElement[] elements)
        {
            if (true)
            {
                foreach (FrameworkElement element in elements)
                {
                    element.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (FrameworkElement element in elements)
                {
                    element.Visibility = Visibility.Collapsed;
                }
            }
        }

        public static void NotLicensedPage()
        {
            if (!App.ІsLicensed)
            {
                App.Frame.Navigate(new StartUpPage());
            }
        }

        public static void NotAuthprizePage()
        {
            if (!App.ІsLicensed)
            {
                App.Frame.Navigate(new StartUpPage());
            }
        }
    }
}
