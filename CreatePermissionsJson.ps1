Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# Enable TLS1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor [System.Net.SecurityProtocolType]::Tls12

#region functions

function Get-TopdeskData {
    param(
        [string]$Endpoint   # e.g. 'applicableChangeTemplates', 'changes/benefits'
    )
    if (-not $global:TopdeskApiUrl -or -not $global:TopdeskCred) {
        throw "TopdeskApiUrl or TopdeskCred not set"
    }
    $uri = "$($global:TopdeskApiUrl.TrimEnd('/'))/tas/api/$Endpoint"
    return Invoke-RestMethod -Uri $uri -Method Get -Credential $global:TopdeskCred -ErrorAction Stop
}

function Set-AuthorizationHeaders {
    param (
        [ValidateNotNullOrEmpty()]
        [string]
        $Username,

        [ValidateNotNullOrEmpty()]
        [string]
        $ApiKey
    )
    # Create basic authentication string
    $bytes = [System.Text.Encoding]::ASCII.GetBytes("${Username}:${Apikey}")
    $base64 = [System.Convert]::ToBase64String($bytes)

    # Set authentication headers
    $authHeaders = [System.Collections.Generic.Dictionary[string, string]]::new()
    $authHeaders.Add("Authorization", "BASIC $base64")
    $authHeaders.Add('Accept', 'application/json; charset=utf-8')

    return $authHeaders
}

function Invoke-TopdeskRestMethod {
    param (
        [ValidateNotNullOrEmpty()]
        [string]
        $Method,

        [ValidateNotNullOrEmpty()]
        [string]
        $Uri,

        [object]
        $Body,

        [string]
        $ContentType = 'application/json; charset=utf-8',

        [System.Collections.IDictionary]
        $Headers
    )
    process {
        try {
            $splatParams = @{
                Uri         = $Uri
                Headers     = $Headers
                Method      = $Method
                ContentType = $ContentType
            }

            if ($Body) {
                $splatParams['Body'] = [Text.Encoding]::UTF8.GetBytes($Body)
            }
            Invoke-RestMethod @splatParams -Verbose:$false
        }
        catch {
            throw $_
        }
    }
}

function Get-TopdeskTemplateById {
    param (
        [ValidateNotNullOrEmpty()]
        [string]
        $BaseUrl,

        [System.Collections.IDictionary]
        $Headers,

        [ValidateNotNullOrEmpty()]
        [String]
        $Id
    )

    $splatParams = @{
        Uri     = "$baseUrl/tas/api/applicableChangeTemplates"
        Method  = 'GET'
        Headers = $Headers
    }
    $responseGet = Invoke-TopdeskRestMethod @splatParams

    $topdeskTemplate = $responseGet.results | Where-Object { ($_.number -eq $Id) }

    if ([string]::IsNullOrEmpty($topdeskTemplate)) {
        $errorMessage = "Topdesk template [$Id] not found. Please verify this template exists and it's available for the API in Topdesk."
        $outputContext.AuditLogs.Add([PSCustomObject]@{
                Message = $errorMessage
                IsError = $true
            })
        return
    }

    Write-Output $topdeskTemplate.id
}

function Escape-JsonString {
    param ([string]$input)
    
    if ([string]::IsNullOrWhiteSpace($input)) { return "" }
    $escaped = [string]$input

    # 1) newlines → \r\n
    $escaped = $escaped -replace "(`r`n|`n|`r)", "\r\n"

    # 2) default‐value mustache: {{ Person.Foo.Bar || "IfEmpty" }}
    $escaped = $escaped -replace '\{\{\s*Person\.([\w\.]+)\s*\|\|\s*"([^"]*)"\s*\}\}', '$($p.$1 -or "$2")'

    # 3) simple mustache: {{ Person.Foo.Bar }}
    $escaped = $escaped -replace '\{\{\s*Person\.([\w\.]+)\s*\}\}', '$($p.$1)'

    # 4) wrap any remaining $p.xxx (not already in $())
    $escaped = $escaped -replace '(?<!\$\()\$p\.[\w\.]+', '$($&)'

    # 5) wrap any remaining $p.xxx (not already in $())
    $escaped = $escaped -replace '(?<!\$\()\$account\.[\w\.]+', '$($&)'

    return $escaped
}

