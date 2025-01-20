Changes to the code:

1. Added exception handling to Program.cs so that runtime/logical errors can be avoided. The error message shows the correct format that the user needs to input after the command.
This means that the user will know for sure what to input and the program will have a failsafe.

2. Summary is now built with StringBuilder instead. StringBuilder is much faster than concaternating many strings. Also used AppendLine() and '\t' instead of have readonly string variables solely for newlines and
indentations; both of which seemed unnecessary.

3. String building has been separated into a different function. This is due to the fact that I wanted the GetSummary() function to be concise in returning the summary, and the actual building would be done in
BuildSummary(). If in the future new things had to be added for the summary for new features, we can just tweak the BuildSummary() function and minimal change would need to be made to the GetSummary() function.
It also overall improves readability of the ScheduledRoutes.cs
