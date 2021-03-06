﻿namespace ArkhamOverlay.TcpUtils.Responses {
    public class CardInfoResponse : Response, ICardInfo {
        public CardButtonType CardButtonType { get; set; }
        public string Name { get; set; }
        public bool IsToggled { get; set; }
        public bool ImageAvailable { get; set; }
    }
}
