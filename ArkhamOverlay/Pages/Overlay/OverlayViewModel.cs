﻿using ArkhamOverlay.Data;
using PageController;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ArkhamOverlay.Pages.Overlay {
    public class OverlayViewModel : ViewModel {
        public OverlayViewModel() {
            ActAgendaCards = new ObservableCollection<OverlayCardViewModel>();
            HandCards = new ObservableCollection<OverlayCardViewModel>();
            EncounterCards = new ObservableCollection<OverlayCardViewModel>();
            PlayerCards = new ObservableCollection<OverlayCardViewModel>();
        }

        public virtual AppData AppData { get; set; }
        public virtual double DeckListFontSize { get; set; }
        public virtual double DeckListItemWidth { get; set; }
        public virtual double DeckListHeight { get; set; }
        public virtual Thickness DeckListMargin { get; set; }

        public virtual IList<string> DeckList { get; set; }
        public virtual ObservableCollection<OverlayCardViewModel> ActAgendaCards { get; set; }
        public virtual ObservableCollection<OverlayCardViewModel> HandCards { get; set; }
        public virtual ObservableCollection<OverlayCardViewModel> EncounterCards { get; set; }
        public virtual ObservableCollection<OverlayCardViewModel> PlayerCards { get; set; }
        public IList<ObservableCollection<OverlayCardViewModel>> AllOverlayCards {
            get {
                return new List<ObservableCollection<OverlayCardViewModel>> { ActAgendaCards, HandCards, EncounterCards, PlayerCards };
            }
        }

        public CardSet CurrentlyDisplayedHandCardSet { get; set; }
    }
}
