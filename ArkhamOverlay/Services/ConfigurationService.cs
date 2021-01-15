﻿using ArkhamOverlay.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ArkhamOverlay.Services {
    public interface IConfiguration {
        int OverlayHeight { get; set; }
        int OverlayWidth { get; set; }
        int CardHeight { get; set; }
        int ActAgendaCardHeight { get; set; }
        int HandCardHeight { get; set; }
        Point ScenarioCardsPosition { get; set; }
        Point LocationsPosition { get; set; }
        Point EncounterCardsPosition { get; set; }
        Point Player1Position { get; set; }
        Point Player2Position { get; set; }
        Point Player3Position { get; set; }
        Point Player4Position { get; set; }
        Point OverlayPosition { get; set; }
        IList<Pack> Packs { get; }
    }

    public class ConfigurationFile : IConfiguration {
        public ConfigurationFile() {
            Packs = new List<Pack>();
        }
                
        public int OverlayHeight { get; set; }
        public int OverlayWidth { get; set; }
        public int CardHeight { get; set; }
        public int ActAgendaCardHeight { get; set; }
        public int HandCardHeight { get; set; }
        public bool UseActAgendaBar { get; set; }
        public IList<Pack> Packs { get; set; }
        public Point ScenarioCardsPosition { get; set; }
        public Point LocationsPosition { get; set; }
        public Point EncounterCardsPosition { get; set; }
        public Point Player1Position { get; set; }
        public Point Player2Position { get; set; }
        public Point Player3Position { get; set; }
        public Point Player4Position { get; set; }
        public Point OverlayPosition { get; set; }

    }

    public class ConfigurationService {
        private readonly Configuration _configuration;
        public ConfigurationService(AppData appData) {
            _configuration = appData.Configuration;
        }

        public void Load() {
            var configuration = new ConfigurationFile {
                OverlayWidth = 1228,
                OverlayHeight = 720,
                CardHeight = 300,
                ActAgendaCardHeight = 200,
                HandCardHeight = 200
            };

            if (File.Exists("Config.json")) {
                try {
                    configuration = JsonConvert.DeserializeObject<ConfigurationFile>(File.ReadAllText("Config.json"));
                } catch {
                    // if there's an error, we don't care- just use the default configuration
                }
            }
            configuration.CopyTo(_configuration);

            _configuration.ConfigurationChanged += Save;
        }

        private void Save() {
            var configurationFile = new ConfigurationFile();
            _configuration.CopyTo(configurationFile);

            File.WriteAllText("Config.json", JsonConvert.SerializeObject(configurationFile));
        }
    }

    public static class ConfigurationExtensions {
        public static void CopyTo(this IConfiguration fromConfiguration, IConfiguration toConfiguration) {
            toConfiguration.OverlayHeight = fromConfiguration.OverlayHeight;
            toConfiguration.OverlayWidth = fromConfiguration.OverlayWidth;
            toConfiguration.CardHeight = fromConfiguration.CardHeight;
            toConfiguration.ActAgendaCardHeight = fromConfiguration.ActAgendaCardHeight;
            toConfiguration.HandCardHeight = fromConfiguration.HandCardHeight;
            toConfiguration.ScenarioCardsPosition = fromConfiguration.ScenarioCardsPosition;
            toConfiguration.LocationsPosition = fromConfiguration.LocationsPosition;
            toConfiguration.EncounterCardsPosition = fromConfiguration.EncounterCardsPosition;
            toConfiguration.Player1Position = fromConfiguration.Player1Position;
            toConfiguration.Player2Position = fromConfiguration.Player2Position;
            toConfiguration.Player3Position = fromConfiguration.Player3Position;
            toConfiguration.Player4Position = fromConfiguration.Player4Position;
            toConfiguration.OverlayPosition = fromConfiguration.OverlayPosition;
            toConfiguration.Packs.Clear();
            foreach (var pack in fromConfiguration.Packs) {
                toConfiguration.Packs.Add(new Pack(pack));
            }
        }
    }
}
