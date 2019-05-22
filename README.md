# Physics-AI-Game
The game created during the Game Behaviour module, Year 3 Derby University Computer Games Programming course.


--------------------------- INSTRUCTIONS ON RUNNING THE GAME ---------------------------

1) Open the folder called "Executable" and run the file called "PhysicsAIGame.exe"

----------------------------------- GAME CONTROLS --------------------------------------

F1 - Toggle full screen
Escape - Close the game

A - Move left
D - Move right
S - Cancel all horizontal movement
Space - Jump
Left Mouse Button - Fire small projectile at the cursor position
Right Mouse Button - Fire larger projectile that can bounce at the cursor position

1 - Set the maximum enemies to 1
2 - Set the maximum enemies to 2
3 - Set the maximum enemies to 3
4 - Set the maximum enemies to 4
5 - Set the maximum enemies to 5

T - Toggles the use of the following debug commands:

Middle Mouse Button - Spawn an enemy at the cursor position
C - Toggle collision between characters
G - Draw a visualisation of the node grid
I - Toggle the AI behaviour
M - Toggle whether or not the AI can be 'killed' by projectiles
O - Draw the connection node graph used by the AI to navigate
P - Draw the path through the nodes taken by the first AI

-------- INSTRUCTIONS FOR BUILDING THE SOLUTION ON A PC WITH MONOGAME INSTALLED --------

1) Copy the source code to your PC
2) Open the solution named "PhysicsAIGame.sln" in the "PhysicsAIGame" folder.
3) Build the project

------ INSTRUCTIONS FOR BUILDING THE SOLUTION ON A PC WITHOUT MONOGAME INSTALLED -------

1) Download the MonoGame 3.5.1 source code
2) Put the folder named "PhysicsAIGame" (inside the "Source" folder) in a location with the MonoGame source code folder
2) Copy the contents of the provided folder named "Extra" to the MonoGame source code folder named "MonoGame.Framework.Content.Pipeline"
3) Open the solution named "PhysicsAIGame.sln" in the "PhysicsAIGame" folder. The "General" project will fail to open.
4) Right click on the .csproj file in the explorer and edit it
5) 		Change the line that says this:   	<Import Project="$(MSBuildExtensionsPath)\MonoGame\v3.0\MonoGame.Content.Builder.targets" />
		To this: 							<Import Project="..\..\MonoGame-3.5.1\MonoGame.Framework.Content.Pipeline\MonoGame.Content.Builder.targets" />
		
6) Right click the .csproj file again and reload it
7) Build the project