function Normalize-InputText {
    param ([string]$text)
    
    if ([string]::IsNullOrWhiteSpace($text)) { return "" }

    # 1) Turn any <br> or <br/> (case-insensitive) into CRLF:
    $text = $text -replace '<br\s*/?>', "`r`n"

    # 2) Convert any remaining lone LF (or existing CRLF) into a single CRLF.
    #    This ensures that any "\n" becomes "`r`n".
    $text = $text -replace '\r?\n', "`r`n"

    return  $text
}

function Create-Form {
    $form = New-Object System.Windows.Forms.Form
    $form.Text = "Topdesk JSON Creator"
    $form.Size = New-Object System.Drawing.Size(1000, 700)
    $form.StartPosition = "CenterScreen"
    $form.MinimumSize = $form.Size

    $loadButton = New-Object System.Windows.Forms.Button
    $loadButton.Text = "Load Template"
    $loadButton.Location = New-Object System.Drawing.Point(20, 16)
    $loadButton.Size = New-Object System.Drawing.Size(100, 30)

    # right after $loadButton in Create-Form
    $templateLabel = New-Object System.Windows.Forms.Label
    $templateLabel.Text = 'Template:'
    $templateLabel.Location = New-Object System.Drawing.Point(140, 25)
    $templateLabel.AutoSize = $true

    $templateDropdown = New-Object System.Windows.Forms.ComboBox
    $templateDropdown.Location = New-Object System.Drawing.Point(200, 23)
    $templateDropdown.Size = New-Object System.Drawing.Size(200, 20)
    $templateDropdown.DropDownStyle = 'DropDownList'

    $idLabel = New-Object System.Windows.Forms.Label
    $idLabel.Text = "Identification ID:"
    $idLabel.Location = New-Object System.Drawing.Point(20, 65)
    $idLabel.AutoSize = $true
    # $idLabel.BorderStyle = 'FixedSingle'

    $idBox = New-Object System.Windows.Forms.TextBox
    $idBox.Location = New-Object System.Drawing.Point(110, 63)
    $idBox.Size = New-Object System.Drawing.Size(100, 20)

    $nameLabel = New-Object System.Windows.Forms.Label
    $nameLabel.Text = "DisplayName:"
    $nameLabel.Location = New-Object System.Drawing.Point(($idBox.Right + 15), 65)
    $nameLabel.AutoSize = $true
    # $nameLabel.BorderStyle = 'FixedSingle'

    $nameBox = New-Object System.Windows.Forms.TextBox
    $nameBox.Location = New-Object System.Drawing.Point(305, 63)
    $nameBox.Size = New-Object System.Drawing.Size(200, 20)
    # $nameBox.text = "$($nameLabel.Width) $($nameLabel.Right)"

    $modeLabel = New-Object System.Windows.Forms.Label
    $modeLabel.Text = "Mode:"
    $modeLabel.Location = New-Object System.Drawing.Point(($nameBox.Right + 15), 65)
    $modeLabel.AutoSize = $true

    $modeDropdown = New-Object System.Windows.Forms.ComboBox
    $modeDropdown.Location = New-Object System.Drawing.Point(560, 63)
    $modeDropdown.Size = New-Object System.Drawing.Size(80, 20)
    $modeDropdown.Items.Add("Grant")
    $modeDropdown.Items.Add("Revoke")
    $modeDropdown.SelectedIndex = 0

    # — “New Incident” button —
    $newIncButton = New-Object System.Windows.Forms.Button
    $newIncButton.Text = "New Incident"
    $newIncButton.Location = New-Object System.Drawing.Point(720, 55)
    $newIncButton.Size = New-Object System.Drawing.Size(120, 30)
    $newIncButton.Anchor = 'Top, right'
    # $form.Controls.Add($newIncButton)
    #  $global:gui.NewIncidentButton = $newIncButton

    # — “New Change” button —
    $newChgButton = New-Object System.Windows.Forms.Button
    $newChgButton.Text = "New Change"
    $newChgButton.Location = New-Object System.Drawing.Point(850, 55)
    $newChgButton.Size = New-Object System.Drawing.Size(120, 30)
    $newChgButton.Anchor = 'Top, Right'
    # $form.Controls.Add($newChgButton)
    # $global:gui.NewChangeButton = $newChgButton

    $fieldPanel = New-Object System.Windows.Forms.Panel
    $fieldPanel.Location = New-Object System.Drawing.Point(20, 90)
    $fieldPanel.Size = New-Object System.Drawing.Size(950, 420)
    $fieldPanel.Anchor = 'Top, Left, Right, Bottom'
    $fieldPanel.BorderStyle = 'FixedSingle'
    # $fieldPanel.AutoSize = $true
    # $fieldPanel.AutoSizeMode =  0
    # $fieldPanel.Padding = 5
    # $form.Controls.Add($fieldPanel)

    $addButton = New-Object System.Windows.Forms.Button
    $addButton.Text = "Add"
    $addButton.Location = New-Object System.Drawing.Point(20, 530)
    $addButton.Size = New-Object System.Drawing.Size(100, 30)
    $addButton.Anchor = 'Bottom, Left'

    $listbox = New-Object System.Windows.Forms.ListBox
    $listbox.Location = New-Object System.Drawing.Point(130, 530)
    $listbox.Size = New-Object System.Drawing.Size(650, 120)
    $listbox.Anchor = 'Bottom, Left, Right'

    $exportButton = New-Object System.Windows.Forms.Button
    $exportButton.Text = "Export JSON"
    $exportButton.Location = New-Object System.Drawing.Point(800, 530)
    $exportButton.Size = New-Object System.Drawing.Size(120, 30)
    $exportButton.Anchor = 'Bottom, Right'

    $deleteButton = New-Object System.Windows.Forms.Button
    $deleteButton.Text = "Delete"
    $deleteButton.Location = New-Object System.Drawing.Point(20, 570)
    $deleteButton.Size = New-Object System.Drawing.Size(100, 30)
    $deleteButton.Anchor = 'Bottom,  Left'

    $editButton = New-Object System.Windows.Forms.Button
    $editButton.Text = "Edit"
    $editButton.Location = New-Object System.Drawing.Point(20, 610)
    $editButton.Size = New-Object System.Drawing.Size(100, 30)
    $editButton.Anchor = 'Bottom,  Left'

    $form.Controls.AddRange(@(
            $loadButton, 
            $idLabel, 
            $idBox, 
            $nameLabel, 
            $nameBox, 
            $modeLabel, 
            $modeDropdown,
            $fieldPanel, 
            $addButton, 
            $listbox, 
            $exportButton, 
            $deleteButton, 
            $editButton, 
            $templateLabel, 
            $templateDropdown, 
            $newIncButton, 
            $newChgButton
        ))

    return @{ 
        Form             = $form; 
        LoadButton       = $loadButton; 
        IdBox            = $idBox; 
        NameBox          = $nameBox; 
        ModeDropdown     = $modeDropdown; 
        FieldPanel       = $fieldPanel; 
        FieldControls    = @{}; 
        AddButton        = $addButton; 
        ListBox          = $listbox; 
        ExportButton     = $exportButton; 
        deleteButton     = $deleteButton; 
        editButton       = $editButton; 
        templateLabel    = $templateLabel; 
        templateDropdown = $templateDropdown; 
        newIncButton     = $newIncButton; 
        newChgButton     = $newChgButton 
    }

}

