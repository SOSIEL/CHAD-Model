﻿using Newtonsoft.Json;

namespace CHADSOSIEL.Configuration
{
    /// <summary>
    /// Probabilities configuration model. Used to parse section "ProbabilitiesConfiguration".
    /// </summary>
    public class ProbabilitiesConfiguration
    {
        [JsonRequired]
        public string Variable { get; set; }

        [JsonRequired]
        public string FilePath { get; set; }

        [JsonRequired]
        public string VariableType { get; set; }

        public bool WithHeader { get; set; }
    }
}