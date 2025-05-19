using System;
using ChatHistory.Settings;
using UnityEngine;

namespace ChatHistory.Systems;

public class KeyBindsBehaviour : MonoBehaviour {

    public KeyBindsBehaviour(IntPtr handle) : base(handle) { }

    public void Start() {
        History.Initialize();
    }

    public void Update() {
        if (History.InputField != null) {
            History.InputField.placeholder.enabled = true;
        }

        if (!History.IsFocused()) {
            return;
        }

        if (Input.GetKeyDown(ENV.DownHistoryKey.Value)) {
            HistoryDown();
        }

        if (Input.GetKeyDown(ENV.UpHistoryKey.Value)) {
            HistoryUp();
        }

        if (Input.GetKeyDown(ENV.CompleteHistoryKey.Value)) {
            HistoryComplete();
        }
    }

    private static void HistoryComplete() {
        if (History.CanComplete()) {
            History.InputField.text = History.PlaceholderCloneRTLTextMeshPro.text.ToString();
            History.InputField.stringPosition = History.InputField.text.Length;
            History.PlaceholderClone.enabled = false;
            History.PlaceholderCloneRTLTextMeshPro.text = "";
        }
    }

    private static void HistoryUp() {
        string currentInput = History.InputField.text;
        string historyItem = History.NavigateUp(currentInput);

        if (string.IsNullOrEmpty(historyItem)) {
            History.InputField.stringPosition = currentInput.Length;
            return;
        }

        History.PlaceholderClone.enabled = true;
        History.PlaceholderCloneRTLTextMeshPro.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
    }

    private static void HistoryDown() {
        string currentInput = History.InputField.text;
        string historyItem = History.NavigateDown(currentInput);

        if (string.IsNullOrEmpty(historyItem)) {
            History.SetPlaceHolderEmpty();
            return;
        }

        History.PlaceholderClone.enabled = true;
        History.PlaceholderCloneRTLTextMeshPro.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
    }
}
