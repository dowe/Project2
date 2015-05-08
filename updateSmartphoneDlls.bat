rem Gets the dll dependencies of the Smartphone.Driver project from the Project2 sln.
rem It would be much cleaner with Visual Studio integration of Xamarin.
set LIB_DRIVER=Smartphone\Smartphone.Driver\lib
robocopy Common.Communication.Client\bin\Debug\ %LIB_DRIVER%
robocopy Common.Commands\bin\Debug\ %LIB_DRIVER%