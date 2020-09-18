using System;
using System.Collections.Generic;
using MelonLoader;
using Harmony;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Compression;

namespace AudicaModding
{
    public class CustomSongDataLoader : MelonMod
    {

        private static bool SongDataLoaded = false;

        //dictionary with song id to custom data
        private static Dictionary<string, CustomData> AllCustomData = new Dictionary<string, CustomData>();

        //load custom data when song list is enabled
        [HarmonyPatch(typeof(SongSelect), "OnEnable", new Type[0])]
        private static class LoadDiffNamesOnSongSelectLoad
        {
            private static void Postfix(DifficultySelect __instance)
            {
                LoadCustomNames();
            }

        }

        public static class BuildInfo
        {
            public const string Name = "CustomSongDataLoader";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }

        private class NRBookmark
        {
            int type;
            float xPosMini;
            float xPosTop;
        }

        private class CustomData
        {
            //default game data (so it wont end up in our extra data dictionary)
            public string songID { get; set; }
            public string moggSong { get; set; }
            public string moggMainSong { get; set; }
            public string title { get; set; }
            public string artist { get; set; }
            public string midiFile { get; set; }
            public string fusionSpatialized { get; set; }
            public string fusionUnspatialized { get; set; }
            public string targetDrums { get; set; }
            public string sustainSongRight { get; set; }
            public string moggSustainSongRight { get; set; }
            public string sustainSongLeft { get; set; }
            public string moggSustainSongLeft { get; set; }
            public string fxSong { get; set; }
            public string moggFxSong { get; set; }
            public float tempo { get; set; }
            public string songEndEvent { get; set; }
            public float prerollSeconds { get; set; }
            public bool useMidiForCues { get; set; }

            public float songEndPitchAdjust { get; set; }

            public string highScoreEvent { get; set; }
            public bool hidden { get; set; }
            public string author { get; set; }
            public int offset { get; set; }
            public float previewStartSeconds { get; set; }
            //public NRBookmark[] bookmarks { get; set; }

            [JsonExtensionData]
            public IDictionary<string, JToken> _extraJsonData { get; set; }

            public bool HasCustomData()
            {
                if (_extraJsonData != null)
                    return _extraJsonData.Count > 0;
                else
                    return false;
            }

        }

        public static void LoadCustomNames()
        {
            if (!SongDataLoaded)
            {
                AllCustomData = new Dictionary<string, CustomData>();

                foreach (SongList.SongData data in SongList.I.songs.ToArray())
                {
                    ZipArchive SongFiles = ZipFile.OpenRead(data.foundPath);

                    foreach (ZipArchiveEntry entry in SongFiles.Entries)
                    {
                        if (entry.Name == "song.desc")
                        {
                            Stream songData = entry.Open();
                            StreamReader reader = new StreamReader(songData);
                            string descDump = reader.ReadToEnd();
                            CustomData JSONData = new CustomData();
                            JSONData = JsonConvert.DeserializeObject<CustomData>(descDump);
                            //MelonLogger.Log(descDump);

                            if (JSONData.HasCustomData())
                            {
                                AllCustomData[data.songID] = JSONData;
                                
                            }

                        }
                    }
                    SongDataLoaded = true;
                    SongFiles.Dispose();
                }
            }
        }

        public static bool SongHasCustomData(string songId)
        {
            return AllCustomData.ContainsKey(songId);
        }

        public static bool SongHasCustomDataKey(string songId, string key)
        {
            if (SongHasCustomData(songId))
            {
                return AllCustomData[songId]._extraJsonData.ContainsKey(key);
            }
            else
                return false;
        }

        public static T GetCustomData<T>(string songID, string key)
        {
            return AllCustomData[songID]._extraJsonData[key].ToObject<T>();
        }
    }
}



