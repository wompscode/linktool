# linktool
Initializes the Oculus Link mic and then terminates once SteamVR has started, so you don't have to!

# What does this actually do?
This tool just uses NAudio to initialize the mic, and then once SteamVR starts, it runs a command in OculusDebugToolCLI to disable ASW then terminates itself.

# Releases
Precompiled binaries are available [here](https://github.com/i386sh/linktool/releases)  
*if there are any issues **please** let me know, this tool was really quickly built.* 

# Building
If you wanna build it just download the repo and then install NAudio from NuGet. Simple stuff.
