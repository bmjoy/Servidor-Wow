﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Common.Database.Xml;
using Common.Globals;
using Common.Helpers;

namespace Common.Database
{
    public class XmlReader
    {
        public static ItemsXml ItemsXml { get; private set; }
        public static RacesXml RacesXml { get; private set; }
        public static ObjectsXml[] ObjectsXml { get; set; }
        public static string[] Files { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static void Boot()
        {
            Log.Print(LogType.Loading, "Loading XML .......................... [OK]");
            try
            {
                // Items
                XmlSerializer serializerItems = new XmlSerializer(typeof(ItemsXml));
                StreamReader readerItems = new StreamReader("xml/items.xml");
                ItemsXml = serializerItems.Deserialize(readerItems) as ItemsXml;
                if (ItemsXml != null)
                    Log.Print(LogType.Loading, $"=_ Items Loaded: {ItemsXml.Item.Count()}");
                readerItems.Close();

                // Races
                XmlSerializer serializerRaces = new XmlSerializer(typeof(RacesXml));
                StreamReader readerRaces = new StreamReader("xml/races.xml");
                RacesXml = serializerRaces.Deserialize(readerRaces) as RacesXml;
                if (RacesXml != null)
                    Log.Print(LogType.Loading, $"=_ Races Loaded: {RacesXml.race.Count()}");
                readerRaces.Close();

                // Game Objects
                if (Directory.Exists("xml/world"))
                {
                    Files = Directory.GetFiles("xml/world", @"*.xml", SearchOption.TopDirectoryOnly);

                    foreach (var VARIABLE in Files)
                    {
                        XmlSerializer serielizerObjects = new XmlSerializer(typeof(ObjectsXml));
                        StreamReader readerObjcets = new StreamReader(VARIABLE);
                        ObjectsXml = serializerRaces.Deserialize(readerRaces) as ObjectsXml[];
                        if (ObjectsXml != null)
                            Log.Print(LogType.Loading, $"=_ Races Loaded: {ObjectsXml.Count()}");
                        readerObjcets.Close();
                    }
                }

            }
            catch (Exception e)
            {
                var trace = new StackTrace(e, true);
                Log.Print(LogType.Error, $"{e.Message}: {e.Source}\n{trace.GetFrame(trace.FrameCount - 1).GetFileName()}:{trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber()}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public static racesRace GetRace(Races race)
        {
            try
            {
                return RacesXml?.race.FirstOrDefault(a => a.id == (int) race);
            }
            catch (Exception e)
            {
                var trace = new StackTrace(e, true);
                Log.Print(LogType.Error, $"{e.Message}: {e.Source}\n{trace.GetFrame(trace.FrameCount - 1).GetFileName()}:{trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber()}");
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="race"></param>
        /// <param name="classe"></param>
        /// <returns></returns>
        public static racesRaceClass GetRaceClass(Races race, Classes classe)
        {
            try
            {
                return RacesXml.race.FirstOrDefault(ok => ok.id == (int) race)?.classes.First(ba => ba.id == classe.ToString());
            }
            catch (Exception e)
            {
                var trace = new StackTrace(e, true);
                Log.Print(LogType.Error, $"{e.Message}: {e.Source}\n{trace.GetFrame(trace.FrameCount - 1).GetFileName()}:{trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber()}");
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ItemsItem GetItem(int value)
        {
            try
            {
                return ItemsXml.Item.First(a => a.id == value);
            }
            catch (Exception e)
            {
                var trace = new StackTrace(e, true);
                Log.Print(LogType.Error, $"{e.Message}: {e.Source}\n{trace.GetFrame(trace.FrameCount - 1).GetFileName()}:{trace.GetFrame(trace.FrameCount - 1).GetFileLineNumber()}");
            }

            return null;
        }
    }
}
