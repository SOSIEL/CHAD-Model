﻿using Newtonsoft.Json;

namespace CHADSOSIEL.Configuration
{
    /// <summary>
    /// Initial state configuration model. Used to parse section "InitialState".
    /// </summary>
    public class InitialStateConfiguration
    {
        [JsonRequired]
        public AgentStateConfiguration[] AgentsState { get; set; }
    }
}
