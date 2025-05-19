using ChatHistory.Systems;
using HarmonyLib;
using ProjectM.UI;

namespace ChatHistory.Hooks;

[Harmony]

public class ClientChatSystemPatch {
    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem._OnInputChanged))]
    public class OnInputChanged {
        public static void Prefix() {
            History.Reset();
        }
    }

    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem._OnInputSelect))]
    public class OnInputSelect {
        public static void Prefix(ClientChatSystem __instance) {
            History.SetInputField(__instance._ChatWindow.ChatInputField);
            History.SetFocusState(true);
        }
    }

    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem._OnInputDeselect))]
    public class OnInputDeselect {
        public static void Prefix() {
            History.SetFocusState(false);
        }
    }

    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem.OnInputFocusLoss))]
    public class OnInputFocusLoss {
        public static void Prefix() {
            History.SetFocusState(false);
        }
    }
}
