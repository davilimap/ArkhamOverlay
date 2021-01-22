﻿using ArkhamOverlay.TcpUtils;
using Newtonsoft.Json.Linq;
using SharpDeck;
using SharpDeck.Events.Received;
using StreamDeckPlugin.Utils;
using System.Threading.Tasks;

namespace StreamDeckPlugin.Actions {
    public abstract class TrackStatAction : StreamDeckAction {
        private TrackStatSettings _settings = new TrackStatSettings();

        protected Deck Deck {
            get {
                if (_settings == null) {
                    return Deck.Player1;
                }

                return _settings.Deck.AsDeck();
            }
        }

        protected override Task OnWillAppear(ActionEventArgs<AppearancePayload> args) {
            _settings = args.Payload.GetSettings<TrackStatSettings>();
            return Task.CompletedTask;
        }

        protected override Task OnKeyUp(ActionEventArgs<KeyPayload> args) {
            //StreamDeckSendSocketService.SendRequest(new ShowDeckListRequest { Deck = Deck });
            return Task.CompletedTask;
        }

        protected async override Task OnSendToPlugin(ActionEventArgs<JObject> args) {
            _settings.Deck = args.Payload["deck"].Value<string>();

            await SetSettingsAsync(_settings);
        }
    }
}
