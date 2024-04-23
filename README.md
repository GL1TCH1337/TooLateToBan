# TooLateToBan

This plugins allows you to ban/silence/mute/gag players who disconnected from server before you punish them.

# Requirements
[Counter Strike Sharp](https://github.com/roflmuffin/CounterStrikeSharp)
[Simple Admin](https://github.com/daffyyyy/CS2-SimpleAdmin/)

# Installation
- Download the latest release from [https://github.com/gl1tch1337/TooLateToBan/releases](https://github.com/gl1tch1337/TooLateToBan/tags)
- Extract the .zip file into `addons/counterstrikesharp/plugins`
- Enjoy

# Configuration
```json
{
  "MaximumCacheSize": 10, // Maximum player count this plugin will cache. If the count exceeds very first player in list will be deleted.
  "Debug": false, // Enables debug messages. No need to change.
  "CommandName": "tltb", // Changes chat command 
  "Reasons": [ // Reason list which will populate the menu.
    "Hacking",
    "Voice Abuse",
    "Chat Abuse",
    "Admin disrespect",
    "Other"
  ],
  "ConfigVersion": 1
}
```

Credits to https://forums.alliedmods.net/showthread.php?p=870222 for plugin idea.


