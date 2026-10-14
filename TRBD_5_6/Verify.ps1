$ErrorActionPreference = 'Stop'
$projectRoot = $PSScriptRoot
$project = Join-Path $projectRoot 'WindowsFormsApp1\WindowsFormsApp1.csproj'
$packages = Join-Path $projectRoot '.packages'
& msbuild $project /restore /t:Build /p:Configuration=Release "/p:RestorePackagesPath=$packages" /v:minimal
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
$executable = Join-Path $projectRoot 'WindowsFormsApp1\bin\Release\WindowsFormsApp1.exe'
$resultPath = Join-Path $projectRoot 'verification'
$verificationProcess = Start-Process -FilePath $executable -ArgumentList '--verify', ('"' + $resultPath + '"') -WindowStyle Hidden -Wait -PassThru
Get-Content -LiteralPath (Join-Path $resultPath 'verification.txt') -Encoding UTF8
if ($verificationProcess.ExitCode -ne 0) { throw "Verification failed: $($verificationProcess.ExitCode)" }
