# TextCopier
This application allows you to store and copy pieces of text to the clipboard from a list. I am using WPF for the UI.

## Origin
I was searching different places in documentation at work to copy and paste text into emails I was sending. I thought it would be better to store all text in one spot and allow it to easily be copied.

## Installation Instructions
None yet.

## How to Use
### To Create an Item
1. Type or paste text into the text boxes below the Description and Text To Copy columns.
2. Click the Create button.
### To Update an Item
1. Select the item to update from the list.
2. Edit the text that appears in the text boxes below the list.
3. Click the Update button.
### To Delete an Item
1. Select the item to delete from the list.
2. Click the Delete button.
### To Copy an Item
* Click the Copy button next to the item you'd like to copy.
### To Clear the Text Boxes
* Click the Clear button.
### To Move an Item Up and Down
* Use the Move Up and Move Down buttons.
### To Sort the List
* Click the "Sort A -> Z" button.

## Screenshots
* [Main Window](Screenshots/main-window.png)

## Roadmap
### Features Implemented
* Users can view a DataGrid of text to copy, with buttons to copy text to the clipboard.
* Users can create, read, update, and delete items using text file data access. This uses CSV files, so text cannot currently contain commas.
* Users can clear the selected item in the DataGrid and the text boxes.
* Users can move items in the UI and database up and down (with buttons).
* SQLite data access.

### To Do
* Sort the items in the UI and database? from A to Z. This feature broke, after we switched from text files to SQLite.
* (Longer Term): Integrate into a larger application, such as an IT Service Desk app, with features like:
    * A notepad that automatically saves what you type
    * An app that keeps a store of quick reference documentation, editable by some and read-only by others, in a multi-user environment

## Contributions
If you want to contribute, please open an issue. If you see something you want to submit a pull request for, you may:
1. Fork the repo
2. Create a new branch
3. Write your code
4. Commit & Push to your branch
5. Create a pull request
