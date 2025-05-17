using System;
using ChatHistory.Systems;
using HarmonyLib;
using ProjectM.Network;
using ProjectM.UI;
using Unity.Collections;
using Unity.Entities;
using Utils.Logger;
using Utils.VRising;

namespace ChatHistory.Hooks;

[Harmony]

public class ClientChatSystemPatch {
    public static Entity _localUser = Entity.Null;

    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem._OnInputChanged))]
    public class OnInputChanged {
        public static void Prefix(ref ClientChatSystem __instance, string text) {
            History.Reset(text);
            Log.Debug($"ClientChatSystemPatch._OnInputChanged: {text} pos {__instance._ChatWindow.ChatInputField.caretPosition}");
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

    [HarmonyPatch(typeof(ClientChatSystem), nameof(ClientChatSystem.OnUpdate))]
    public class OnUpdate {
        public static void Prefix(ClientChatSystem __instance) {
            NativeArray<Entity> entities = __instance._ReceiveChatMessagesQuery.ToEntityArray(Allocator.Temp);
            try {
                foreach (Entity entity in entities) {
                    try {
                        if (entity.Has<ChatMessageServerEvent>()) {
                            // Debug: Print all components on the entity
                            Log.Debug($"Entity {entity} components:");
                            var entityManager = __instance.EntityManager;
                            var componentTypes = entityManager.GetComponentTypes(entity);
                            try {
                                foreach (var componentType in componentTypes) {
                                    Log.Debug($"  - {componentType}");
                                }
                            } finally {
                                componentTypes.Dispose();
                            }

                            if (_localUser == Entity.Null) {
                                _localUser = Utils.VRising.Entities.User.GetLocalUser();
                            }

                            // Entity userEntity = entity.Read<FromCharacter>().User;

                            // if (userEntity != _localUser) {
                            //     Log.Debug($"ClientChatSystemPatch.OnUpdate: not local user {userEntity} != {_localUser}");
                            //     continue;
                            // }

                            ChatMessageServerEvent chatMessage = entity.Read<ChatMessageServerEvent>();
                            string message = chatMessage.MessageText.Value;

                            Log.Debug($"ClientChatSystemPatch.OnUpdate: {message}");

                            History.Prepend(message);
                        }
                    } catch (Exception e) {
                        Log.Warning($"ClientChatSystemPatch.OnUpdate: failed to process entity: {entity}, error: {e}");
                    }
                }
            } finally {
                entities.Dispose();
            }
        }
    }
}