function Create-FieldControls($template) {
    $global:gui.FieldPanel.Controls.Clear()
    $global:gui.FieldControls.Clear()

    $y = 5

    foreach ($property in $template.PSObject.Properties) {
        $label = New-Object System.Windows.Forms.Label
        $label.Text = $property.Name
        $label.Location = New-Object System.Drawing.Point(0, $y)
        $label.Size = New-Object System.Drawing.Size(120, 20)
        $label.AutoSize = $true
        $global:gui.FieldPanel.Controls.Add($label)

        $box = New-Object System.Windows.Forms.TextBox
        $box.Multiline = $property.Value -is [string] -and $property.Value.Length -gt 80
        $box.ScrollBars = if ($box.Multiline) { 'Vertical' } else { 'None' }
        $box.Location = New-Object System.Drawing.Point(130, $y)
        $box.Size = New-Object System.Drawing.Size(800, $(if ($box.Multiline) { 60 } else { 20 }))
        $box.Text = Normalize-InputText -text $property.Value
        $box.Font = [Drawing.Font]::new("consolas", 9)
        $box.Name = $property.Name

        # Write-Warning  "$($property.Name) $($property.Value)"
        
        $box.Add_DoubleClick({

                $global:nameTextbox = $this.Name
                $popup = New-Object System.Windows.Forms.Form
                $popup.Text = "Edit Field - $($this.Name)"
                $popup.Size = New-Object System.Drawing.Size(800, 600)
                $popup.StartPosition = 'CenterParent'

                $largeBox = New-Object System.Windows.Forms.TextBox
                $largeBox.Multiline = $true
                $largeBox.ScrollBars = 'Both'
                $largeBox.Dock = 'Top'
                $largeBox.WordWrap = $false
                $largeBox.Size = New-Object System.Drawing.Size(800, 520)
                $largeBox.Anchor = 'Top, Left, Bottom, Right'
                $largeBox.Font = [Drawing.Font]::new("consolas", 11)
                $largeBox.Text = $this.Text
                $largeBox.SelectionStart = $largeBox.Text.Length
                $largeBox.SelectionLength = 0

                $okButton = New-Object System.Windows.Forms.Button
                $okButton.Text = 'OK'
                $okButton.Width = 80
                $okButton.Anchor = 'Bottom, Right'
                $okButton.Location = New-Object System.Drawing.Point(580, 530)

                $cancelButton = New-Object System.Windows.Forms.Button
                $cancelButton.Text = 'Cancel'
                $cancelButton.Width = 80
                $cancelButton.Anchor = 'Bottom, Right'
                $cancelButton.Location = New-Object System.Drawing.Point(680, 530)
                $cancelButton.Add_Click({ $popup.Close() })

                $okButton.Add_Click({
                        # Write-Warning "nameTextbox=$($global:nameTextbox) formObject=$($global:gui.Form.Text)"
                        $targetTextBox = $global:gui.Form.Controls.Find($global:nameTextbox, $true)
                        $targetTextBox[0].Text = $largeBox.Text
                        $popup.Close()
                    })

                $popup.Controls.Add($largeBox)
                $popup.Controls.Add($cancelButton)
                $popup.Controls.Add($okButton)
                $popup.ShowDialog()
            })

        $global:gui.FieldPanel.Controls.Add($box)
        $global:gui.FieldControls[$property.Name] = $box

        $y += $box.Height + 10

    }
    
    $bottomPadding = 180
    $newFormHeight = $global:gui.FieldPanel.Top + $y + $bottomPadding
    # Write-Warning "$($global:gui.FieldPanel.Top) $($y) $($bottomPadding)"
    $global:gui.Form.Height = $newFormHeight
    
    $global:gui.FieldPanel.Height = $box.Top + $box.Height + 10
    # $global:gui.Form.MinimumSize = $global:gui.Form.Size
    # if ($global:gui.FieldPanel.Height -le $box.Top +  $box.Height ) {
    #     Write-Warning "$($global:gui.FieldPanel.Height) $($box.Top +  $box.Height)"
    #     $global:gui.FieldPanel.Height = $box.Top +  $box.Height + 7
    # }
}

