using UnityEngine;
using Utils.Settings;

namespace ChatHistory.Settings;

public static class ENV {
    // Mission_General
    private readonly static string settings = "0.⚙️ Settings";
    public static ConfigElement<KeyCode> DownHistoryKey { get; set; }
    public static ConfigElement<KeyCode> UpHistoryKey { get; set; }
    public static ConfigElement<KeyCode> CompleteHistoryKey { get; set; }

    public static class Testing {

        public static void Setup() {
            Utils.Settings.Config.AddConfigActions(load);
        }

        // Load the plugin config variables.
        private static void load() {
            DownHistoryKey = Utils.Settings.Config.Bind(
                settings,
                nameof(DownHistoryKey),
                KeyCode.DownArrow,
                "Down Arrow Key to scroll down the chat history."
            );

            UpHistoryKey = Utils.Settings.Config.Bind(
                settings,
                nameof(UpHistoryKey),
                KeyCode.UpArrow,
                "Up Arrow Key to scroll up the chat history."
            );

            CompleteHistoryKey = Utils.Settings.Config.Bind(
                settings,
                nameof(CompleteHistoryKey),
                KeyCode.RightArrow,
                "The key to complete the current chat message."
            );

            Utils.Settings.Config.Save();
        }
    }
}
