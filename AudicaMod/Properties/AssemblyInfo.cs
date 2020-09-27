using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using AudicaModding;

[assembly: AssemblyTitle(SongDataLoader.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(SongDataLoader.BuildInfo.Company)]
[assembly: AssemblyProduct(SongDataLoader.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + SongDataLoader.BuildInfo.Author)]
[assembly: AssemblyTrademark(SongDataLoader.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(SongDataLoader.BuildInfo.Version)]
[assembly: AssemblyFileVersion(SongDataLoader.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(SongDataLoader), SongDataLoader.BuildInfo.Name, SongDataLoader.BuildInfo.Version, SongDataLoader.BuildInfo.Author, SongDataLoader.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Harmonix Music Systems, Inc.", "Audica")]