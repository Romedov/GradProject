using System;
using System.Windows;
using GradProject.ViewModels;

namespace GradProject
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ShiftViewModel ShiftVM { get; private set; } = new ShiftViewModel();
        public static SellViewModel SellVM { get; private set; } = new SellViewModel();
        public static ReturnViewModel ReturnVM { get; private set; } = new ReturnViewModel();
    }
}
