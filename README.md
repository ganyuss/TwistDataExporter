
# Twist Data Exporter

## Introduction

This program is made to export data from twist Workspaces into a generic JSON file. It relies on the [Twist API v3](https://developer.twist.com/v3/#introduction). It has been created as a response to Twist not having a proper export tool yet.

## how to use it

To use this program. You must run it with the first parameter being a OAuth token from a Twist integration. If you do not know how to get one, see the "How to create a custom integration" pdf documentation.

In the output folder called "export", the data will be divided by workspace. Each workspace folder will contain a "data.json" file, and an "attachments" folder. The data.json file will contain the workspace information, with the threads, the comments etc. Attachments are saved with their file IDs as file names. Their original file names can still be found in the data.json file.

Note: this software does not export all the information. for example at this point in time, the user's avatar URL addresses are not exported in the JSON file. To select what is exported, see the ExportModel/Export.cs file.

## Known issues

Some attachments are not downloaded properly, and cannot be read. 

## License

Feel free to use this piece of code however you like, I mainly translated the Twist API documentation into a C# project. 