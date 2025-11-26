# TOPdesk Permissions Editor - Test Plan

## Document Information
- **Version**: 1.0
- **Last Updated**: 2025-01-22
- **Application**: TOPdesk Permissions Editor
- **Platform**: Windows WPF (.NET 8)

---

## 1. Test Scope

### 1.1 In Scope
- File operations (New, Open, Save, Save As, Close)
- CRUD operations for permissions (Add, Edit, Delete, Duplicate)
- UI functionality (tabs, forms, list, splitter)
- Data validation
- Text editor popup
- Auto-selection and tab switching

### 1.2 Out of Scope
- Keyboard shortcuts (not implemented)
- Unsaved changes prompts (not implemented)
- Performance testing
- Automated unit tests

---

## 2. Test Environment

### 2.1 Prerequisites
- Windows 10/11
- .NET 8 Runtime installed
- Test data files available in `TestData/` folder

### 2.2 Test Data
Required test files:
- `TestData/test_changes.json` - Valid change permissions file
- Create additional test files as needed for edge cases

---

## 3. Test Cases

## 3.1 File Operations

### TC-001: Create New Incident File
**Objective**: Verify creating a new incident file works correctly

**Steps**:
1. Launch application
2. Click **File → New → New Incident File** (or toolbar "New Incident" button)

**Expected Results**:
- ✓ Incident tab is selected and enabled
- ✓ Change tab is disabled (grayed out)
- ✓ One empty incident permission is automatically added to the list
- ✓ The new permission is selected
- ✓ Incident editor shows the new permission ready for editing
- ✓ Status bar shows "New incident file created"

**Test Data**: N/A

---

### TC-002: Create New Change File
**Objective**: Verify creating a new change file works correctly

**Steps**:
1. Launch application
2. Click **File → New → New Change File** (or toolbar "New Change" button)

**Expected Results**:
- ✓ Change tab is selected and enabled
- ✓ Incident tab is disabled (grayed out)
- ✓ One empty change permission is automatically added to the list
- ✓ The new permission is selected
- ✓ Change editor shows the new permission ready for editing
- ✓ Status bar shows "New change file created"

**Test Data**: N/A

---

### TC-003: Open Incident File
**Objective**: Verify opening an incident JSON file

**Steps**:
1. Launch application
2. Click **File → Open** (or toolbar "Open" button)
3. Navigate to `TestData/` folder
4. Select a valid incident.json file
5. Click **Open**

**Expected Results**:
- ✓ File loads successfully
- ✓ All permissions appear in the list on the left
- ✓ First permission is automatically selected
- ✓ Incident tab is selected and enabled
- ✓ Change tab is disabled (grayed out)
- ✓ Incident editor displays the first permission's data
- ✓ Status bar shows "Loaded X permission(s) from [filename]"

**Test Data**: Valid incident.json file

---

### TC-004: Open Change File
**Objective**: Verify opening a change JSON file

**Steps**:
1. Launch application
2. Click **File → Open**
3. Select `TestData/test_changes.json`
4. Click **Open**

**Expected Results**:
- ✓ File loads successfully
- ✓ All permissions appear in the list
- ✓ First permission is automatically selected
- ✓ Change tab is selected and enabled
- ✓ Incident tab is disabled (grayed out)
- ✓ Change editor displays the first permission's data
- ✓ Status bar shows "Loaded X permission(s) from test_changes.json"

**Test Data**: `TestData/test_changes.json`

---

### TC-005: Save File
**Objective**: Verify saving changes to the current file

**Steps**:
1. Open a file (incident or change)
2. Make changes to a permission (e.g., edit Display Name)
3. Click **File → Save** (or toolbar "Save" button)

**Expected Results**:
- ✓ File is saved to the original location
- ✓ Save button becomes disabled (no unsaved changes)
- ✓ Status bar shows "Saved X permission(s) to [filename]"
- ✓ Re-open the file and verify changes were saved

**Test Data**: Any valid JSON file

---

### TC-006: Save As
**Objective**: Verify saving file to a new location

**Steps**:
1. Open a file
2. Click **File → Save As**
3. Choose a new filename/location
4. Click **Save** in the dialog

**Expected Results**:
- ✓ File is saved to the new location
- ✓ Current file path updates to the new location
- ✓ Window title shows new filename
- ✓ Status bar shows "Saved to [new filename]"
- ✓ Original file remains unchanged
- ✓ New file contains all current data

**Test Data**: Any valid JSON file

