using System;
using HarmonyLib;
using KaitoKid.ArchipelagoUtilities.Net.Interfaces;
using StardewArchipelago.Archipelago;
using StardewModdingAPI;

namespace StardewArchipelago.Locations.CodeInjections.Modded
{
    public static class GreenhouseSprinklerInjections
    {
        private static ILogger _logger;
        private static IModHelper _helper;
        private static StardewArchipelagoClient _archipelago;

        public static void Initialize(ILogger logger, IModHelper modHelper, StardewArchipelagoClient archipelago)
        {
            _logger = logger;
            _helper = modHelper;
            _archipelago = archipelago;
        }

        // namespace Bpendragon.GreenhouseSprinklers
        // partial class ModEntry
        // private void AddLetterIfNeeded(int curLevel)
        public static void AddLetterIfNeeded_CheckPreviousLevels_Postfix(object __instance, int curLevel)
        {
            try
            {
                var modType = AccessTools.TypeByName("Bpendragon.GreenhouseSprinklers.ModEntry");
                Type[] argTypes = {typeof(int)};
                var addLetterMethod = AccessTools.Method(modType, "AddLetterIfNeeded", argTypes);
                if (curLevel > 0)
                {
                    // Recursively invoke to check previous levels too
                    var level = curLevel-1;
                    object[] args = {level};
                    addLetterMethod.Invoke(__instance, args);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed in {nameof(AddLetterIfNeeded_CheckPreviousLevels_Postfix)}:\n{ex}");
            }
        }
    }
}
