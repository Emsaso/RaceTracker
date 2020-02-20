using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;


namespace RaceTracker
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private int _amOfPushSent;
        readonly INotificationManager _notificationManager;
        int _notificationNumber = 0;

        public MainPage()
        {
            InitializeComponent();
            
            _notificationManager = DependencyService.Get<INotificationManager>();
            _notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                //ShowNotification(evtData.Title, evtData.Message);
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        private void PushNotificationTest(object sender, EventArgs e)
        {
            var raceTrackerUrl = "https://racetracker.no/";
            _amOfPushSent++;
            TestLabel.Text = "Push request test succeeded " + _amOfPushSent;

            //Uri uri = new Uri(raceTrackerUrl);
            //await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            //Analytics.TrackEvent("My custom event");
            //Analytics.TrackEvent("ios=2ce226c4-0805-4453-b670-1940f04bd4e7;android=android=6da72cba-ff85-4cfa-b9c5-d0b6146ea3c9", new Dictionary<string, string>(_amOfPushSent));
            //Analytics.TrackEvent($"Push Event Test {_amOfPushSent}, {raceTrackerUrl}", new Dictionary<string, string>());
            //Analytics.TrackEvent($"Push Event Test {_amOfPushSent}");

            Guid newGuid = Guid.NewGuid();
            Analytics.TrackEvent($"{newGuid}");
            
            _notificationNumber++;
            string title = $"Checkpoint #{_notificationNumber}";
            string message = $"{raceTrackerUrl}{newGuid}";
            _notificationManager.ScheduleNotification(title, message);
            //bool isEnabled = await Push.IsEnabledAsync();

            //Crashes.GenerateTestCrash();
        }
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    //Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                    Text = $"{message}",
                    TextColor = Color.Blue,
                    TextDecorations = TextDecorations.Underline,
                    GestureRecognizers = { new TapGestureRecognizer
                    {
                        Command = _navigationCommand,
                        CommandParameter = message
                    }}
                };
                UrlOutput.Children.Clear();
                UrlOutput.Children.Add(msg);
            });
        }

        private readonly ICommand _navigationCommand = new Command<string>((url) =>
        {
            Launcher.OpenAsync(new Uri(url));
        });

    }
}
