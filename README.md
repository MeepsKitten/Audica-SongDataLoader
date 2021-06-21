# Song Data Loader
 A utility that allows mods to read default and custom song data from `song.desc`
 
# Download
[Song Data Loader Download](https://github.com/MeepsKitten/CustomSongDataLoader/releases/latest)

[Legacy version for MelonLoader 0.2.7.X](https://github.com/MeepsKitten/Audica-SongDataLoader/releases/tag/v1.2.0)
 
# Mods That use Song Data Loader
* [Song Browser](https://github.com/Silzoid/SongBrowser)
* [Meeps' UI Enhancements](https://github.com/MeepsKitten/Meeps-Audica-UI-Enhancements)
 
(Missing a mod? Contact CB#1997 on Discord)

# Using Song Data Loader in your mod
* Grab [this dll](https://github.com/MeepsKitten/CustomSongDataLoader/releases/latest)
* Add it as a refrence in your project
* Include the dll with your mod download (or ask them to download it from [here](https://github.com/MeepsKitten/CustomSongDataLoader/releases/latest))
 
Need help? Contact CB#1997 on Discord
 
# "Documentation"
## public class SongDataLoader
### Properties:
* <code>public static Dictionary<string, SongData> AllSongData</code>
Contains all the song data for all loaded songs. Dictionary string is the song ID. Returns a SongData class (documented) below
### Methods:
* <code>public static void ReloadSongData()</code>
Reloads the song data for all songs

## public class SongData
### Properties:
[insert every default audica song.desc variable]

### Methods:
* <code>public bool SongHasCustomData()</code>
Returns true if there is any custom song data present for a specified SongData, otherwise false

* <code>public bool SongHasCustomDataKey(string key)</code>
Returns true if a specified SongData has custom data for a specified key

* <code>public T GetCustomData`<T>`(string key)</code>
 generic function that gets data for a specific SongData that corresponds to a given key
 
 * <code>public static bool IsDataLoaded()</code>
 returns true if data loading has finished


### Usage Examples
<pre><code>//return if the song doesnt have custom data
if (!SongDataLoader.AllSongData[song.songID].HasCustomData())
  break;

//if the song has data for the key "customExpert"
if (SongDataLoader.AllSongData[song.songID].SongHasCustomDataKey("customExpert"))
{

}

//get a default data value. tempo in this case
SongDataLoader.AllSongData[song.songID].tempo
</code></pre>
