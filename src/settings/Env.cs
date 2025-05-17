using UnityEngine;
using Utils.Settings;

namespace ChatHistory.Settings;

public static class ENV {
    // Mission_General
    private readonly static string settings = "0.⚙️ Settings";
    public static ConfigElement<KeyCode> DownHistoryKey { get; set; }
    public static ConfigElement<KeyCode> UpHistoryKey { get; set; }
    public static ConfigElement<KeyCode> CompleteHistoryKey { get; set; }
    public static ConfigElement<int> MaximumHistorySize { get; set; }
    public static ConfigElement<bool> PersistOnRestart { get; set; }

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

            MaximumHistorySize = Utils.Settings.Config.Bind(
                settings,
                nameof(MaximumHistorySize),
                50,
                "The maximum number of messages to keep in the chat history. If the number of messages exceeds this value, the oldest messages will be removed. If set to <= 0, the history will not be limited (use with caution)."
            );

            PersistOnRestart = Utils.Settings.Config.Bind(
                settings,
                nameof(PersistOnRestart),
                true,
                "If true, the chat history will be saved and loaded on restart. If false, the chat history will be lost on restart."
            );

            Utils.Settings.Config.Save();
        }
    }
}
