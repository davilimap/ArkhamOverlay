﻿using ArkhamOverlay.Data;
using PageController;
using System.Collections.Generic;

namespace ArkhamOverlay.Pages.ChooseEncounters {
    public class ChooseEncountersViewModel : ViewModel {
        public ChooseEncountersViewModel() {
            Cycles = new List<SelectableEncounterCycle>();
        }

        public virtual IList<SelectableEncounterCycle> Cycles { get; private set; }
    }

    public class SelectableEncounterCycle {
        public SelectableEncounterCycle() {
            EncounterSets = new List<ISelectableEncounterSet>();
        }

        public IList<ISelectableEncounterSet> EncounterSets { get; private set; }
    }

    public interface ISelectableEncounterSet {
        bool IsSelected { get; set; }
        string Name { get; }
    }

    public class SelectableEncounterSet : ISelectableEncounterSet {
        public SelectableEncounterSet(EncounterSet encounterSet) {
            EncounterSet = encounterSet;
        }

        public EncounterSet EncounterSet { get; private set; }
        public bool IsSelected { get; set; }
        public string Name { get => EncounterSet.Name; }
    }

    public class SelectableLocalPackManifest : ISelectableEncounterSet {
        public SelectableLocalPackManifest(LocalPackManifest localPackManifest) {
            Manifest = localPackManifest;
        }

        public LocalPackManifest Manifest { get; private set; }
        public bool IsSelected { get; set; }
        public string Name { get => Manifest.Name; }
    }

}
