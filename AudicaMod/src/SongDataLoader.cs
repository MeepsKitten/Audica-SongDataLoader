using System;
using System.Collections.Generic;
using MelonLoader;
using Harmony;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Threading;

namespace AudicaModding
{
    public class SongDataLoader : MelonMod
    {

        private static bool SongDataLoaded = false;

        public static bool IsDataLoaded()
        {
            return SongDataLoaded;
        }

        //dictionary with song id to custom data
        public static Dictionary<string, SongData> AllSongData = new Dictionary<string, SongData>();

        public override void OnApplicationStart()
        {
            SongList.OnSongListLoaded.On(new Action(() => { LoadSongData(); }));
        }

        public static class BuildInfo
        {
            public const string Name = "SongDataLoader";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.2.0"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }

        public class NRBookmark
        {
            int type;
            float xPosMini;
            float xPosTop;
            string text;
            Color color;
            int uiColor;
        }

        public class SongData
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
            public bool hidden { get; set; }
            public int offset { get; set; }


            //optional
            public NRBookmark[] bookmarks { get; set; }

            public string albumArt { get; set; }
            public byte[] albumArtData { get; set; }
            public string author { get; set; }
            public float previewStartSeconds { get; set; }
            public string targetDrums { get; set; }
            public float songEndPitchAdjust { get; set; }
            public string highScoreEvent { get; set; }

            [JsonExtensionData]
            private IDictionary<string, JToken> _extraJsonData { get; set; }

            public bool HasCustomData()
            {
                if (!SongDataLoaded)
                {
                    MelonLogger.Log("Song data not loaded before attempted use");
                    return false;
                }

                if (_extraJsonData != null)
                    return _extraJsonData.Count > 0;
                else
                    return false;
            }

            //Tell you if a specified song has data for a specified key
            public bool SongHasCustomDataKey(string key)
            {
                return _extraJsonData.ContainsKey(key);
            }

            //generic function that gets data for a specific song that corresponds to a given key
            public T GetCustomData<T>(string key)
            {
                return _extraJsonData[key].ToObject<T>();
            }
        }



        //When called, this function will update 'AllCustomData' with data from every song installed (already called on [HarmonyPatch(typeof(SongSelect), "OnEnable", new Type[0])])
        public static void LoadSongData()
        {
            if (!SongDataLoaded)
            {
                AllSongData = new Dictionary<string, SongData>();

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

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
                                SongData JSONData = new SongData();
                                JSONData = JsonConvert.DeserializeObject<SongData>(descDump, new JsonSerializerSettings
                                {
                                    Error = (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args) =>
                                    {
                                        args.ErrorContext.Handled = true;
                                        MelonLogger.LogError(data.zipPath + ": song.desc has invalid values");
                                    }
                                });

                                if (JSONData.albumArt != null && (JSONData.albumArt.Length > 0))
                                {
                                    //MelonLogger.Log(data.zipPath + " has cover path. looking for cover (" + JSONData.albumArt + ")");
                                    foreach (ZipArchiveEntry file in SongFiles.Entries)
                                    {
                                        if (file.Name == JSONData.albumArt)
                                        {
                                            //MelonLogger.Log(data.zipPath + " has cover art");
                                            Stream filestream = file.Open();

                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                filestream.CopyTo(ms);
                                                JSONData.albumArtData = ms.ToArray();
                                            }                                          
                                        }
                                    }

                                }

                                AllSongData[data.songID] = JSONData;
                            }
                        }
                        SongDataLoaded = true;
                        SongFiles.Dispose();
                    }
                    MelonLogger.Log("Done Loading");
                }).Start();
            }
        }

        public static void ReloadSongData()
        {
            SongDataLoaded = false;
            LoadSongData();

            
        }


    }
}



