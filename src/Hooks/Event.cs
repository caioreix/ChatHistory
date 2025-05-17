

using HarmonyLib;
using ProjectM;
using UnityEngine.EventSystems;
using Utils.Logger;

namespace ChatHistory.Hooks;

[Harmony]

public class EventPatch {
    // [HarmonyPatch(typeof(EventSystem), nameof(EventSystem.SendMessage))]
    // public class SendMessage {
    //     public static void Prefix(EventSystem __instance, string message) {
    //         Log.Debug($"EventPatch.SendMessage: ");
    //     }
    // }

    // [HarmonyPatch(typeof(ClientSystemChatUtils), nameof(ClientSystemChatUtils.AddLocalMessage))]
    // public class AddLocalMessage {
    //     public static void Prefix() {
    //         Log.Debug($"EventPatch.AddLocalMessage: ");
    //     }
    // }

    // [HarmonyPatch(typeof(ClientSystemChatUtils), nameof(ClientSystemChatUtils.AddLocalSystemMessage))]
    // public class AddLocalSystemMessage {
    //     public static void Prefix() {
    //         Log.Debug($"EventPatch.AddLocalMessage: ");
    //     }
    // }
}
