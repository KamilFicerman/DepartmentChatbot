using DepartmentChatbot.Models;

namespace DepartmentChatbot
{
    public partial class App : Application
    {
        //uri for http connections
        public const string uri = "...";
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

    }
}
