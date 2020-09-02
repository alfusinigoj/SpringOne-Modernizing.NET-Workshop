Start-Process powershell -ArgumentList "-NoExit -Command ""cd c:\users\WorkshopStudent\src\Workshop"""

Start-Process chrome -ArgumentList "https://docs-dev.steeltoe.io/session.html?redirect=/labs/modernize-dotnet"

devenv c:\users\WorkshopStudent\src\Workshop\Workshop.sln
code c:\users\WorkshopStudent\src\Workshop\
