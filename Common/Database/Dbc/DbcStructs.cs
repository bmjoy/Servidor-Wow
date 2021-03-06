﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Globals;

namespace Common.Database.Dbc
{

    #region AreaTable.dbc
    public class AreaTableReader : DbcReader<AreaTable>
    {
        public AreaTable GetArea(int id)
        {
            return RecordDataIndexed.Values.ToArray().FirstOrDefault(a => a.AreaId == id);
        }
    }

    public class AreaTable : DbcRecordBase
    {
        public int AreaId;
        public int AreaMapId;
        public int AreaZone;
        public int AreaExploreFlag;
        public int AreaZoneType;
        public int AreaLevel;
        public string AreaName;

        public override int Read()
        {
            AreaId = GetInt32(0);
            AreaMapId = GetInt32(1); // May be needed in the future
            AreaZone = GetInt32(2);
            AreaExploreFlag = GetInt32(3);
            AreaZoneType = GetInt32(4);
            AreaLevel = GetInt32(10);
            AreaName = GetString(11);

            if (AreaLevel > 255)
                AreaLevel = 255;

            if (AreaLevel < 0)
                AreaLevel = 0;

            return AreaId;
        }
    }
    #endregion

    #region Faction.dbc

    public class FactionReader : DbcReader<Faction>
    {
        public Faction GetFaction(int id)
        {
            return RecordDataIndexed.Values.ToArray().FirstOrDefault(a => a.FactionFlag == id);
        }

        public List<string> GenerateFactions(Races race)
        {
            List<string> listReturn = new List<string>();

            for (int i = 0; i < 63; i++)
            {
                var faction = GetFaction(i);
                if (faction != null)
                {
                    for (int flags = 0; flags < 4; flags++)
                    {
                        if (HaveFlag(faction.Flags[flags], race - 1))
                            listReturn.Add($"{faction.FactionId}, {faction.ReputationFlags[flags]}, {faction.ReputationStats[flags]}");
                    }
                }
                else
                    listReturn.Add("0, 0, 0");
            }

            return listReturn;
        }

        private static bool HaveFlag(int value, Races race)
        {
            value = value >> (int)race;
            value = value % 2;

            if (value == 1) return true;

            return false;
        }
    }

    public class Faction : DbcRecordBase
    {
        public int FactionId;
        public int FactionFlag;
        public int[] Flags = new int[4];
        public int[] ReputationStats = new int[4];
        public int[] ReputationFlags = new int[4];
        public string FactionName;

        public override int Read()
        {
            FactionId = GetInt32(0);
            FactionFlag = GetInt32(1);

            Flags[0] = GetInt32(2);
            Flags[1] = GetInt32(3);
            Flags[2] = GetInt32(4);
            Flags[3] = GetInt32(5);

            ReputationStats[0] = GetInt32(10);
            ReputationStats[1] = GetInt32(11);
            ReputationStats[2] = GetInt32(12);
            ReputationStats[3] = GetInt32(13);

            ReputationFlags[0] = GetInt32(14);
            ReputationFlags[1] = GetInt32(15);
            ReputationFlags[2] = GetInt32(16);
            ReputationFlags[3] = GetInt32(17);

            FactionName = GetString(19);

            return FactionId;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region CharStartOutfit.dbc
    public class CharStartOutfitReader : DbcReader<CharStartOutfit>
    {
        public CharStartOutfit Get(Classes classe, Races race, Genders gender)
        {
            return
                RecordDataIndexed.Values.ToArray()
                    .FirstOrDefault(c => c.Class == (int)classe && c.Race == (int)race && c.Gender == (int)gender);
        }
    }

    public class CharStartOutfit : DbcRecordBase
    {
        public int Id;
        public uint Race;
        public uint Class;
        public uint Gender;
        public int[] Items = new int[11];

        public override int Read()
        {
            Id = GetInt32(0);

            var tmp = GetUInt32(1);
            Race = tmp & 0xFF;
            Class = (tmp >> 8) & 0xFF;
            Gender = (tmp >> 16) & 0xFF;

            for (int i = 0; i < Items.Length; ++i)
                Items[i] = GetInt32(2 + i);

            return Id;
        }
    }
    #endregion

    #region ChrRaces.dbc
    public class ChrRacesReader : DbcReader<ChrRaces>
    {
        public ChrRaces GetData(Races id)
        {
            return RecordDataIndexed.Values.ToArray().FirstOrDefault(a => a.RaceId == (int)id);
        }
    }
    public class ChrRaces : DbcRecordBase
    {
        public int RaceId;
        public int FactionId;
        public int ModelM;
        public int ModelF;
        public int TeamId; //1 = Horde / 7 = Alliance
        public uint TaxiMask;
        public int CinematicId;
        public string Name;

        public override int Read()
        {
            RaceId = GetInt32(0);
            FactionId = GetInt32(2);
            ModelM = GetInt32(4);
            ModelF = GetInt32(5);
            TeamId = GetInt32(8);
            TaxiMask = GetUInt32(14);
            CinematicId = GetInt32(16);
            Name = GetString(17);

            return RaceId;
        }
    }
    #endregion

    #region EmotesText.dbc
    public class EmotesTextReader : DbcReader<EmotesText>
    {
        public EmotesText GetData(int id)
        {
            return RecordDataIndexed.Values.ToArray().FirstOrDefault(a => a.Id == id);
        }
    }

    public class EmotesText : DbcRecordBase
    {
        public int Id;
        public string EmoteName;
        public int EmoteId;
        public int[] MEmoteText = new int[16];

        public override int Read()
        {
            Id = GetInt32(0);
            EmoteName = GetString(1);
            EmoteId = GetInt32(2);

            for (int i = 0; i < MEmoteText.Length; ++i)
                MEmoteText[i] = GetInt32(2 + i);

            return Id;
        }
    }
    #endregion

    #region Map.dbc
    public class MapReader : DbcReader<Map>
    {
        public Map GetArea(int id)
        {
            return RecordDataIndexed.Values.ToArray().FirstOrDefault(a => a.MapId == id);
        }
    }

    public class Map : DbcRecordBase
    {
        public int MapId;
        public string MapName;
        public string MapString;

        public override int Read()
        {
            MapId = GetInt32(0);
            MapName = GetString(1);
            MapString = GetString(4);

            return MapId;
        }
    }
    #endregion
}