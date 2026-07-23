# Starts SupportTicket API + Web for local development (same terminal, no extra windows).
# Usage: .\run-apps.ps1
# Stop: Ctrl+C

$ErrorActionPreference = "Stop"

$root = $PSScriptRoot
$apiDir = Join-Path $root "src\SupportTicket.Api"
$webDir = Join-Path $root "src\SupportTicket.Web"
$logDir = Join-Path $env:TEMP "supportticket-run"
New-Item -ItemType Directory -Force -Path $logDir | Out-Null
$apiOutLog = Join-Path $logDir "api.out.log"
$apiErrLog = Join-Path $logDir "api.err.log"
$webOutLog = Join-Path $logDir "web.out.log"
$webErrLog = Join-Path $logDir "web.err.log"

if (-not (Test-Path $apiDir)) {
    Write-Error "API project not found at $apiDir"
}
if (-not (Test-Path $webDir)) {
    Write-Error "Web project not found at $webDir"
}

$webEnv = Join-Path $webDir ".env"
$webEnvExample = Join-Path $webDir ".env.example"
if (-not (Test-Path $webEnv) -and (Test-Path $webEnvExample)) {
    Copy-Item $webEnvExample $webEnv
    Write-Host "Created $webEnv from .env.example"
}

$nodeModules = Join-Path $webDir "node_modules"
if (-not (Test-Path $nodeModules)) {
    Write-Host "Installing frontend dependencies..."
    Push-Location $webDir
    try {
        npm install
        if ($LASTEXITCODE -ne 0) {
            Write-Error "npm install failed with exit code $LASTEXITCODE"
        }
    }
    finally {
        Pop-Location
    }
}

$viteJs = Join-Path $webDir "node_modules\vite\bin\vite.js"
if (-not (Test-Path $viteJs)) {
    Write-Error "Vite not found at $viteJs. Run npm install in src\SupportTicket.Web first."
}

$apiUrl = "http://localhost:5000"
$swaggerUrl = "$apiUrl/swagger"
$webUrl = "http://localhost:5173"

function Test-UrlReady {
    param([string]$Url)

    try {
        $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 1
        return ($response.StatusCode -ge 200 -and $response.StatusCode -lt 500)
    }
    catch {
        return $false
    }
}

function Stop-Tree {
    param([System.Diagnostics.Process]$Process)

    if ($null -eq $Process) { return }
    try {
        if (-not $Process.HasExited) {
            taskkill /PID $Process.Id /T /F 2>$null | Out-Null
        }
    }
    catch { }
}

Write-Host ""
Write-Host "Starting Support Ticket apps..."
Write-Host "  API:  $swaggerUrl"
Write-Host "  Web:  $webUrl"
Write-Host "Press Ctrl+C to stop both."
Write-Host ""

$apiProc = $null
$webProc = $null

try {
    foreach ($log in @($apiOutLog, $apiErrLog, $webOutLog, $webErrLog)) {
        if (Test-Path $log) { Remove-Item $log -Force }
    }

    # Launch API via cmd so env vars apply with Start-Process redirection.
    $apiCmd = "set ASPNETCORE_ENVIRONMENT=Development&& set ASPNETCORE_URLS=$apiUrl&& dotnet run --no-launch-profile"
    $apiProc = Start-Process `
        -FilePath "cmd.exe" `
        -ArgumentList "/c", $apiCmd `
        -WorkingDirectory $apiDir `
        -WindowStyle Hidden `
        -RedirectStandardOutput $apiOutLog `
        -RedirectStandardError $apiErrLog `
        -PassThru

    # Run Vite via node directly — npm.cmd fails under redirected/no-window starts.
    $webProc = Start-Process `
        -FilePath "node" `
        -ArgumentList "`"$viteJs`" --host localhost --port 5173" `
        -WorkingDirectory $webDir `
        -WindowStyle Hidden `
        -RedirectStandardOutput $webOutLog `
        -RedirectStandardError $webErrLog `
        -PassThru

    # Open each app as soon as it is ready (both polled together — no sequential wait).
    $apiOpened = $false
    $webOpened = $false
    $deadline = (Get-Date).AddSeconds(90)

    Write-Host "Waiting for both apps in parallel..."
    while ((-not $apiOpened -or -not $webOpened) -and (Get-Date) -lt $deadline) {
        if ($apiProc.HasExited) {
            $errText = if (Test-Path $apiErrLog) { Get-Content $apiErrLog -Raw } else { "" }
            Write-Error "API exited early (code $($apiProc.ExitCode)).`n$errText"
        }
        if ($webProc.HasExited) {
            $errText = if (Test-Path $webErrLog) { Get-Content $webErrLog -Raw } else { "" }
            Write-Error "Web exited early (code $($webProc.ExitCode)).`n$errText"
        }

        if (-not $apiOpened -and (Test-UrlReady $apiUrl)) {
            Write-Host "API is ready."
            Start-Process $swaggerUrl
            $apiOpened = $true
        }
        if (-not $webOpened -and (Test-UrlReady $webUrl)) {
            Write-Host "Web is ready."
            Start-Process $webUrl
            $webOpened = $true
        }

        if (-not $apiOpened -or -not $webOpened) {
            Start-Sleep -Milliseconds 300
        }
    }

    if (-not $apiOpened) {
        Write-Warning "API not ready in time; opening $swaggerUrl anyway."
        Start-Process $swaggerUrl
    }
    if (-not $webOpened) {
        Write-Warning "Web not ready in time; opening $webUrl anyway."
        Start-Process $webUrl
    }

    Write-Host "Opened $swaggerUrl and $webUrl in your browser."
    Write-Host "Apps are running. Press Ctrl+C to stop."
    Write-Host "Logs: $logDir"

    while ($true) {
        if ($apiProc.HasExited) {
            Write-Warning "API process exited with code $($apiProc.ExitCode)."
            if (Test-Path $apiErrLog) { Get-Content $apiErrLog }
            break
        }
        if ($webProc.HasExited) {
            Write-Warning "Web process exited with code $($webProc.ExitCode)."
            if (Test-Path $webErrLog) { Get-Content $webErrLog }
            break
        }
        Start-Sleep -Seconds 1
    }
}
finally {
    Write-Host ""
    Write-Host "Stopping apps..."
    Stop-Tree $webProc
    Stop-Tree $apiProc
    Write-Host "Stopped."
}
