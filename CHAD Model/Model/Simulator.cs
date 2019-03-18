﻿using System;
using System.Threading;

namespace Model
{
    public class Simulator
    {
        #region Fields

        private readonly ILogger _logger;

        private SimulatorStatus _status;

        #endregion

        #region Constructors

        public Simulator(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Interface

        public AgroHydrology AgroHydrology { get; private set; }

        public event Action StatusChanged;

        public Configuration Configuration { get; private set; }

        public SimulatorStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                StatusChanged?.Invoke();
            }
        }

        public void Start()
        {
            var previousStatus = Status;

            Status = SimulatorStatus.Run;

            if (previousStatus == SimulatorStatus.Run)
                return;

            if (previousStatus == SimulatorStatus.OnPaused)
                return;

            AgroHydrology = new AgroHydrology(_logger, Configuration.Parameters, Configuration.ClimateList,
                Configuration.Fields, Configuration.CropEvapTransList);

            Simulate();

            Stop();
        }

        public void Stop()
        {
            if (Status == SimulatorStatus.Stopped)
                return;

            CurrentSeason = 0;
            CurrentDay = 0;

            Status = SimulatorStatus.Stopped;
        }

        public void Pause()
        {
            if (Status == SimulatorStatus.Stopped || Status == SimulatorStatus.OnPaused)
                return;

            Status = SimulatorStatus.OnPaused;
        }

        public int CurrentSeason { get; private set; }

        public int CurrentDay { get; private set; }

        public void SetConfiguration(Configuration configuration)
        {
            if (Status != SimulatorStatus.Stopped)
                throw new InvalidOperationException("Unable to change configuration while simulator is working");

            Configuration = configuration;
        }

        #endregion

        #region All other members

        private void Simulate2()
        {
            for (var seasonNumber = 1; seasonNumber < int.MaxValue; seasonNumber++)
            {
                CheckStatus();
                CurrentSeason = seasonNumber;

                for (var dayNumber = 1; dayNumber < int.MaxValue; dayNumber++)
                {
                    CheckStatus();
                    CurrentDay = dayNumber;
                }
            }
        }

        private void Simulate()
        {
            for (var seasonNumber = 1; seasonNumber < Configuration.Parameters.NumOfSeasons; seasonNumber++)
            {
                CheckStatus();
                CurrentSeason = seasonNumber;


                for (var dayNumber = 1; dayNumber <= Configuration.DaysCount; dayNumber++)
                {
                    CheckStatus();
                    CurrentDay = dayNumber;


                    AgroHydrology.ProcessDay(dayNumber);
                }
            }
        }

        private void CheckStatus()
        {
            while (Status == SimulatorStatus.OnPaused)
                Thread.Sleep(500);

            if (Status == SimulatorStatus.Stopped)
                Thread.CurrentThread.Abort();
        }

        #endregion
    }
}