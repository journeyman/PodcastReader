﻿using PodcastReader.Phone8.ViewModels;
using ReactiveUI;
using Magellan.WP.Controls;

namespace PodcastReader.Phone8.Views
{
    public partial class PodcastView : Layout, IViewFor<PodcastItemViewModel>
    {
        private PodcastItemViewModel _viewModel;

        public PodcastView()
        {
            InitializeComponent();
        }

        public PodcastItemViewModel ViewModel 
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (PodcastItemViewModel) value; } }
    }
}