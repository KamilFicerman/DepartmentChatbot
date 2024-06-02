using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DepartmentChatbot.Models;
using DepartmentChatbot.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentChatbot.ViewModels
{
    public partial class EmployeesViewModel : ObservableObject
    {
        private CancellationTokenSource _cancellationTokenSource;
        [ObservableProperty]
        ObservableCollection<Employee> employees;
        [ObservableProperty]
        ObservableCollection<Employee> employeesVisible;
        bool isBusy = false;
        bool employeesVisibleInitialized = false;
        EmployeesService employeesService;

        public EmployeesViewModel()
        {
            Employees = [];
            EmployeesVisible = [];
            _cancellationTokenSource = new CancellationTokenSource();
            employeesService = new EmployeesService();


            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        Employees = await employeesService.GetEmployees();
                        if (!employeesVisibleInitialized)
                        {
                            EmployeesVisible = Employees;
                            employeesVisibleInitialized = true;
                        }
                        if (!isBusy)
                        {
                            EmployeesVisible = Employees;
                        }
                    }
                    catch { }
                    await Task.Delay(60000);
                }
            });
        }

        [RelayCommand]
        void OnTextUpdated(string text)
        {
            isBusy = true;
            if (!string.IsNullOrEmpty(text))
            {
                string filterText = text.ToLower();
                EmployeesVisible = Employees.Where(emp => emp.Name.ToLower().Contains(filterText)).ToObservableCollection();
            }
            else
            {
                EmployeesVisible = Employees;
                isBusy = false;
            }
        }

        ~EmployeesViewModel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