---

### TC-007: Close File
**Objective**: Verify closing a file

**Steps**:
1. Open a file
2. Click **File → Close** (or toolbar "Close" button)

**Expected Results**:
- ✓ Permissions list is cleared
- ✓ Editor panels are cleared
- ✓ Both Incident and Change tabs become enabled
- ✓ Close button becomes disabled
- ✓ Status bar shows "File closed"
- ✓ Window title shows "TOPdesk Permissions Editor"

**Test Data**: Any valid JSON file

---

## 3.2 CRUD Operations

### TC-008: Add Incident Permission
**Objective**: Verify adding a new incident permission

**Prerequisites**: Have an incident file open or create new incident file

**Steps**:
1. Click **Edit → Add Incident Permission** (or toolbar "Add Incident" button)

**Expected Results**:
- ✓ New permission appears in the list
- ✓ New permission is automatically selected
- ✓ New permission has a unique ID (e.g., I001, I002)
- ✓ Incident editor shows the new permission
- ✓ All fields are empty except ID
- ✓ File is marked as having unsaved changes

**Test Data**: N/A

---

### TC-009: Add Change Permission
**Objective**: Verify adding a new change permission

**Prerequisites**: Have a change file open or create new change file

**Steps**:
1. Click **Edit → Add Change Permission** (or toolbar "Add Change" button)

**Expected Results**:
- ✓ New permission appears in the list
- ✓ New permission is automatically selected
- ✓ New permission has a unique ID (e.g., C001, C002)
- ✓ Change editor shows the new permission
- ✓ All fields are empty except ID
- ✓ File is marked as having unsaved changes

**Test Data**: N/A

---

### TC-010: Edit Permission - Display Name
**Objective**: Verify editing the Display Name field

**Prerequisites**: Have at least one permission loaded

**Steps**:
1. Select a permission from the list
2. In the editor, change the "Display Name" field
3. Type new text

**Expected Results**:
- ✓ Text updates in the textbox as you type
- ✓ The permission list item immediately updates to show the new Display Name
- ✓ File is marked as having unsaved changes

**Test Data**: Any loaded permission

---

### TC-011: Edit Permission - Long Text Fields
**Objective**: Verify editing long text fields (Request, Category, etc.)

**Prerequisites**: Have at least one permission loaded

**Steps**:
1. Select a permission from the list
2. Navigate to Grant or Revoke tab in the editor
3. Edit a multi-line field (e.g., Request)
4. Add text with multiple lines

**Expected Results**:
- ✓ Text updates as you type
- ✓ No word wrapping occurs (text extends horizontally)
- ✓ Horizontal scrollbar appears when text exceeds width
- ✓ Text uses Consolas 12pt font
- ✓ File is marked as having unsaved changes

**Test Data**: Any loaded permission

---

### TC-012: Delete Permission
**Objective**: Verify deleting a selected permission

**Prerequisites**: Have multiple permissions loaded

**Steps**:
1. Select a permission from the list
2. Click **Edit → Delete** (or toolbar "Delete" button)

**Expected Results**:
- ✓ Selected permission is removed from the list
- ✓ Next permission in the list is automatically selected (if available)
- ✓ Editor updates to show the newly selected permission
- ✓ If it was the last permission, editor is cleared
- ✓ File is marked as having unsaved changes

**Test Data**: File with multiple permissions

---

### TC-013: Duplicate Permission
**Objective**: Verify duplicating a selected permission

**Prerequisites**: Have at least one permission loaded

**Steps**:
1. Select a permission from the list
2. Click **Edit → Duplicate** (or toolbar "Duplicate" button)

**Expected Results**:
- ✓ A new permission appears in the list
- ✓ New permission has a unique ID (incremented)
- ✓ All other fields are copied from the original
- ✓ New permission is automatically selected
- ✓ Editor shows the duplicated permission
- ✓ File is marked as having unsaved changes

**Test Data**: Any loaded permission

---

### TC-014: Delete - No Selection
**Objective**: Verify Delete button is disabled when nothing is selected

**Prerequisites**: Have a file open

**Steps**:
1. Click in empty space to deselect any permission
2. Observe the Delete button

**Expected Results**:
- ✓ Delete button is disabled (grayed out)
- ✓ Cannot click Delete menu item or toolbar button

**Test Data**: Any loaded file

---

### TC-015: Duplicate - No Selection
**Objective**: Verify Duplicate button is disabled when nothing is selected

