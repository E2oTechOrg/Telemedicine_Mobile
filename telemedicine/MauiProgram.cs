using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace telemedicine
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Cambria-Font-For Android.ttf", "Cambria");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            // 🔥 Remove Android underline for Entry
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = null;
            });

            // 🔥 Remove Android underline for Picker
            PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = null;
            });

            // 🎥 Grant camera/mic permission requests inside WebView + hide wake-lock toast
            WebViewHandler.Mapper.AppendToMapping("CustomWebChrome", (handler, view) =>
            {
                handler.PlatformView.SetWebChromeClient(new telemedicine.Platforms.Android.CustomWebChromeClient());
                handler.PlatformView.SetWebViewClient(new telemedicine.Platforms.Android.CustomWebViewClient());
                handler.PlatformView.Settings.JavaScriptEnabled = true;
                handler.PlatformView.Settings.MediaPlaybackRequiresUserGesture = false;
                handler.PlatformView.Settings.DomStorageEnabled = true;
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}