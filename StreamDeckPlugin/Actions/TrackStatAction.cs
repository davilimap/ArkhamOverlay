﻿using ArkhamOverlay.TcpUtils;
using ArkhamOverlay.TcpUtils.Requests;
using ArkhamOverlay.TcpUtils.Responses;
using Newtonsoft.Json.Linq;
using SharpDeck;
using SharpDeck.Events.Received;
using StreamDeckPlugin.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace StreamDeckPlugin.Actions {
    public abstract class TrackStatAction : StreamDeckAction {
        public static IList<TrackStatAction> ListOf = new List<TrackStatAction>();

        private TrackStatSettings _settings = new TrackStatSettings();

        private Timer _keyPressTimer = new Timer(700);

        private int _value { get; set; }

        public TrackStatAction(StatType statType) {
            StatType = statType;
            ListOf.Add(this);
            _keyPressTimer.Enabled = false;
            _keyPressTimer.Elapsed += KeyHeldDown;
        }

        public Deck Deck {
            get {
                if (_settings == null) {
                    return Deck.Player1;
                }

                return _settings.Deck.AsDeck();
            }
        }

        public StatType StatType { get; }

        protected override Task OnWillAppear(ActionEventArgs<AppearancePayload> args) {
            _settings = args.Payload.GetSettings<TrackStatSettings>();
            return SetTitleAsync(_value.ToString());
        }

        private object _keyUpLock = new object();
        private bool _decreaseSent = false;

        protected override Task OnKeyDown(ActionEventArgs<KeyPayload> args) {
            _settings = args.Payload.GetSettings<TrackStatSettings>();

            _decreaseSent = false;
            _keyPressTimer.Interval = 700;
            _keyPressTimer.Enabled = true;

            return Task.CompletedTask;
        }

        private void KeyHeldDown(object sender, ElapsedEventArgs e) {
            lock (_keyUpLock) {
                _decreaseSent = true;
                SendStatValueRequest(false);
                _keyPressTimer.Interval = 350;  //speed up when you hold it down
            }
        }

        protected override Task OnKeyUp(ActionEventArgs<KeyPayload> args) {
            lock (_keyUpLock) {
                _keyPressTimer.Enabled = false;
                if (_decreaseSent) {
                    return Task.CompletedTask;
                }

                SendStatValueRequest(true);
            }
            return Task.CompletedTask;
        }

        private void SendStatValueRequest(bool increase) {
            var response = StreamDeckSendSocketService.SendRequest<ChangeStatValueResponse>(new ChangeStatValueRequest { Deck = Deck, StatType = StatType, Increase = increase });
            _value = response.Value;
            SetTitleAsync(_value.ToString());
        }


        protected async override Task OnSendToPlugin(ActionEventArgs<JObject> args) {
            _settings.Deck = args.Payload["deck"].Value<string>();

            await SetSettingsAsync(_settings);
        }

        public void UpdateValue(int value) {
            _value = value;
            SetTitleAsync(_value.ToString());
        }
    }
}