**Prerequisites**: Have a file open

**Steps**:
1. Click in empty space to deselect any permission
2. Observe the Duplicate button

**Expected Results**:
- ✓ Duplicate button is disabled (grayed out)
- ✓ Cannot click Duplicate menu item or toolbar button

**Test Data**: Any loaded file

---

## 3.3 UI Functionality

### TC-016: Tab Switching - Incident File
**Objective**: Verify tabs behave correctly with incident file

**Steps**:
1. Open an incident file or create new incident file
2. Try to click on the Change tab

**Expected Results**:
- ✓ Incident tab is active and enabled
- ✓ Change tab is grayed out and cannot be clicked
- ✓ Cannot switch to Change tab

**Test Data**: Any incident file

---

### TC-017: Tab Switching - Change File
**Objective**: Verify tabs behave correctly with change file

**Steps**:
1. Open a change file or create new change file
2. Try to click on the Incident tab

**Expected Results**:
- ✓ Change tab is active and enabled
- ✓ Incident tab is grayed out and cannot be clicked
- ✓ Cannot switch to Incident tab

**Test Data**: Any change file

---

### TC-018: Tab Switching - No File
**Objective**: Verify tabs are both enabled when no file is open

**Steps**:
1. Launch application (or close any open file)
2. Observe both tabs