# $global:entries = @()
$global:entries = [System.Collections.ArrayList]::new()
$global:templates = @()
$global:nameTextbox = ''

$global:headers = ""

# ----------------------------
# 1a) Empty “Change” skeleton
# ----------------------------
$global:EmptyChange = [PSCustomObject]@{
    Request           = ""
    Requester         = ""
    Action            = $null
    BriefDescription  = ""
    Template          = ""
    Category          = ""
    SubCategory       = ""
    ChangeType        = ""
    Impact            = ""
    Benefit           = $null
    Priority          = ""
    EnableGetAssets   = $true
    SkipNoAssetsFound = $false
    AssetsFilter      = ""
}

# ----------------------------
# 1b) Empty “Incident” skeleton
# ----------------------------
$global:EmptyIncident = [PSCustomObject]@{
    Caller             = ""
    RequestShort       = ""
    RequestDescription = ""
    Action             = ""
    Branch             = ""
    OperatorGroup      = ""
    Operator           = $null
    Category           = ""
    SubCategory        = ""
    CallType           = ""
    Impact             = $null
    Priority           = $null
    Duration           = $null
    EntryType          = $null
    Urgency            = $null
    ProcessingStatus   = $null
    EnableGetAssets    = $true
    SkipNoAssetsFound  = $false
    AssetsFilter       = ""
}

