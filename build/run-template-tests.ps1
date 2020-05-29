$ErrorActionPreference = 'Stop'

$templates= 'UnoApp', 'UnoApp-WinUI'

function Get-Configuration(
    [bool]$uwp = $false,
    [bool]$android = $false,
    [bool]$iOS = $false,
    [bool]$macOS = $false,
    [bool]$wasm = $false,
    [bool]$wasmVsCode = $false)
{
    $uwpFlag = '-uwp'
    $androidFlag = '-android'
    $iOSFlag = '-ios'
    $macOSFlag = '-macos'
    $wasmFlag = '-wasm'
    $wasmVsCodeFlag = '--vscodeWasm'

    $a = If ($uwp)        { $uwpFlag }        Else { $uwpFlag        + '=false' }
    $b = If ($android)    { $androidFlag }    Else { $androidFlag    + '=false' }
    $c = If ($iOS)        { $iOSFlag }        Else { $iOSFlag        + '=false' }
    $d = If ($macOS)      { $macOSFlag }      Else { $macOSFlag      + '=false' }
    $e = If ($wasm)       { $wasmFlag }       Else { $wasmFlag       + '=false' }
    $f = If ($wasmVsCode) { $wasmVsCodeFlag } Else { $wasmVsCodeFlag + '=false' }

    @($a, $b, $c, $d, $e, $f)
}

$templateConfigurations =
@(
    (Get-Configuration -uwp 1),
    (Get-Configuration -android 1),
    (Get-Configuration -iOS 1),
    (Get-Configuration -macOS 1),
    (Get-Configuration -wasm 1),
    (Get-Configuration -uwp 1 -android 1 -iOS 1 -macOS 1 -wasm 1),
    (Get-Configuration -uwp 1 -android 1 -iOS 1 -macOS 1 -wasm 1 -wasmVsCode 1)
)

foreach($template in $templates)
{
    for($i = 0; $i -lt $templateConfigurations.Length; $i++)
    {
        $arguments = @('new', $template, '-n', ($template + $i)) + $templateConfigurations[$i]

        dotnet $arguments

        $arguments = @('/r', '/p:Configuration=Release', '/detailedsummary', ($template + $i + "/" + $template + $i + ".sln"))

        msbuild $arguments
    }
}