**Expected Results**:
- ✓ Both Incident and Change tabs are enabled
- ✓ Can click either tab (though they'll be empty)

**Test Data**: N/A

---

### TC-019: GridSplitter Resize
**Objective**: Verify the splitter between permissions list and editor works

**Steps**:
1. Open any file
2. Hover mouse over the vertical gray bar between list and editor
3. Click and drag left/right

**Expected Results**:
- ✓ Cursor changes to resize cursor (↔)
- ✓ Permissions list width changes as you drag
- ✓ Editor panel width changes inversely
- ✓ List doesn't shrink below 200px
- ✓ Editor doesn't shrink below 400px
- ✓ Permissions list items stretch to fill the column

**Test Data**: Any file with permissions

---

### TC-020: Permissions List Display
**Objective**: Verify permissions list displays correctly

**Prerequisites**: Open file with multiple permissions

**Steps**:
1. Open a file with several permissions
2. Observe the list

**Expected Results**:
- ✓ Each permission shows Display Name in bold
- ✓ Each permission shows ID below Display Name in gray
- ✓ Selected item has highlighted background
- ✓ List items stretch full width to the splitter
- ✓ No gap between list and splitter

**Test Data**: File with multiple permissions

---

### TC-021: Grant/Revoke Tabs in Editor
**Objective**: Verify Grant and Revoke tabs work in the editor

**Prerequisites**: Have a permission selected

**Steps**:
1. Select a permission
2. Click on the "Grant" tab in the editor
3. Click on the "Revoke" tab in the editor

**Expected Results**:
- ✓ Grant tab shows Grant section fields
- ✓ Revoke tab shows Revoke section fields
- ✓ Can switch between tabs freely
- ✓ Data persists when switching tabs
- ✓ Both tabs are always enabled when a permission is selected

**Test Data**: Any permission

---

## 3.4 Text Editor Popup

### TC-022: Open Text Editor Popup
**Objective**: Verify double-clicking a TextBox opens the popup editor

**Prerequisites**: Have a permission selected

**Steps**:
1. Select a permission
2. Double-click on any TextBox field (e.g., Display Name, Request, Category)

**Expected Results**:
- ✓ Text Editor dialog opens
- ✓ Dialog is centered on screen
- ✓ Dialog title shows "Text Editor - [FieldName]"
- ✓ TextBox shows current field value
- ✓ Text is NOT selected/highlighted
- ✓ Cursor is in the TextBox
- ✓ Dialog is modal (blocks main window)

**Test Data**: Any permission

---

### TC-023: Text Editor - Edit and Save
**Objective**: Verify editing text in popup and saving

**Steps**:
1. Double-click a TextBox to open popup
2. Modify the text
3. Click **OK**

**Expected Results**:
- ✓ Dialog closes
- ✓ Main window TextBox updates with new text
- ✓ If field was Display Name, list item updates immediately
- ✓ File is marked as having unsaved changes

**Test Data**: Any permission

---

### TC-024: Text Editor - Cancel
**Objective**: Verify canceling popup discards changes

**Steps**:
1. Double-click a TextBox to open popup
2. Modify the text
3. Click **Cancel**

**Expected Results**:
- ✓ Dialog closes
- ✓ Main window TextBox remains unchanged
- ✓ Changes are discarded

**Test Data**: Any permission

---

### TC-025: Text Editor - Font and Wrapping
**Objective**: Verify text editor formatting

**Steps**:
1. Double-click a TextBox to open popup
2. Type or paste text with multiple lines

**Expected Results**:
- ✓ Text uses Consolas 12pt font
- ✓ No word wrapping (text extends horizontally)
- ✓ Horizontal scrollbar appears when needed
- ✓ Vertical scrollbar appears when needed
- ✓ Window is 700x900 pixels
- ✓ Can resize window (minimum 500x300)

**Test Data**: Any permission

---

## 3.5 Data Validation

### TC-026: Load Invalid JSON
**Objective**: Verify handling of malformed JSON files

**Steps**:
1. Create a file with invalid JSON syntax (missing brackets, quotes, etc.)
2. Try to open it

**Expected Results**:
- ✓ Error message appears
- ✓ Status bar shows error: "Error opening file: Invalid JSON format in file: [filename]"
- ✓ File does not load
- ✓ Application remains stable

**Test Data**: Create invalid JSON file

---

### TC-027: Load JSON - Wrong Structure
**Objective**: Verify handling of valid JSON with wrong structure

**Steps**:
1. Create a JSON file that's valid but not an array
2. Try to open it

**Expected Results**:
- ✓ File validation fails
- ✓ Appropriate error message shown
- ✓ File does not load

**Test Data**: `{"test": "value"}` (object instead of array)

---

### TC-028: Save - Empty Permissions List
**Objective**: Verify behavior when saving with no permissions

**Steps**:
1. Create new file
2. Delete all permissions
3. Try to save

**Expected Results**:
- ✓ File saves successfully
- ✓ Saved file contains empty array: `[]`
- ✓ Can re-open the empty file

**Test Data**: N/A

---

### TC-029: Auto-Select First Item on Load
**Objective**: Verify first permission is selected when file opens

**Steps**:
1. Open any file with multiple permissions

**Expected Results**:
- ✓ First permission in the list is automatically selected
- ✓ Editor shows the first permission's data
- ✓ User doesn't need to manually select it

**Test Data**: File with multiple permissions

---

### TC-030: Display Name Updates List
**Objective**: Verify editing Display Name updates the list in real-time

**Steps**:
1. Select a permission
2. Edit the Display Name field
3. Type new text character by character

**Expected Results**:
- ✓ List item updates in real-time as you type
- ✓ No delay between typing and list update
- ✓ Updates work for both Incident and Change permissions

**Test Data**: Any permission

---

## 3.6 Edge Cases

### TC-031: Open File - Empty Array
**Objective**: Verify opening a file with empty permissions array

**Steps**:
1. Create a JSON file: `[]`
2. Open it

**Expected Results**:
- ✓ File loads successfully
- ✓ Status bar shows "Loaded 0 permission(s) from [filename]"
- ✓ Permissions list is empty
- ✓ Editor is empty
- ✓ Add buttons are enabled

**Test Data**: `[]`

---

### TC-032: Maximum ID Generation
**Objective**: Verify ID generation with many permissions

**Steps**:
1. Open a file
2. Add 99+ permissions

**Expected Results**:
- ✓ IDs increment correctly (I001, I002, ... I100, I101...)
- ✓ No ID collisions
- ✓ All IDs are unique

**Test Data**: N/A

---

### TC-033: Special Characters in Fields
**Objective**: Verify handling of special characters

**Steps**:
1. Enter text with special characters: `<>&"'\n\r\t`
2. Save and reopen file

**Expected Results**:
- ✓ Special characters are properly escaped in JSON
- ✓ Characters display correctly when reopened
- ✓ No data corruption

**Test Data**: Any permission

---

### TC-034: Very Long Text
**Objective**: Verify handling of extremely long text fields

**Steps**:
1. Paste 10,000+ characters into a Request field
2. Save file

**Expected Results**:
- ✓ Text is accepted
- ✓ File saves successfully
- ✓ Horizontal scrollbar appears in TextBox
- ✓ Application remains responsive

**Test Data**: Large text block

---

### TC-035: Rapid Permission Selection
**Objective**: Verify stability when rapidly clicking different permissions

**Steps**:
1. Open file with many permissions
2. Rapidly click different permissions in the list

**Expected Results**:
- ✓ Editor updates correctly for each selection
- ✓ No lag or freezing
- ✓ No data corruption
- ✓ Application remains stable

**Test Data**: File with 10+ permissions

---

### TC-036: Window Resize
**Objective**: Verify application behaves correctly when resizing window

**Steps**:
1. Open application
2. Resize window to very small size (near minimum)
3. Resize window to very large size
4. Resize to normal size

**Expected Results**:
- ✓ Window respects minimum size (800x500)
- ✓ Layout adjusts correctly at all sizes
- ✓ Splitter position is maintained proportionally
- ✓ No UI elements are clipped or hidden
- ✓ Scrollbars appear when needed

**Test Data**: Any state

---

## 3.7 Workflow Tests

### TC-037: Complete Workflow - Create and Save Incident
**Objective**: Test complete workflow from creation to save

**Steps**:
1. Launch application
2. Click "New Incident"
3. Fill in ID: "TEST001", Display Name: "Test Permission"
4. Switch to Grant tab
5. Fill in: Caller: "test@test.com", Category: "Test", RequestShort: "Test Request"
6. Switch to Revoke tab
7. Fill in similar data
8. Click Save As
9. Save as "test_workflow.json"
10. Close application
11. Reopen application
12. Open "test_workflow.json"

**Expected Results**:
- ✓ File is created successfully
- ✓ All data is saved correctly
- ✓ File loads correctly on reopen
- ✓ All fields contain the correct data

**Test Data**: N/A

---

### TC-038: Complete Workflow - Edit and Duplicate
**Objective**: Test editing and duplicating workflow

**Steps**:
1. Open test_changes.json
2. Select first permission
3. Edit Display Name to "Modified Permission"
4. Click Duplicate
5. Edit the duplicate's Display Name to "Duplicated Permission"
6. Save file
7. Reopen file

**Expected Results**:
- ✓ Original permission shows "Modified Permission"
- ✓ Duplicate exists with "Duplicated Permission"
- ✓ Duplicate has all other fields copied from original
- ✓ Duplicate has unique ID
- ✓ Changes persist after save/reopen

**Test Data**: test_changes.json

---

### TC-039: Complete Workflow - Multiple Edits
**Objective**: Test making multiple edits before saving

**Steps**:
1. Open a file
2. Add 3 new permissions
3. Edit 2 existing permissions
4. Delete 1 permission
5. Duplicate 1 permission
6. Save file
7. Reopen file

**Expected Results**:
- ✓ All changes are saved correctly
- ✓ Permission count is correct
- ✓ All edits are persisted
- ✓ Deleted permission is gone
- ✓ Duplicated permission exists with correct data

**Test Data**: Any file

---

## 4. Test Execution Checklist

### Pre-Testing
- [ ] Build application in Release mode
- [ ] Verify .NET 8 runtime is installed
- [ ] Prepare test data files in TestData folder
- [ ] Create empty test results document

### During Testing
- [ ] Execute each test case in order
- [ ] Mark Pass/Fail for each test
- [ ] Take screenshots of any failures
- [ ] Document steps to reproduce any bugs
- [ ] Note any unexpected behavior

### Post-Testing
- [ ] Calculate pass/fail percentage
- [ ] Create bug reports for all failures
- [ ] Prioritize bugs (Critical/High/Medium/Low)
- [ ] Update test plan with any new test cases discovered
- [ ] Archive test results with version number

---

## 5. Bug Reporting Template

When logging bugs, include:

**Bug ID**: BUG-XXX
**Test Case**: TC-XXX
**Severity**: Critical / High / Medium / Low
**Summary**: Brief description

**Steps to Reproduce**:
1. Step 1
2. Step 2
3. Step 3

**Expected Result**: What should happen
**Actual Result**: What actually happened
**Screenshots**: Attach if applicable
**Environment**: Windows version, .NET version

---

## 6. Test Metrics

Track the following metrics:

- **Total Test Cases**: 39
- **Test Cases Passed**: ___
- **Test Cases Failed**: ___
- **Pass Rate**: ____%
- **Critical Bugs**: ___
- **High Priority Bugs**: ___
- **Medium Priority Bugs**: ___
- **Low Priority Bugs**: ___

---

## 7. Sign-Off

**Tester Name**: _________________
**Date**: _________________
**Signature**: _________________

**Approved By**: _________________
**Date**: _________________
**Signature**: _________________

---

## 8. Notes

Use this section for any additional observations, suggestions, or comments during testing.

---

**End of Test Plan**
