using System;
using System.Collections.Generic;
using KaitoKid.ArchipelagoUtilities.Net.Client;
using StardewArchipelago.Constants;
using StardewValley;

namespace StardewArchipelago.GameModifications.EntranceRandomizer
{
    public class EquivalentWarps
    {
        private const string jojaMart = "JojaMart";
        private const string abandonedJojaMart = "AbandonedJojaMart";
        private const string movieTheater = "MovieTheater";
        private const string trailer = "Trailer";
        private const string trailerBig = "Trailer_Big";
        private const string beach = "Beach";
        private const string beachNightMarket = "BeachNightMarket";
        private const string grandpaShedRuins = "Custom_GrandpasShedRuins";
        private const string grandpaShedFinish = "Custom_GrandpasShed";
        private const string auroraVineyard = "Custom_AuroraVineyard";
        private const string auroraVineyardCellar = "Custom_ApplesRoom";
        private const string auroraVineyardRefurbished = "Custom_AuroraVineyardRefurbished";
        private const string auroraVineyardCellarRefurbished = "Custom_AuroraVineyardCellarRefurbished";
        private static readonly string[] _jojaMartLocations = { jojaMart, abandonedJojaMart, movieTheater };
        private static readonly string[] _trailerLocations = { trailer, trailerBig };
        private static readonly string[] _beachLocations = { beach, beachNightMarket };
        private static readonly string[] _grandpaShedLocations = { grandpaShedRuins, grandpaShedFinish };
        private static readonly string[] _auroraVineyardLocations = { auroraVineyard, auroraVineyardRefurbished };
        private static readonly string[] _auroraVineyardCellarLocations = { auroraVineyardCellar, auroraVineyardCellarRefurbished };

        public List<string[]> EquivalentAreas = new()
        {
            _jojaMartLocations,
            _trailerLocations,
            _beachLocations,
            _grandpaShedLocations,
            _auroraVineyardLocations,
            _auroraVineyardCellarLocations,
        };

        private readonly ArchipelagoClient _archipelago;

        public EquivalentWarps(ArchipelagoClient archipelago)
        {
            _archipelago = archipelago;
        }

        public string GetDefaultEquivalentEntrance(string entrance)
        {
            if (entrance.Contains(EntranceManager.TRANSITIONAL_STRING))
            {
                var parts = entrance.Split(EntranceManager.TRANSITIONAL_STRING);
                var area1 = parts[0];
                var area2 = parts[1];
                var defaultArea1 = GetDefaultEquivalentEntrance(area1);
                var defaultArea2 = GetDefaultEquivalentEntrance(area2);
                return $"{defaultArea1}{EntranceManager.TRANSITIONAL_STRING}{defaultArea2}";
            }

            if (IsJojaMart(entrance, out _))
            {
                return jojaMart;
            }

            if (IsTrailer(entrance, out _))
            {
                return trailer;
            }

            if (IsBeach(entrance, out _))
            {
                return beach;
            }

            if (IsGrandpaShed(entrance, out _))
            {
                return grandpaShedRuins;
            }

            if (IsAuroraVineyard(entrance, out _))
            {
                return auroraVineyard;
            }

            if (IsAuroraVineyardCellar(entrance, out _))
            {
                return auroraVineyardCellar;
            }

            return entrance;
        }

        public string GetCorrectEquivalentEntrance(string entrance)
        {
            if (entrance.Contains(EntranceManager.TRANSITIONAL_STRING))
            {
                var parts = entrance.Split(EntranceManager.TRANSITIONAL_STRING);
                var area1 = parts[0];
                var area2 = parts[1];
                var correctArea1 = GetCorrectEquivalentEntrance(area1);
                var correctArea2 = GetCorrectEquivalentEntrance(area2);
                return $"{correctArea1}{EntranceManager.TRANSITIONAL_STRING}{correctArea2}";
            }

            if (IsJojaMart(entrance, out var jojaMartCorrectEntrance))
            {
                return jojaMartCorrectEntrance;
            }

            if (IsTrailer(entrance, out var trailerCorrectEntrance))
            {
                return trailerCorrectEntrance;
            }

            if (IsBeach(entrance, out var beachCorrectEntrance))
            {
                return beachCorrectEntrance;
            }

            if (IsGrandpaShed(entrance, out var shedCorrectEntrance))
            {
                return shedCorrectEntrance;
            }

            if (IsAuroraVineyard(entrance, out var auroraVineyardCorrectEntrance))
            {
                return auroraVineyardCorrectEntrance;
            }

            if (IsAuroraVineyardCellar(entrance, out var auroraVineyardCellarCorrectEntrance))
            {
                return auroraVineyardCellarCorrectEntrance;
            }

            return entrance;
        }