# $global:tempBoxes = [System.Collections.Generic.List[Object]]::new()

$global:gui = Create-Form

$global:gui.LoadButton.Add_Click({
        $dlg = New-Object System.Windows.Forms.OpenFileDialog
        $dlg.Filter = "JSON Files (*.json)|*.json"
        $dlg.Title = "Load Template JSON"
        if ($dlg.ShowDialog() -eq 'OK') {
            $global:templates = Get-Content -Path $dlg.FileName -raw | ConvertFrom-Json

            # clear & fill the combo
            $global:gui.TemplateDropdown.Items.Clear()
            for ($i = 0; $i -lt $global:templates.Count; $i++) {
                $t = $global:templates[$i]
                # handle both Id and id
                $id = $t.Identification.Id
                if (-not $id) { $id = $t.Identification.id }
                $global:gui.TemplateDropdown.Items.Add("[$id] $($t.DisplayName)")
            }

            if ($global:templates.Count -gt 0) {
                $global:gui.TemplateDropdown.SelectedIndex = 0
            }
        }
    })

$global:gui.TemplateDropdown.Add_SelectedIndexChanged({
        $idx = $global:gui.TemplateDropdown.SelectedIndex
        if ($idx -ge 0) {
            $t = $global:templates[$idx]

            # again, both Id/id
            $id = $t.Identification.Id
            if (-not $id) { $id = $t.Identification.id }

            $global:gui.IdBox.Text = $id
            $global:gui.NameBox.Text = $t.DisplayName

            $mode = $global:gui.ModeDropdown.SelectedItem
            $data = $t.$mode

            # Write-Warning ( $data | ConvertTo-Json -Depth 10)
            # Write-Warning $data.gettype()

            Create-FieldControls $data
        }
    })

# When the user switches between Grant/Revoke, reload fields for the current template
$global:gui.ModeDropdown.Add_SelectedIndexChanged({
        $tplIdx = $global:gui.TemplateDropdown.SelectedIndex
        if ($tplIdx -ge 0) {
            $template = $global:templates[$tplIdx]

            # get the correct mode
            $mode = $global:gui.ModeDropdown.SelectedItem

            # grab that branch (Grant or Revoke) and rebuild the panel
            $data = $template.$mode
            if ($data) {
                Create-FieldControls $data
            }
            else {
                # if one side is empty, clear the panel
                $global:gui.FieldPanel.Controls.Clear()
                $global:gui.FieldControls.Clear()
            }
        }
    })

