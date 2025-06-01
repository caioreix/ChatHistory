using BepInEx;
using BepInEx.Unity.IL2CPP;
using ChatHistory.Systems;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utils.VRising.Entities;

namespace ChatHistory;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]

public class Plugin : BasePlugin {
    private static Harmony harmony;
    private static KeyBindsBehaviour keyBindsBehaviour;

    private static void UnregisterKeyBindsBehaviour() {
        if (ClassInjector.IsTypeRegisteredInIl2Cpp<KeyBindsBehaviour>()) {
            {
                var field = typeof(ClassInjector).GetField("InjectedTypes", BindingFlags.NonPublic | BindingFlags.Static);
                var value = field.GetValue(null);
                var InjectedTypes = value as HashSet<string>;
                InjectedTypes?.Remove(typeof(KeyBindsBehaviour).FullName);
            }

            {
                var helpersType = typeof(ClassInjector).Assembly.GetType("Il2CppInterop.Runtime.Injection.InjectorHelpers");
                var field = helpersType.GetField("s_ClassNameLookup", BindingFlags.NonPublic | BindingFlags.Static);
                var value = field.GetValue(null);
                if (value is Dictionary<(string _namespace, string _class, IntPtr imagePtr), IntPtr> s_ClassNameLookup) {
                    var type = typeof(KeyBindsBehaviour);
                    string klass = type.Name;
                    string namespaze = type.Namespace ?? string.Empty;

                    s_ClassNameLookup.Keys.Where(key => key._class == klass && key._namespace == namespaze).ToList().ForEach(key => {
                        s_ClassNameLookup.Remove(key);
                    });
                }
            }
        }
    }

    public override void Load() {
        if (!World.IsClient) {
            return;
        }

        Settings.Config.Load(Config, Log, "Client");

        UnregisterKeyBindsBehaviour();
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
        UnregisterKeyBindsBehaviour();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} client side is unloaded!");
        return true;
    }
}
