// Author: Jinwen Lei, Xing Liu, Fall 2021
// University of Utah

Features I wish the graders to be aware of:
Once you enter an invalid hostname, a message box will pop up and allow you to retry.
And if you press the cancel button, another message box will pop up with the error message.
Just press cancel again, and then you can try to enter the correct hostname and connect again.

For controls, we wanted to use a FILO data structure for our key handlers, but we failed to do that.
Eventually, we used if and else if statements.

The most interesting part of the assignment should be the animations,
because we spent a lot of time making it fluently.
It was once glitched. Whenever a player disconnects, it eitherloopsp the explosion animation,
or the last frame remains on the screen forever.

Player controls are the same as the example client, and we have a "Help" button for this info.

PS9:
We added the second mode for players. When players pick up the power-up, they get a speed boost 
for several seconds, which can help them chase or flee from their opponents. They do not need to 
change their code to play the second mode. We could change the setting stored on the server-side 
to select the second mode.
For convenience, we stored most of the game parameters into the setting file, including world 
size, MSPerFrame, FramesPerShot, RespawnRate, MaxHitPoint of the tank, EnginePower(speed) of the 
tank, SuperEnginePower(speed in the second mode) of the tank, MaxNumOfPowers, MaxPowerDelay, and 
SpeedMode(for selecting the second mode). 

The challenging part of the assignment was deciding where to lock and the keyword for locking. 
Also, dealing with client-tank is hard. For example, when a client socket gets abnormal, we want 
to remove the socket. However, we need to mark the tank as disconnected as well. We spent a lot 
of time figuring out a solution. Luckily, our code worked well after running for a long time.
Picking a random available position for objects is also challenging because a lot of math is 
involved. After careful consideration, we decided to pick a <int, int> position instead of 
<double, double>. Moreover, our code does not check tank-tank collisions.
