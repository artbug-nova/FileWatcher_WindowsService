set mypath=%cd%
sc create FileService binPath="%mypath%\bin\Debug\netcoreapp3.1\WindowsServiceWorker.exe"
sc description FileService "Directory file checking service"