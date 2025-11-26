function Get-JsonLeafPaths {
    param(
        [Parameter(Mandatory)]
        [AllowNull()]
        [AllowEmptyString()]
        $Object,
        [string]$Prefix = ""
    )
    
    # Handle null, empty, or whitespace-only values
    if ($null -eq $Object -or ($Object -is [string] -and [string]::IsNullOrWhiteSpace($Object))) {
        if ($Prefix) { $Prefix }
        return
    }
    
    if ($Object -is [System.Management.Automation.PSCustomObject]) {
        foreach ($prop in $Object.PSObject.Properties) {
            $path = if ($Prefix) { "$Prefix.$($prop.Name)" } else { $prop.Name }
            Get-JsonLeafPaths -Object $prop.Value -Prefix $path
        }
    }
    elseif ($Object -is [System.Collections.IEnumerable] -and $Object -isnot [string]) {
        # Handle empty arrays
        if (@($Object).Count -eq 0) {
            if ($Prefix) { $Prefix }
            return
        }
        foreach ($item in $Object) {
            Get-JsonLeafPaths -Object $item -Prefix $Prefix
        }
    }
    else {
        if ($Prefix) { $Prefix }
    }
}

# Usage
$json = Get-Content "person.json" | ConvertFrom-Json
Get-JsonLeafPaths -Object $json | Sort-Object -Unique | Set-Content "output.txt" -Encoding utf8

# Specify UTF-8 encoding
# Get-JsonLeafPaths -Object $json | Sort-Object -Unique | Out-File "output.txt" -Encoding UTF8
