using DepartmentChatbot.ViewModels;
using System.Diagnostics;

namespace DepartmentChatbot
{
    public partial class EmployeesPage : ContentPage
    {
        public EmployeesPage(EmployeesViewModel vm)
        {

            InitializeComponent();
            BindingContext = vm;
        }
    }
}