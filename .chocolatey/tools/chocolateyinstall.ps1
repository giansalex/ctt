
$ErrorActionPreference = 'Stop'

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$packageArgs = @{
    PackageName    = $env:ChocolateyPackageName
    unzipLocation  = $toolsDir
    url        = 'https://github.com/giansalex/ctt/releases/download/1.0.1/ctt-win32.zip' 
    url64      = 'https://github.com/giansalex/ctt/releases/download/1.0.1/ctt-win64.zip' 

    checksum       = 'C69BD0CC96BA02BED730368A5138790C5049A86E19E043B4C30EBC92936D6562'
    checksumType   = 'sha256'
    checksum64     = 'BA733BC7A1E22588AC6973944B13111F2C577182A7BBE0CF0B807A94EE22C783'
    checksumType64 = 'sha256'
}

Install-ChocolateyZipPackage @packageArgs
