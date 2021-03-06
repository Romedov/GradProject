﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GradProject.Views
{
    /// <summary>
    /// Логика взаимодействия для SellView.xaml
    /// </summary>
    public partial class SellView : Page
    {
        public SellView()
        {
            InitializeComponent();
            this.DataContext = App.SellVM;
        }
        private void ValidatePTI(object sender, TextCompositionEventArgs e) //только цифры
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
        private void ValidatePKD(object sender, KeyEventArgs e) //только цифры
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
        private void ConcludeKU(object sender, KeyEventArgs e) //обновление итога
        {
            App.SellVM.Conclusion = App.SellVM.ItemsToSell.Conclude();
        }
        private void OnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            App.SellVM.KeyBtn = btn;
        }
    }
}
