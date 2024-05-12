Write-Host @"
This script is used to run the Azure Cosmos DB Data Migration Tool (DMT). 

You have two options to provide the DMT to the script:

1. Download the DMT yourself from the official website (https://github.com/AzureCosmosDB/data-migration-desktop-tool/releases/download/2.1.5/dmt-2.1.5-win-x64.zip), extract the zip file, and note the folder where `dmt.exe` is located. When you run the script, it will prompt you to provide this folder path.

2. Allow the script to download the DMT for you. When you run the script, it will ask you if you want to download the DMT. If you answer 'yes', the script will download the latest version of the DMT, extract it, and use it to perform the data migration.

Please ensure you have the necessary permissions to download files and run executables on your machine.
"@

$DownloadDMT = Read-Host -Prompt "Do you want to download the DMT option (1)? (y/n)"
$DMTPath = ""

if ($DownloadDMT -eq "y") {
    # Define the URL of the DMT zip file
    $DMTUrl = "https://github.com/AzureCosmosDB/data-migration-desktop-tool/releases/download/2.1.5/dmt-2.1.5-win-x64.zip"

    # Define the path where the DMT zip file will be downloaded
    $DownloadPath = "$env:TEMP\dmt-2.1.5-win-x64.zip"

    # Download the DMT zip file
    Invoke-WebRequest -Uri $DMTUrl -OutFile $DownloadPath

    # Define the path where the DMT will be extracteds
    $ExtractPath = "$env:TEMP\DMT"

    # Extract the DMT zip file
    Expand-Archive -Path $DownloadPath -DestinationPath $ExtractPath -Force
    
    $DMTPath = Join-Path -Path $ExtractPath -ChildPath "windows-package"
    Write-Host "The DMT.exe has been extracted to: $ExtractPath"
}
else
{
    $DMTPath = Read-Host -Prompt "Please provide the path where DMT.exe is located"
}

 
$ScriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition
 
$MigrationFiles = Get-ChildItem -Path $ScriptDirectory -Filter *.cosmosdb.json

foreach ($File in $MigrationFiles) {
    # Run the DMT with the current migration file
     Write-Host "Running migration with settings file: $($File.Name)"
    & "$DMTPath\dmt.exe" --settings "$($File.FullName)"
}
