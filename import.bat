ROBOCOPY ..\Annihilation\Game .\Game /MIR
ROBOCOPY ..\Annihilation\LiteLauncher .\LiteLauncher /MIR
ROBOCOPY ..\Annihilation\story .\story /MIR
ROBOCOPY ..\Annihilation\doc .\doc /MIR
COPY     ..\Annihilation\AGENTS.md .

TIMEOUT 2
