############################################################################
###                                                                      ###
###                    NUGET  PACKAGE and PUBLISH                        ###
###                                                                      ###
############################################################################

param (
  [string]$apiKey,
  [string]$folder = $PSScriptRoot,
  [string]$feedSource = "https://nuget.org",
  [switch]$help
)

function DisplayHelp()
{
    "Available command line arguments:"
    "    =>      folder: Directory location where the nupkg files will be saved. Default: current directory."
    "    =>  feedSource: Url of the feed to publish the package(s) to. Default: https://nuget.org"
    "    =>     api key: Feed secret api key. Default: none."
    ""
    ""
    "eg. & '.\publish.ps1' -folder C:\temp\TempNuGetPackages -apiKey ABCD-EFG..."
    "eg. & '.\publish.ps1' -folder C:\temp\TempNuGetPackages -apiKey ABCD-EFG... -feedSource http://myurl.com/"
    ""
}

function CleanUpInputArgs()
{
    $apiKey = $apiKey.Trim()
    $feedSource = $feedSource.Trim()
}

function DisplayCommandLineArgs()
{
    
    if ($apiKey)
    {
        if ($apiKey.length -gt 6)
        {
            $truncatedApiKey = "......" + $apiKey.substring($apiKey.length - 6)
        }
        else
        {
            for($i = 0; $i -lt $apiKey.length; $i++)
            {
                $truncatedApiKey += "*"
            }
        }
    }
    else
    {
        $truncatedApiKey = "<none provided>"
    }
     
    "Options provided:"
    "    => folder: $folder"
    "    => feedSource: $feedSource"
    "    => apiKey: $truncatedApiKey"
    ""

    if ($folder -eq "")
    {
        ""
        "**** A folder parameter provided cannot be an empty string."
        ""
        ""
        throw;
    }

    if ($feedSource -eq "")
    {
        ""
        "**** The NuGet push source parameter provided cannot be an empty string."
        ""
        ""
        throw;
    }

    if ($apiKey -eq "")
    {
        ""
        "**** The apiKey source parameter provided cannot be an empty string."
        ""
        ""
        throw;
    }

    # Setup the nuget path.
    if (-Not $nuget -eq "")
    {
        $global:nugetExe = $nuget
    }
    else
    {
        # Assumption, nuget.exe is the current folder where this file is.
        $global:nugetExe = Join-Path $PSScriptRoot "nuget.exe" 
    }
	"$global:nugetExe"

    if (!(Test-Path $global:nugetExe -PathType leaf))
    {
        ""
        "**** Nuget file was not found. Please provide the -nuget parameter with the nuget.exe path -or- copy the nuget.exe to the current folder, side-by-side to this powershell file."
        ""
        ""
        throw;
    }
}

function PushThePackagesToNuGet()
{
    ""
    "Getting all *.nupkg's files to push to : $feedSource"

    $files = Get-ChildItem $folder -Filter *.nupkg

    if ($files.Count -eq 0)
    {
        ""
        "**** No nupkg files found in the directory: $folder"
        "Terminating process."
        throw;
    }

    "Found: " + $files.Count + " files :)"

    foreach($file in $files)
    {
        &$nugetExe push ($file.FullName) -Source $feedSource -apiKey $apiKey

        ""
    }
}

##############################################################################
##############################################################################

$ErrorActionPreference = "Stop"
$global:nugetExe = ""


""
" ---------------------- start script ----------------------"
""
""
"  Starting NuGet publishing script"
""
"  This script will look for -all- *.nupack files in a directory"
"  and publish them to a NuGet server, if an api key was provided."
""
"  *** NEED HELP? use the -help argument."
"      eg.  & '.\publish.ps1' -help"
""
""

if ($help)
{
    DisplayHelp;
}
else
{
    CleanUpInputArgs
    DisplayCommandLineArgs
    PushThePackagesToNuGet
}

""
""
" ---------------------- end of script ----------------------"
""
""