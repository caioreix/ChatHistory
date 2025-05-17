using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using ChatHistory.Systems;
using HarmonyLib;
using UnityEngine;

namespace ChatHistory;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]

public class Plugin : BasePlugin {
    private static Harmony harmony;
    public override void Load() {
        Settings.Config.Load(Config, Log, "Client");

        AddComponent<KeyBindsBehaviour>();

        harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Utils.Logger.Log.Trace("Patching harmony");
        harmony.PatchAll();

        Utils.Logger.Log.Info($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
