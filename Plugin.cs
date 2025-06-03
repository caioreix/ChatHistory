using BepInEx;
using BepInEx.Unity.IL2CPP;
using ChatHistory.Systems;
using HarmonyLib;
using UnityEngine;
using Utils.Injection;
using Utils.VRising.Entities;

namespace ChatHistory;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]

public class Plugin : BasePlugin {
    private static Harmony harmony;
    private static KeyBindsBehaviour keyBindsBehaviour;

    public override void Load() {
        if (!World.IsClient) {
            return;
        }

        Settings.Config.Load(Config, Log, "Client");

        IL2CPP.Unregister<KeyBindsBehaviour>();
        keyBindsBehaviour = AddComponent<KeyBindsBehaviour>();

        harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        Log.LogDebug("Patching harmony");
        harmony.PatchAll();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} client side is loaded!");
    }

    public override bool Unload() {
        if (!World.IsClient) {
            return true;
        }

        harmony.UnpatchSelf();
        GameObject.Destroy(keyBindsBehaviour);
        IL2CPP.Unregister<KeyBindsBehaviour>();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} client side is unloaded!");
        return true;
    }
}
