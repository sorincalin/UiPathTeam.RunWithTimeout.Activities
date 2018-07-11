# UiPathTeam.RunWithTimeout.Activities

<b>Summary</b>

Pack of currently 2 activities:

Run Process With Timeout - allows starting processes and waiting for them to exit using the System.Diagnostic.Process class.
Run Script With Timeout - allows starting scripts and waiting for them to exit using Windows Script Host (cscript.exe) application.

<b>Benefits</b>

Allows waiting for retrieving the command line output of a process or script as well as waiting for it to finish before continuing.

<b>Package specifications</b>

The only difference between the two activities is that Run Script With Timeout utilizes the Windows Scripting Host (cscript.exe) to run the script specified in the filename. Both of them use the System.Diagnostic.Process class. They have the following arguments:

InArguments
* string Filename (required)
* string Working directory
* string Arguments
* bool Capture output (required) - Indicates if the activity should capture the ouput of the process. If this is set to true, the process will be killed when the timeout is exceeded.
* bool Wait for exit flag (required) - Indicates if the activity should wait for the process to finish.
* int Wait for exit timeout in ms - Number of miliseconds to wait for the process to finish.\
* bool Kill at timeout - Indicates if the process should be killed after the wait timeout is over. If Capture ouput option is selected, it will default to true.

OutArguments
* string Standard output - Standard output stream content of the process.
* string Standard error - Standard error stream content of the process.
* int Process Id - Id of the started process.
* bool Finished - Indicates it the process has finished in the allocated time.
* int Exit code - Exit Code of the process. If the process did not finish in the allocated time will be set to Int.MinValue.

<b>Dependencies</b>

Windows Script Host has to be enabled.
