using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WordDocMaker
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTkwMDI1QDMxMzcyZTM0MmUzMFE5V2NsaHk2K0w5MXhNQVNMR0EvOWhvNzJ2dklXazAzMkpSYjdWZXVKUXM9");

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
