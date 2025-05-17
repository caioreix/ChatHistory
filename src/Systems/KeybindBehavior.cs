using System;
using ChatHistory.Settings;
using RTLTMPro;
using UnityEngine;
using Utils.Logger;

namespace ChatHistory.Systems;

public class KeyBindsBehaviour : MonoBehaviour {

    public KeyBindsBehaviour(IntPtr handle) : base(handle) { }

    private readonly long threshold = 100;
    // public void Start() { }

    public void Update() {
        if (!History.IsFocused()) {
            return;
        }

        if (Input.GetKeyDown(ENV.DownHistoryKey.Value)) {
            // int pos = History.JumpCurrentPosition(-1);
            // Log.Debug($"History[{pos}]: {History.GetHistory(pos)}");
            HistoryDown();
        }

        if (Input.GetKeyDown(ENV.UpHistoryKey.Value)) {
            // HistoryUp();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            // HistoryComplete();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (History.PlaceholderClone.enabled) {
                History.PlaceholderClone.enabled = false;
            }
            History.SetDefaultPlaceholder();
            History._canComplete = false;
        }
    }

    private static void HistoryComplete() {
        if (History._canComplete) {
            RTLTextMeshPro placeholder = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
            History.InputField.text = placeholder.text.ToString();
            History.InputField.stringPosition = History.InputField.text.Length;
            History.PlaceholderClone.enabled = false;
            History._canComplete = false;
        }
    }

    private static void HistoryUp() {
        string currentInput = History.InputField.text;
        string historyItem = History.NavigateUp(currentInput);
        // History.InputField.text = historyItem;

        if (string.IsNullOrEmpty(historyItem)) {
            return;
        }

        History.PlaceholderClone.enabled = true;
        RTLTextMeshPro placeholder = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
        placeholder.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
        History._canComplete = true;
    }

    private static void HistoryDown() {
        string currentInput = History.InputField.text;
        string historyItem = History.NavigateDown(currentInput);
        // History.InputField.text = historyItem;

        if (string.IsNullOrEmpty(historyItem)) {
            return;
        }

        History.PlaceholderClone.enabled = true;
        RTLTextMeshPro placeholder = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
        placeholder.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
        History._canComplete = true;
    }
}
