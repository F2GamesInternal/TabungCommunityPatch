# TabungCommunityPatch v1.0

## Installing TabungCommunityPatch
Requirements
- A clean, unmodified copy of The Tabung
- BepInEx 5.x (included with the release if applicable)

## Installation
- Download the latest .DLL release from the GitHub Releases page.
- Extract the archive.
- Copy the OfflinePhoton.DLL file into your BepInEx\plugins folder for The Tabung.


## Required files in BepInEx/core

- 0Harmony.dll
- BepInEx.dll
- BepInEx.Preloader.dll
- BepInEx.Harmony.dll
- HarmonyXInterop.dll
- Mono.Cecil.dll
- Mono.Cecil.Mdb.dll
- Mono.Cecil.Pdb.dll
- Mono.Cecil.Rocks.dll
- MonoMod.Backports.dll
- MonoMod.ILHelpers.dll
- MonoMod.RuntimeDetour.dll
- MonoMod.Utils.dll

If MonoMod.ILHelpers.dll is missing, the plugin can fail with:
FileNotFoundException: Could not load file or assembly 'MonoMod.ILHelpers'

# Modes
This build adds a config-driven Online / Offline split.

Offline:
Uses the offline room patches and local-only RPC / spawn behavior.

Online:
Leaves the game's normal multiplayer flow intact, while replacing the Photon App ID and region from config.

# Config file

After the plugin runs once, edit:

BepInEx/config/OfflinePhoton.cfg

Important fields:
- General = Mode
- General = Nickname
- Photon  = AppIdRealtime
- Photon = AppIdVoice
- Photon  = Region



Example Online config:
- Mode = Online
- AppIdRealtime = your_photon_app_id_here
- AppIdVoice = your_photon_app_id_here
- Region = us

If AppIdRealtime is empty, the plugin falls back to Offline behavior.

# What must be referenced in Visual Studio

Game DLLs:
- Assembly-CSharp.dll
- PhotonUnityNetworking.dll
- PhotonRealtime.dll
- Photon3Unity3D.dll
- PhotonChat.dll

Unity DLLs:
- UnityEngine.dll
- UnityEngine.CoreModule.dll
- UnityEngine.SceneManagementModule.dll
- UnityEngine.UI.dll
- UnityEngine.IMGUIModule.dll
- UnityEngine.InputLegacyModule.dll

BepInEx / runtime:
- BepInEx.dll
- 0Harmony.dll

## Build steps

1. Open the project in Visual Studio 2022.
2. Add the references listed above.
3. Build Release.
4. Copy OfflinePhoton.dll into BepInEx/plugins.

## Debug console

Press F1 to toggle the debug console.
It logs:
- debug messages
- warnings
- errors
- exceptions
- stack traces
