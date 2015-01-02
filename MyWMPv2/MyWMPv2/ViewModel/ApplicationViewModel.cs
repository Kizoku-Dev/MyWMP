﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using MyWMPv2.Model;
using MyWMPv2.Utilities;

namespace MyWMPv2.ViewModel
{
    class ApplicationViewModel : ObservableObject
    {
        private TemplateEngine _templateEngine;
        private HomeViewModel _homeViewModel;

        public ApplicationViewModel(ListView listMusic, ListView listVideo, ListView listImage)
        {
            _templateEngine = new TemplateEngine();
            _templateEngine.SetNormalTheme();
            _homeViewModel = new HomeViewModel();
            _homeViewModel.PropertyChanged += (sender, arg) => PropertyChangedHandler(arg, listMusic, listVideo, listImage);
            _templateEngine.PropertyChanged += (sender, arg) => PropertyChangedHandler(arg, listMusic, listVideo, listImage);
        }

        private void PropertyChangedHandler(PropertyChangedEventArgs e,
            ListView listMusic, ListView listVideo, ListView listImage)
        {
            switch (e.PropertyName)
            {
                case "ItemsMusic":
                    listMusic.ItemsSource = HomeViewModel.MusicViewModel.Library.Items;
                    break;
                case "ItemsVideo":
                    listVideo.ItemsSource = HomeViewModel.VideoViewModel.Library.Items;
                    break;
                case "ItemsImage":
                    listImage.ItemsSource = HomeViewModel.ImageViewModel.Library.Items;
                    break;
                default:
                    OnPropertyChanged(e.PropertyName);
                    break;
            }
        }

        public TemplateEngine TE
        {
            get { return _templateEngine; }
            set { _templateEngine = value; }
        }

        public HomeViewModel HomeViewModel
        {
            get { return _homeViewModel; }
            set { _homeViewModel = value; }
        }

        public ICommand CloseWindowCommand { get { return new DelegateCommand(CloseWindow); } }

        private void CloseWindow(object sender)
        {
            Environment.Exit(0);
        }
    }
}
