using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace RaceTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    var summary = $"Checkpoint reached:" +
                                   $"\n\t{e.Title}" +
                                   $"\n\t{e.Message}";
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach (var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(summary);
                };
            }
            //AppCenter.Start("ios=2ce226c4-0805-4453-b670-1940f04bd4e7;" +
            //                "android=6da72cba-ff85-4cfa-b9c5-d0b6146ea3c9",
            //    typeof(Analytics), typeof(Crashes), typeof(Push));
            //typeof(Distribute));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
