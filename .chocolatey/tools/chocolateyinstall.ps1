
$ErrorActionPreference = 'Stop'

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$packageArgs = @{
    PackageName    = $env:ChocolateyPackageName
    unzipLocation  = $toolsDir
    url        = 'https://github.com/giansalex/ctt/releases/download/1.0.0/ctt-win32.zip' 
    url64      = 'https://github.com/giansalex/ctt/releases/download/1.0.0/ctt-win64.zip' 

    checksum       = '82BF9F9F445B7D8FCA3DBB6F87DF636F7F81AC5B0BB41AEF94CF2F2B4DFE6B98'
    checksumType   = 'sha256'
    checksum64     = '8D1FD51F7EF58D068353048F4EF2E60842BD84F9AC5E0A7ED536F2DA1F33089A'
    checksumType64 = 'sha256'
}

Install-ChocolateyZipPackage @packageArgs
