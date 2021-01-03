﻿using ArkhamOverlay.Services;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArkhamOverlay.Data {
    public delegate void CardToggledEvent(ICardButton card);

    public class Card : ICardButton, INotifyPropertyChanged {
        public Card() {
        }

        public Card(ArkhamDbCard arkhamDbCard, bool isPlayerCard, bool cardBack = false) {
            Code = arkhamDbCard.Code;
            Name = arkhamDbCard.Xp == "0" || string.IsNullOrEmpty(arkhamDbCard.Xp) ? arkhamDbCard.Name : arkhamDbCard.Name + " (" + arkhamDbCard.Xp + ")";
            Faction = GetFaction(arkhamDbCard.Faction_Name);
            Type = GetCardType(arkhamDbCard.Type_Code);
            ImageSource = cardBack ? arkhamDbCard.BackImageSrc : arkhamDbCard.ImageSrc;
            IsPlayerCard = isPlayerCard;

            if (cardBack) {
                Name += " (Back)";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            if (handler == null) {
                return;
            }
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }

        public string ImageSource { get; set; }

        public CardType Type { get; set; }

        public bool IsVisible { get; private set;}
        public Brush BorderBrush { get {return IsVisible ? new SolidColorBrush(Colors.Black) : Background; } }

        public Brush Background {
            get {
                switch (Type) {
                    case CardType.Agenda:
                        return new SolidColorBrush(Colors.Chocolate);
                    case CardType.Act:
                        return new SolidColorBrush(Colors.BurlyWood);
                    case CardType.Enemy:
                        return new SolidColorBrush(Colors.SlateBlue);
                    case CardType.Treachery:
                        return new SolidColorBrush(Colors.SlateGray);
                    case CardType.Asset:
                    case CardType.Event:
                    case CardType.Skill:
                        switch (Faction) {
                            case Faction.Guardian:
                                return new SolidColorBrush(Colors.DarkBlue);
                            case Faction.Seeker:
                                return new SolidColorBrush(Colors.DarkGoldenrod);
                            case Faction.Rogue:
                                return new SolidColorBrush(Colors.DarkGreen);
                            case Faction.Survivor:
                                return new SolidColorBrush(Colors.DarkRed);
                            case Faction.Mystic:
                                return new SolidColorBrush(Colors.Indigo);
                            default:
                                return new SolidColorBrush(Colors.DarkGray);
                        }
                    default:
                        return new SolidColorBrush(Colors.DarkGray);
                }
            }
        }

        public bool IsHorizontal { get { return Type == CardType.Act || Type == CardType.Agenda; } }

        public bool IsPlayerCard { get; private set; }

        public SelectableCards SelectableCards { get; set; }
        public Card FlipSideCard { get; set; }

        public void Click() {
            if (SelectableCards == null) {
                return;
            }

            IsVisible = !IsVisible;
            SelectableCards.ToggleCard(this);
            OnPropertyChanged(nameof(BorderBrush));
        }

        public void Hide() {
            IsVisible = false;
            OnPropertyChanged(nameof(BorderBrush));
        }

        private CardType GetCardType(string typeCode) {
            if(Enum.TryParse(typeCode, ignoreCase: true, out CardType type)) {
                return type;
            } else {
                return CardType.Other;
            }
        }

        private Faction GetFaction(string factionCode) {
            if (Enum.TryParse(factionCode, ignoreCase: true, out Faction type)) {
                return type;
            }
            else {
                return Faction.Other;
            }
        }
    }
}
