# Custom Song Data Loader
 A utility that allows mods to read custom song data
 
 feel free to include [this dll](https://github.com/MeepsKitten/CustomSongDataLoader/releases/latest) with any dependant mods you make :D
 

# "Documentation"
## public class CustomSongDataLoader : MelonMod
### Methods:
* <code>public static bool SongHasCustomData(string songId)</code>
Returns true if there is any custom song data present for a specified songId, otherwise false

* <code>public static bool SongHasCustomDataKey(string songId, string key)</code>
Returns true if a specified songId has custom data for a specified key

* <code>public static T GetCustomData<T>(string songID, string key)</code>
 generic function that gets data for a specific songId that corresponds to a given key


### Usage Examples
<pre><code>//return if the song doesnt have custom data
if (!CustomSongDataLoader.SongHasCustomData(song.songID))
  return;

//if the song has data for the key "customExpert"
if (CustomSongDataLoader.SongHasCustomDataKey(song.songID, "customExpert"))
{
  //get the data for the song under the key "customExpert" as a string and use it for a text box
  textobject.SetText(CustomSongDataLoader.GetCustomData<string>(song.songID, "customExpert"));
}
</code></pre>
