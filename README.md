[0.24.2] Athlonic Electronics / LCD - Launch CountDown (Audio&Onscreen)
v1.7.2 (August 10/2014)


Bored of yelling the launch countdown yourself ?
Do you find the launch sequence a little ... lifeless ?

So here comes an enhancement for your launch clamps : an authentical launch countdown brought to you by Athlonic Electronics.

_________________________________________________________
Installation :

- You just have to install the "LaunchCountDown" folder and the "ModuleManager.dll" file in your KSP "GameData" folder.

It will upgrade your KSP stock TT18-A Launch Stability Enhancer (aka launch clamp) with a countdown feature.
No additional parts needed thanks to the excellent "ModuleManager" plugin (included) from Ialdabaoth/Sarbian.
( credits and updates here : http://forum.kerbalspaceprogram.com/threads/55219-Module-Manager-1-5-%28Nov-11%29 )

_________________________________________________________
How it works ?


* When building your rockets, you can assign actions (Start Countdown and Abort Launch) shortcut key/button for launch clamps in the action group editor tab.
or/and
* When on the launch pad, just Push dah "Go Flight !" Button.

* Put your in-game engines volume around 25% for the best launch sequence experience

Be aware that it will auto-activate the launch stage once the countdown reach "Ignition" so plan your stage accordingly :
-> First stage : Engines AND launch clamps,
-> and do NOT wait ignition to put some generous throttle...

Mechjeb's autopilot users :
1. Setup your ascension profile,
2. Engage MJ autopilot,
3. Start the countdown,
4. Sit back and relax ^^


When using "Apollo Style" launch sequence :
1. You can set your first stage with liquid fuel engines, and your second stage with launch clamps and SRB.
2. This way you can spool up your main engines (at around 10% thrust) by activating your first stage when "engine ignition start" is announced at T-8 seconds.
3. Gently put generous throttle when "All engines running" is announced at T-1 second.
4. And wait for the release of the clamps and Lift-off !



________________________________________________________
Known issues :
- you may spend too much time on the launch pad now ^^
- let me know if you find some more...

________________________________________________________
Changelog :

v 1.7.2 :
- Recompiled for KSP 0.24.2
- Fixed FASA Apollo/Atlas launchclamps  support
Known issue : 
- countdown sequence pause every seconds in KSP x64 (32bit is fine)
(under investigation)

v 1.7.1 :
- Recompiled for KSP 0.23.5 (to be on the safe side)
- Added frizzank's FASA launchclamps compatibility (ModuleManager cfg)
- Added a Debug option (spam the log with useless stuff)
- Audioclips stop when entering settings

v 1.7 :
- Added back Kerbalized audio set (made it default)
- Plugin now clear audio collections on part unload to free up memory

v 1.6.1 (Hotfix) :
- Fixed issue where audio clips were loaded for each instance of launchclamp part.

v 1.6 :
- Rewritten the code using 'Collections' to manage audio files
- This also fixed the desync sequence issue when using heavy ship/mods
- Added some screen messages accordingly to the launch sequence events
- disabled "Kerbalized" & "Default" audio set for now (better Kerbalized version in the work)

v 1.5 :
- recompiled/checked for KSP v0.23 compatibility
- targeting .NET 3.5 instead of 4.5
- including ModuleManager v1.5

v 1.4 :
- Added Kerbalized audio (first pass) 

v 1.3 :
- Made audio sequences being tied to "voices" volume setting instead of engines
- Also made audio 2D+panLevel 0 to avoid weird sounds issues when zooming view
- Added "Apollo Style" launch sequence
- Added a setting window allowing to choose launch sequences
- updated with Module Manager 4992.21296 (Sarbian's version)

v 1.2 :
- Added an "Abort Launch !" button,
- Reduced the size of the UI.

v 1.1 :
- Fixed, issue when multiple launch clamp parts was present (Cybutek "using static var" trick),
- Replaced 'right-click' on launch clamp by an UI Launch button,
- Made the new UI button position persistent when dragged,
- Made the new UI button to recenter if it goes off screen on resolution change,
- Some code cleaning, optimizations.

v 1.0 : Initial release.

________________________________________________________
(To do list) :
- auto-detect if no engines are on the current stage to avoid wounds,
- add a Kerbalish countdown, Chatterer style,
- add an advanced mode with detailed launch procedures (checks, engine ignition, ...),
- learn to code properly and optimize methods...

KSP Forum thread : http://forum.kerbalspaceprogram.com/threads/42859-0-22-Athlonic-Electronics-LCD-Launch-CountDown-%28Audio-Onscreen%29-v1-3

License (boring) stuff :
Attribution-NonCommercial-ShareAlike 3.0 Unported (CC BY-NC-SA 3.0)
http://creativecommons.org/licenses/by-nc-sa/3.0/
