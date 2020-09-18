using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using AudicaModding;

[assembly: AssemblyTitle(CustomSongDataLoader.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(CustomSongDataLoader.BuildInfo.Company)]
[assembly: AssemblyProduct(CustomSongDataLoader.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + CustomSongDataLoader.BuildInfo.Author)]
[assembly: AssemblyTrademark(CustomSongDataLoader.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(CustomSongDataLoader.BuildInfo.Version)]
[assembly: AssemblyFileVersion(CustomSongDataLoader.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(CustomSongDataLoader), CustomSongDataLoader.BuildInfo.Name, CustomSongDataLoader.BuildInfo.Version, CustomSongDataLoader.BuildInfo.Author, CustomSongDataLoader.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]