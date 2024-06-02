using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using DepartmentChatbot.ViewModels;
using System.Globalization;

namespace DepartmentChatbot.Services
{
    public static class SpeechToTextService
    {
        public static async Task StartListening(CancellationToken cancellationToken, MainViewModel mvm)
        {
            var isGranted = await SpeechToText.RequestPermissions(cancellationToken);
            if (!isGranted)
            {
                await Toast.Make("Brak uprawnień!").Show(CancellationToken.None);
                return;
            }

            SpeechToText.Default.RecognitionResultUpdated += (sender, args) => OnRecognitionTextUpdated(sender, args, mvm);
            SpeechToText.Default.RecognitionResultCompleted += (sender, args) => OnRecognitionTextCompleted(sender, args, mvm);
            await SpeechToText.StartListenAsync(CultureInfo.GetCultureInfo("pl"), CancellationToken.None);
        }

        public static async Task StopListening(CancellationToken cancellationToken, MainViewModel mvm)
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= (sender, args) => OnRecognitionTextUpdated(sender, args, mvm);
            SpeechToText.Default.RecognitionResultCompleted -= (sender, args) => OnRecognitionTextCompleted(sender, args, mvm);
        }

        static void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args, MainViewModel mvm)
        {
            mvm.Text = args.RecognitionResult;
        }

        static void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args, MainViewModel mvm)
        {
            mvm.Text = args.RecognitionResult;
        }
    }
}
