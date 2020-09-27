$AudicaFolder=$args[0]
$ModFolder=$args[1] 

$ReferenceFiles = "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll", "Il2CppMono.Security.dll", "Il2Cppmscorlib.dll", "Il2CppSystem.Configuration.dll", "Il2CppSystem.Core.dll", "Il2CppSystem.dll", "Il2CppSystem.Runtime.Serialization.dll", "Il2CppSystem.Xml.dll", "Oculus.VR.dll", "Sanford.Multimedia.Midi.dll", "SonyNP.dll", "SteamVR.dll", "TwitchChatter.dll", "UnhollowerBaseLib.dll", "UnhollowerRuntimeLib.dll", "Unity.TextMeshPro.dll", "UnityEngine.AIModule.dll", "UnityEngine.AnimationModule.dll", "UnityEngine.AudioModule.dll", "UnityEngine.CoreModule.dll", "UnityEngine.IMGUIModule.dll", "UnityEngine.ParticleSystemModule.dll", "UnityEngine.Physics2DModule.dll", "UnityEngine.PhysicsModule.dll", "UnityEngine.TerrainModule.dll", "UnityEngine.TextCoreModule.dll", "UnityEngine.TextRenderingModule.dll", "UnityEngine.UI.dll", "UnityEngine.UIModule.dll", "UnityEngine.UnityWebRequestModule.dll", "UnityEngine.UnityWebRequestWWWModule.dll", "UnityEngine.VehiclesModule.dll", "UnityEngine.XRModule.dll"

if ([string]::IsNullOrWhiteSpace($AudicaFolder) -Or [string]::IsNullOrWhiteSpace($ModFolder)) { 
    Write-Host "Copys extracted melon loader references from audica folder to mod project references."
    Write-Host "Usage:"
    Write-Host ".\copy-references.ps1 path\to\audica\folder name-of-mod-folder"
}


# -- CHECK THIS IS ACTUAL AUDICA FOLDER --
if (!(Test-Path -Path "${AudicaFolder}\Audica.exe")) {
    throw "Provided path does not exist or is not the audica folder!"
}

# -- CHECK FOR MELON MOD --
if (!(Test-Path -Path "${AudicaFolder}\MelonLoader\Managed\") -Or !(Test-Path -Path "${AudicaFolder}\version.dll")) {
    throw "MelonLoader has not been installed in the Audica folder! See the README."
}

# -- CHECK FOR GENERATED ASSEMBLY DLLS --
if (!(Test-Path -Path "${AudicaFolder}\MelonLoader\Managed\UnityEngine.dll")) {
    throw "MelonLoader is installed but no assemblies have been found. Have you run the game at least once since installing?"
}

# -- CHECK FOR MOD FOLDER --
# absolute check first
if (!(Test-Path -Path "${ModFolder}")) {

    # if that failed, try a relative check
    ${ModFolder} = "${PSScriptRoot}\${ModFolder}"
    if (!(Test-Path -Path "${ModFolder}")) {
        throw "Mod project folder ${PSScriptRoot}\${ModFolder} does not exist."
    }
}

# -- COPY FILES --

Write-Host -ForegroundColor green "Checks passed! Copying files ..."

Write-Host "Copying MelonLoader.ModHandler.dll ..."
Copy-Item "${AudicaFolder}\MelonLoader\MelonLoader.ModHandler.dll" -Destination "${ModFolder}\Lib"
foreach ($file in $ReferenceFiles) {
    Write-Host "Copying ${file} ..."
    Copy-Item "${AudicaFolder}\MelonLoader\Managed\${file}" -Destination "${ModFolder}\Lib"
}