using GradProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GradProject
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ShiftViewModel ShiftVM { get; private set; } = new ShiftViewModel();
        public static SellViewModel SellVM { get; private set; } = new SellViewModel();
    }
}
