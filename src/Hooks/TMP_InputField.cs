using HarmonyLib;
using TMPro;
using Utils.Logger;

namespace ChatHistory.Hooks;

[Harmony]

public class TMP_InputFieldPatch {

    [HarmonyPatch(typeof(TMP_InputField), nameof(TMP_InputField.LineUpCharacterPosition))]
    public class LineUpCharacterPosition {
        public static void Prefix(TMP_InputField __instance, int originalPos, ref bool goToFirstChar) {
            Log.Debug($"LineUpCharacterPosition: {originalPos} {goToFirstChar}");
            goToFirstChar = false;
        }
    }

    [HarmonyPatch(typeof(TMP_InputField), nameof(TMP_InputField.LineDownCharacterPosition))]
    public class LineDownCharacterPosition {
        public static void Prefix(TMP_InputField __instance, int originalPos, ref bool goToLastChar) {
            Log.Debug($"LineDownCharacterPosition: {originalPos} {goToLastChar}");
            goToLastChar = false;
        }
    }
}