        private bool IsJojaMart(string area, out string correctArea)
        {
            foreach (var jojaMartLocation in _jojaMartLocations)
            {
                if (!area.Equals(jojaMartLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var numberOfTheaters = _archipelago.GetReceivedItemCount(APItem.MOVIE_THEATER);

                if (numberOfTheaters >= 2)
                {
                    correctArea = area.Replace(jojaMartLocation, movieTheater);
                    return true;
                }

                if (numberOfTheaters >= 1)
                {
                    correctArea = area.Replace(jojaMartLocation, abandonedJojaMart);
                    return true;
                }

                correctArea = area.Replace(jojaMartLocation, jojaMart);
                return true;
            }

            correctArea = area;
            return false;
        }

        private bool IsTrailer(string area, out string correctArea)
        {
            foreach (var trailerLocation in _trailerLocations)
            {
                if (!area.Equals(trailerLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Game1.MasterPlayer.mailReceived.Contains("pamHouseUpgrade"))
                {
                    correctArea = area.Replace(trailerLocation, trailerBig);
                    return true;
                }

                correctArea = area.Replace(trailerLocation, trailer);
                return true;
            }

            correctArea = area;
            return false;
        }

        private bool IsBeach(string area, out string correctArea)
        {
            foreach (var beachLocation in _beachLocations)
            {
                if (!area.Equals(beachLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Game1.dayOfMonth >= 15 && Game1.dayOfMonth <= 17 && Game1.currentSeason.Equals("winter", StringComparison.OrdinalIgnoreCase))
                {
                    correctArea = area.Replace(beachLocation, beachNightMarket);
                    return true;
                }

                correctArea = area.Replace(beachLocation, beach);
                return true;
            }

            correctArea = area;
            return false;
        }

        private bool IsGrandpaShed(string area, out string correctArea)
        {
            foreach (var shedLocation in _grandpaShedLocations)
            {
                if (!area.Equals(shedLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Game1.MasterPlayer.mailReceived.Contains("ShedRepaired"))
                {
                    correctArea = area.Replace(shedLocation, grandpaShedFinish);
                    return true;
                }

                correctArea = area.Replace(shedLocation, grandpaShedRuins);
                return true;
            }

            correctArea = area;
            return false;
        }

        private bool IsAuroraVineyard(string area, out string correctArea)
        {
            foreach (var auroraVineyardLocation in _auroraVineyardLocations)
            {
                if (!area.Equals(auroraVineyardLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Game1.MasterPlayer.mailReceived.Contains("PlayerWantsAuroraVineyard"))
                {
                    correctArea = area.Replace(auroraVineyardLocation, auroraVineyardRefurbished);
                    return true;
                }

                correctArea = area.Replace(auroraVineyardLocation, auroraVineyard);
                return true;
            }

            correctArea = area;
            return false;
        }

        private bool IsAuroraVineyardCellar(string area, out string correctArea)
        {
            foreach (var auroraVineyardCellarLocation in _auroraVineyardCellarLocations)
            {
                if (!area.Equals(auroraVineyardCellarLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Game1.MasterPlayer.mailReceived.Contains("PlayerWantsAuroraVineyard"))
                {
                    correctArea = area.Replace(auroraVineyardCellarLocation, auroraVineyardCellarRefurbished);
                    return true;
                }

                correctArea = area.Replace(auroraVineyardCellarLocation, auroraVineyardCellar);
                return true;
            }

            correctArea = area;
            return false;
        }
    }
}
