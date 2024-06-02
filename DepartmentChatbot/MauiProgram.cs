using banditoth.MAUI.DeviceId;
using CommunityToolkit.Maui;
using DepartmentChatbot.Services;
using DepartmentChatbot.ViewModels;
using Microsoft.Extensions.Logging;
using DepartmentChatbot.Handlers;

namespace DepartmentChatbot
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureDeviceIdProvider()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                 {
#if ANDROID || IOS || WINDOWS
                     handlers.AddHandler<SearchBar, SearchBarExHandler>();
#endif
                 });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MessageService>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<EmployeesPage>();
            builder.Services.AddSingleton<EmployeesViewModel>();
            return builder.Build();
        }
    }
}
