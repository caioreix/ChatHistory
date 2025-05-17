using System;
using ChatHistory.Settings;
using RTLTMPro;
using UnityEngine;
using Utils.Logger;

namespace ChatHistory.Systems;

public class KeyBindsBehaviour : MonoBehaviour {

    public KeyBindsBehaviour(IntPtr handle) : base(handle) { }

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

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            History.InputField.placeholder.enabled = true;
            HistoryComplete();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            History.InputField.placeholder.enabled = true;
        }
    }

    private static void HistoryComplete() {
        if (History.CanComplete()) {
            RTLTextMeshPro textMesh = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
            History.InputField.text = textMesh.text.ToString();
            History.InputField.stringPosition = History.InputField.text.Length;
            History.PlaceholderClone.enabled = false;
            textMesh.text = "";
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
        RTLTextMeshPro placeholder = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
        placeholder.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
        History._canComplete = true;
    }

    private static void HistoryDown() {
        string currentInput = History.InputField.text;
        string historyItem = History.NavigateDown(currentInput);

        if (string.IsNullOrEmpty(historyItem)) {
            History.PlaceholderClone.GetComponent<RTLTextMeshPro>().text = "";
            History.PlaceholderClone.enabled = false;
            return;
        }

        History.PlaceholderClone.enabled = true;
        RTLTextMeshPro placeholder = History.PlaceholderClone.GetComponent<RTLTextMeshPro>();
        placeholder.text = historyItem;

        History.InputField.stringPosition = currentInput.Length;
        History._canComplete = true;
    }
}