# — Handler for “New Change” —
$global:gui.newChgButton.Add_Click({
        # 1) Clear out Identification + DisplayName + Mode
        $global:gui.IdBox.Text = ""
        $global:gui.NameBox.Text = ""
        $global:gui.ModeDropdown.SelectedItem = "Grant"  # or whichever default makes sense

        # 2) Re‐draw the field panel using those blank‐default fields
        Create-FieldControls $global:EmptyChange
    })

# — Handler for “New Incident” —
$global:gui.newIncButton.Add_Click({
        # 1) Clear Identification + DisplayName + Mode (if you want those reset)
        $global:gui.IdBox.Text = ""
        $global:gui.NameBox.Text = ""
        $global:gui.ModeDropdown.SelectedItem = "Grant"  # or whichever default is appropriate

        # 2) Re‐draw the field panel using those blank‐default incident fields
        Create-FieldControls $global:EmptyIncident
    })


$global:gui.AddButton.Add_Click({
        $id = $global:gui.IdBox.Text
        $name = $global:gui.NameBox.Text
        $mode = $global:gui.ModeDropdown.SelectedItem

        if (-not ($id -and $name -and $mode)) { return }

        # gather the values from the dynamic fields
        $section = @{}
        foreach ($key in $global:gui.FieldControls.Keys) {
            $section[$key] = $global:gui.FieldControls[$key].Text
        }

        $existing = $global:entries | Where-Object { $_.Identification.Id -eq $id }

        if ($existing) {
            # already have a Grant/Revoke for this ID?
            if ($existing.$mode.Count -gt 0) {
                [System.Windows.Forms.MessageBox]::Show(
                    "A $mode entry for ID [$id] already exists.`nYou can only add one $mode per ID.",
                    "Duplicate Entry",
                    [System.Windows.Forms.MessageBoxButtons]::OK,
                    [System.Windows.Forms.MessageBoxIcon]::Warning
                )
                return
            }
            # just fill in the missing branch
            $existing.DisplayName = $name
            $existing.$mode = $section
        }
        else {
            # brand-new ID, initialize both branches then fill the chosen one
            $entry = @{
                Identification = @{ Id = $id }
                DisplayName    = $name
                Grant          = @{}
                Revoke         = @{}
            }
            $entry.$mode = $section
            # $global:entries += $entry
            [void]$global:entries.Add($entry)
        }

        # add one list-box line for this ID+mode
        $global:gui.ListBox.Items.Add("[$id] $name - $mode")
    })

$global:gui.editButton.Add_Click({
        $idx = $global:gui.ListBox.SelectedIndex
        if ($idx -lt 0) { return }
    
        $itemText = $global:gui.ListBox.Items[$idx]
        Write-Warning "1111"
        Write-Warning $itemText
        if ($itemText -notmatch '^\[(.+?)\].+ - (Grant|Revoke)$') { return }
    
        $id = $matches[1]
        $mode = $matches[2]
        Write-Warning "1111"
        # find the entry in $global:entries that has this ID
        $entry = $global:entries | Where-Object { $_.Identification.Id -eq $id }
        # Write-Warning ( $entry | ConvertTo-Json -Depth 10)
        if (-not $entry) { return }
        Write-Warning "2222"
        # populate the top‐line fields
        $global:gui.IdBox.Text = $entry.Identification.Id
        $global:gui.NameBox.Text = $entry.DisplayName
        $global:gui.ModeDropdown.SelectedItem = $mode
    
        # cast that branch’s hashtable into a PSCustomObject
        $templateObj = [pscustomobject]$entry.$mode
    
        # redraw fields for Grant/Revoke
        Create-FieldControls $templateObj
    
        # remove that one ListBox line, but leave the other branch intact
        $global:gui.ListBox.Items.RemoveAt($idx)
    
        # we only want to remove the branch we’re editing; 
        # if both branches are empty afterward, remove the entire entry
        $entry.$mode = @{} 
        if (($entry.Grant.Count -eq 0) -and ($entry.Revoke.Count -eq 0)) {
            $global:entries.Remove($entry) | Out-Null
        }
    })

