This plugin has been ported to Exiled 3.0 by JohnodonCode. This was originally created by PintTheDragon.

# EasyEvents
EasyEvents is a plugin to let you easily create simple-to-use and powerful events for your server. It works by letting you create event scripts/profiles. These scripts are lists of simple commands that will modify the game. For example, you can change the spawn rates for certain roles, detonate the warhead, teleport roles to certain locations, and more. EasyEvents aims to automate all of the work that goes into running events, and allows you to create events that would be difficult to do manually. To learn how to create custom events, check out the [Wiki](https://github.com/PintTheDragon/EasyEvents/wiki).

## Example
Here is an example of what a EasyEvent script for the event "Peanut Run" might look like:
```
//We need to lock the round before we spawn in the SCPs, or else it will end immediately.
roundlock

//0 is the class number for SCP-173, and the 100 means that 100% of players will spawn in as it.
spawn 0,100

//This will turn on the warhead
detonate
```
This text will need to be put in a text file, inside the script directory (which can be found and changed inside the config file). The default directory is inside of the EXILED Config directory, named "EasyEvents" (%APPDATA%\EXILED\Config\EasyEvents on windows).

You can also check out the [Wiki](https://github.com/JohnodonCode/EasyEvents/wiki) for more information on creating events.

## Configuration
The above section goes into more detail on how to setup scripts. To configure EasyEvents, you can go into EXILED's config file. Currently, you will only be able to change the script directory.

## Usage
To use EasyEvents, first setup a script. Then you can use the script's name (if your script is named "peanutrun.txt", this will be "peanutrun") in the "events" command. Simply go into the RemoteAdmin console, then type "event &lt;event name&gt;", replacing "&lt;event name&gt;" with the name of the script. Note that events can only be ran before the round is started.
