﻿using System;
using System.Collections.ObjectModel;
using DesktopApplication.Services;
using DesktopApplication.Tools;
using Model;

namespace DesktopApplication.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        #region Fields

        private readonly Simulator _simulator;

        private readonly IStorageService _storageService;
        private ConfigurationViewModel _configurationViewModel;

        #endregion

        #region Constructors

        public ApplicationViewModel(IStorageService storageService)
        {
            _storageService = storageService;
            _simulator = new Simulator();
            _simulator.StatusChanged += SimulatorOnStatusChanged;

            ConfigurationsViewModels = new ObservableCollection<ConfigurationViewModel>();
            foreach (var configuration in storageService.GetConfigurations())
                ConfigurationsViewModels.Add(new ConfigurationViewModel(configuration));
        }

        #endregion

        #region Public Interface

        public event Action SimulatorStatusChanged;

        public ConfigurationViewModel MakeConfigurationViewModel()
        {
            return new ConfigurationViewModel(new Configuration());
        }

        public void Start()
        {
            _simulator.Start();
        }

        public void Stop()
        {
            _simulator.Stop();
        }

        public void Pause()
        {
            _simulator.Pause();
        }

        public void Configure()
        {
            ConfigurationViewModel?.Configure();
            RaiseStatusChanged();
        }


        public bool CanStart =>
            (_simulator.Status == SimulatorStatus.Stopped || _simulator.Status == SimulatorStatus.OnPaused)
            && ConfigurationViewModel != null
            && ConfigurationViewModel.IsConfigured;

        public bool CanPause => _simulator.Status == SimulatorStatus.Run;

        public bool CanStop =>
            _simulator.Status == SimulatorStatus.Run || _simulator.Status == SimulatorStatus.OnPaused;

        public ConfigurationViewModel ConfigurationViewModel
        {
            get => _configurationViewModel;
            set
            {
                _simulator.SetConfiguration(_configurationViewModel.Configuration);
                _configurationViewModel = value;

                OnPropertyChanged(nameof(ConfigurationViewModel));
                RaiseStatusChanged();
            }
        }

        public ObservableCollection<ConfigurationViewModel> ConfigurationsViewModels { get; }

        public void AddConfigurationViewModel(ConfigurationViewModel configurationViewModel)
        {
            ConfigurationsViewModels.Add(configurationViewModel);
            ConfigurationViewModel = configurationViewModel;
        }

        #endregion

        #region All other members

        private void SimulatorOnStatusChanged()
        {
            RaiseStatusChanged();
        }

        private void RaiseStatusChanged()
        {
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanPause));
            OnPropertyChanged(nameof(CanStop));
            SimulatorStatusChanged?.Invoke();
        }

        #endregion
    }
}