# $global:gui.editButton.Add_Click({
#         $idx = $global:gui.ListBox.SelectedIndex
#         if ($idx -lt 0) { return }
    
#         # e.g. "[C001] Aanvraag… - Grant"
#         $itemText = $global:gui.ListBox.Items[$idx]
#         if ($itemText -notmatch '^\[(.+?)\].+ - (Grant|Revoke)$') { return }
    
#         $id = $matches[1]
#         $mode = $matches[2]
    
#         # find the entry object by Id
#         $entry = $global:entries | Where-Object { $_.Identification.Id -eq $id }
#         if (-not $entry) { return }
    
#         # set ID/Name/Mode on the form
#         $global:gui.IdBox.Text = $entry.Identification.Id
#         $global:gui.NameBox.Text = $entry.DisplayName
#         $global:gui.ModeDropdown.SelectedItem = $mode
    
#         # cast the hashtable branch to PSCustomObject
#         $template = [pscustomobject]$entry.$mode
    
#         # redraw the panel
#         Create-FieldControls $template
    
#         # remove the old entry so user can re-add after editing
#         $global:entries.Remove($entry) | Out-Null
#         $global:gui.ListBox.Items.RemoveAt($idx)
#     })


$global:gui.deleteButton.Add_Click({
        $idx = $global:gui.ListBox.SelectedIndex
        if ($idx -lt 0) { return }

        # e.g. "[C001] Aanvraag... - Grant"  
        $itemText = $global:gui.ListBox.Items[$idx]

        if ($itemText -match '^\[(.+?)\].+ - (Grant|Revoke)$') {
            $id = $matches[1]
            $mode = $matches[2]

            # find the entry object
            $entry = $global:entries | Where-Object { $_.Identification.Id -eq $id }
            if ($entry) {
                # clear only that branch
                $entry.$mode = @{}

                # if both Grant & Revoke are now empty, remove the entire entry
                $grantCount = $entry.Grant.Count
                $revokeCount = $entry.Revoke.Count
                # Write-Warning "grantCount=$grantCount revokeCount=$revokeCount" 
                # $grantCount  = $entry.Grant.PSObject.Properties.Names.Count
                # $revokeCount = $entry.Revoke.PSObject.Properties.Names.Count
                # Write-Warning "grantCount=$grantCount revokeCount=$revokeCount" 
                if ($grantCount -eq 0 -and $revokeCount -eq 0) {
                    [void]$global:entries.Remove($entry)
                }
            }
        }

        # finally remove the line from the UI list
        $global:gui.ListBox.Items.RemoveAt($idx)
    })

$global:gui.ExportButton.Add_Click({
        $saveDialog = New-Object System.Windows.Forms.SaveFileDialog
        $saveDialog.Filter = "JSON files (*.json)|*.json"
        $saveDialog.Title = "Save Topdesk File"

        if ($saveDialog.ShowDialog() -eq "OK") {
            $escapedEntries = $global:entries | ForEach-Object {
                $entry = $_  # outer entry (Grant/Revoke + Identification + DisplayName)
                # Write-Warning $($_.Identification | ConvertTo-Json -Depth 10)
                foreach ($mode in @("Grant", "Revoke")) {
                    if ($entry.$mode) {
                        foreach ($field in $entry.$mode.PSObject.Properties) {
                            $fieldName = $field.Name
                            $fieldValue = $field.Value
                            if ($fieldValue -is [string]) {
                                $entry.$mode[$fieldName] = Escape-JsonString -input $fieldValue
                            }
                        }
                    }
                }

                return $entry
            }

            $escapedEntries | ConvertTo-Json -Depth 10 | Out-File -Encoding UTF8 -FilePath $saveDialog.FileName

            [System.Windows.Forms.MessageBox]::Show(
                "Exported to $($saveDialog.FileName)",
                "Export Successful"
            )
        }
    })


[void]$global:gui.Form.ShowDialog()
