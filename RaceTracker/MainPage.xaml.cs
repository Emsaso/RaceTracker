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
        readonly INotificationManager _notificationManager;
        int _notificationNumber;

        public MainPage()
        {
            InitializeComponent();

            _notificationManager = DependencyService.Get<INotificationManager>();
            _notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        private void PushNotificationTest(object sender, EventArgs e)
        {
            var raceTrackerUrl = "https://racetracker.no/";
            Guid newGuid = Guid.NewGuid();

            _notificationNumber++;
            TestLabel.Text = "Push request test succeeded " + _notificationNumber;
            string title = $"Checkpoint #{_notificationNumber}";
            string message = $"{raceTrackerUrl}{newGuid}";
            _notificationManager.ScheduleNotification(title, message);
            Analytics.TrackEvent($"{_notificationManager}, {newGuid}");
        }
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Browser.OpenAsync(message, BrowserLaunchMode.SystemPreferred);
                var ttl = new Label()
                {
                    Text = $"{title}:",
                    TextColor = Color.Black
                };
                var msg = new Label()
                {
                    //Text = $"Link",
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
                UrlOutput.Children.Add(ttl);
                UrlOutput.Children.Add(msg);
            });
        }

        private readonly ICommand _navigationCommand = new Command<string>((url) =>
        {
            Launcher.OpenAsync(new Uri(url));
        });

    }
}
