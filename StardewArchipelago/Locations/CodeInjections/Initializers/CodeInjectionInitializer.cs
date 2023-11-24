﻿using StardewArchipelago.Archipelago;
using StardewArchipelago.Locations.CodeInjections.Vanilla.MonsterSlayer;
using StardewArchipelago.Serialization;
using StardewModdingAPI;
using StardewArchipelago.Stardew;

namespace StardewArchipelago.Locations.CodeInjections.Initializers
{
    public static class CodeInjectionInitializer
    {
        public static void Initialize(IMonitor monitor, IModHelper modHelper, ArchipelagoClient archipelago, ArchipelagoStateDto state, LocationChecker locationChecker, StardewItemManager itemManager, WeaponsManager weaponsManager)
        {
            VanillaCodeInjectionInitializer.Initialize(monitor, modHelper, archipelago, state, locationChecker, itemManager, weaponsManager);
            if (archipelago.SlotData.Mods.IsModded)
            {
                ModCodeInjectionInitializer.Initialize(monitor, modHelper, archipelago, locationChecker);
            }
        }
    }
}
