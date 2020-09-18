Write-Host -ForegroundColor green "`nAudica modding template quick start"
Write-Host -ForegroundColor green "===================================`n"

$DefaultName = "AudicaMod"
$DefaultAuthor = "DefinitelyNotHarmonix"
$DefaultVersion = "0.1.0"
$DefaultPath = "C:\Program Files (x86)\Steam\steamapps\common\Audica\"

# -- Find audica location if possible --
Get-ChildItem HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall -Recurse | ForEach-Object {
    $Name = (Get-ItemProperty $_.PSPath) | Select-Object -ExpandProperty "DisplayName" -ErrorAction SilentlyContinue
    $InstallLocation = (Get-ItemProperty $_.PSPath) | Select-Object -ExpandProperty "InstallLocation" -ErrorAction SilentlyContinue
    if ($Name -match "Audica") {
        $DefaultPath = $InstallLocation
        Write-Host "✅ Found audica installation at $InstallLocation`n"
    }
}


# -- INPUT --

Write-Host "Let's configure the project and post-build script! (Enter blank to accept defaults in [] brackets)"

$Name = Read-Host -Prompt "Mod Name [`"${DefaultName}`"]"
if ([string]::IsNullOrWhiteSpace($Name)) { $Name=$DefaultName; }

$Author = Read-Host -Prompt "Mod Author [`"${DefaultAuthor}`"]"
if ([string]::IsNullOrWhiteSpace($Author)) { $Author=$DefaultAuthor; }

$Version = Read-Host -Prompt "Mod Version [`"${DefaultVersion}`"]"
if ([string]::IsNullOrWhiteSpace($Version)) { $Version=$DefaultVersion; }

do {
    $pathValid = "False"
    $AudicaPath = Read-Host -Prompt "Where is your main Audica directory? [${DefaultPath}]"
    
    if ([string]::IsNullOrWhiteSpace($AudicaPath)) { $AudicaPath=$DefaultPath; }
    if (Test-Path -Path "${AudicaPath}\Audica.exe") { $pathValid = "True" }
    else { Write-Host -ForegroundColor red "That doesn't look right! Make sure it's the full path to the directory where `"Audica.exe`" is." }
}while($pathValid -ne "True")


# -- COPY REFERENCE LIB FILES --
Write-Host -ForegroundColor yellow "Copying MelonLoader libs ..."
try {
    Invoke-Expression -Command ".\copy-references.ps1 `"${AudicaPath}`" AudicaMod" 
} catch {
    Write-Host -ForegroundColor red $_ 
    Write-Host 'Press any key to exit...';
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
    exit 1
}
Write-Host -ForegroundColor green "Done!`n"


# -- WRITE CHANGES --

# Write name in solution / project structure
Write-Host -ForegroundColor yellow "Renaming csproj + assembly file to ${Name} ..."
Rename-Item "${PSScriptRoot}\AudicaMod\AudicaModStarter.csproj" "${PSScriptRoot}\AudicaMod\${Name}.csproj"
((Get-Content "${PSScriptRoot}\AudicaModding.sln" -Raw) -replace 'AudicaModStarter', ${Name}) | Set-Content "${PSScriptRoot}\AudicaModding.sln"
((Get-Content "${PSScriptRoot}\AudicaMod\${Name}.csproj" -Raw) -replace 'AudicaModStarter', ${Name}) | Set-Content "${PSScriptRoot}\AudicaMod\${Name}.csproj"
Write-Host -ForegroundColor green "Done!`n"


# Set post-build dll copy
Write-Host -ForegroundColor yellow "Setting post-build script to copy mod dll to ${DefaultPath}\Mods\${Name}.dll ..."
$PostBuildScript = "copy `"`$(TargetPath)`" `"${AudicaPath}\Mods\`$(TargetFileName)`""
((Get-Content "${PSScriptRoot}\AudicaMod\${Name}.csproj" -Raw) -replace 'echo "Done!"', ${PostBuildScript}) | Set-Content "${PSScriptRoot}\AudicaMod\${Name}.csproj"
Write-Host -ForegroundColor green "Done!`n"


# Set mod metadata
Write-Host -ForegroundColor yellow "Setting mod name / author / version in src/Main.cs ..."
((Get-Content "${PSScriptRoot}\AudicaMod\src\Main.cs" -Raw) -replace 'AudicaModStarter',${Name}) | Set-Content "${PSScriptRoot}\AudicaMod\src\Main.cs"
((Get-Content "${PSScriptRoot}\AudicaMod\src\Main.cs" -Raw) -replace 'AudicaModAuthor', ${Author}) | Set-Content "${PSScriptRoot}\AudicaMod\src\Main.cs"
((Get-Content "${PSScriptRoot}\AudicaMod\src\Main.cs" -Raw) -replace '1.0.0', ${Version}) | Set-Content "${PSScriptRoot}\AudicaMod\src\Main.cs"
Write-Host -ForegroundColor green "Done!`n"

Write-Host 'Press any key to exit...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');