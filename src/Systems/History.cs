
using System;
using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using UnityEngine;
using Utils.Database;
using Utils.Logger;

namespace ChatHistory.Systems;

public class History {
    public static TMPro.TMP_InputField InputField = null;
    public static UnityEngine.UI.Graphic PlaceholderClone = null;

    public static void SetInputField(TMPro.TMP_InputField inputField) {
        // // inputField.placeholder
        // PlaceholderClone = UnityEngine.Object.Instantiate(inputField.placeholder);
        // // Save the original parent of the placeholder
        // var originalParent = inputField.placeholder.transform.parent;
        // // Create a dummy parent to store it temporarily if needed
        // var tempParent = new UnityEngine.GameObject("TempPlaceholderParent").transform;
        // // Reparent the placeholder
        // inputField.placeholder.transform.SetParent(tempParent);
        // // Clone it
        // PlaceholderClone = UnityEngine.Object.Instantiate(inputField.placeholder);
        // Move the original placeholder back to its original parent
        if (!Cache.Exists("PlaceholderClone")) {
            PlaceholderClone = UnityEngine.Object.Instantiate(inputField.placeholder);
            PlaceholderClone.transform.SetParent(inputField.placeholder.transform.parent);
            PlaceholderClone.transform.position = inputField.placeholder.transform.position;
            PlaceholderClone.transform.localPosition = inputField.placeholder.transform.localPosition;
            PlaceholderClone.enabled = true;
            // inputField.onValueChanged.AddListener(() => {
            //     PlaceholderClone.enabled = false;
            // });

            inputField.placeholder.transform.SetParent(inputField.placeholder.transform.parent.parent);
            inputField.placeholder.transform.localPosition = new Vector3(0, PlaceholderClone.transform.localPosition.y + 21, PlaceholderClone.transform.localPosition.z);
            RTLTextMeshPro textMesh = inputField.placeholder.GetComponent<RTLTextMeshPro>();
            textMesh.color = new Color(
                1,
                1,
                1,
                1
            );
            Cache.Key("PlaceholderClone", true);
            Log.Debug($"PlaceholderClone {inputField.placeholder.transform.parent.name}: {inputField.placeholder.transform.position}");
        }

        // Cleanup the temporary object
        // UnityEngine.Object.Destroy(tempParent.gameObject);


        InputField = inputField;
    }

    private static List<string> _history = [];
    private static bool _isFocused = false;
    private static int _currentPosition = 0;
    public static bool _canComplete = false;
    public static string _defaultPlaceholder = "";

    public static void SetDefaultPlaceholder() {
        if (PlaceholderClone == null) {
            return;
        }

        _defaultPlaceholder = PlaceholderClone.GetComponent<RTLTextMeshPro>().text.ToString();
        PlaceholderClone.enabled = true;
    }

    public static void Reset(string message) {
        if (string.IsNullOrEmpty(message)) {
            PlaceholderClone.GetComponent<RTLTextMeshPro>().text = _defaultPlaceholder;
            PlaceholderClone.enabled = true;
        }

        _currentPosition = 0;
        _canComplete = false;
    }

    public static string GetHistory(int index) {
        if (_history.Count == 0) {
            return string.Empty;
        } else if (index < 0) {
            index = 0;
        } else if (index >= _history.Count) {
            index = _history.Count - 1;
        }

        return _history[index];
    }

    public static string GetCurrentItem() {
        return GetHistory(_currentPosition);
    }

    public static List<string> SearchHistory(string prefix) {
        if (string.IsNullOrEmpty(prefix))
            return _history.ToList();

        return _history
            .Where(item => item.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static string FindAndNavigateHistory(string prefix, bool searchUp) {
        var matches = SearchHistory(prefix);
        if (matches.Count == 0)
            return string.Empty;

        int startPos = _currentPosition;
        int direction = searchUp ? 1 : -1;
        int count = 0;

        while (count < matches.Count) {
            int newPos = (_currentPosition + direction) % _history.Count;
            if (newPos < 0) newPos = _history.Count - 1;

            _currentPosition = newPos;
            var current = GetCurrentItem();

            if (current.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return current;

            if (_currentPosition == startPos)
                break;

            count++;
        }

        return string.Empty;
    }

    public static string NavigateUp(string currentInput = "") {
        if (_history.Count == 0)
            return currentInput;

        if (!string.IsNullOrEmpty(currentInput)) {
            return FindAndNavigateHistory(currentInput, true);
        }

        JumpCurrentPosition(1);
        return GetCurrentItem();
    }

    public static string NavigateDown(string currentInput = "") {
        if (_history.Count == 0)
            return currentInput;

        if (!string.IsNullOrEmpty(currentInput)) {
            return FindAndNavigateHistory(currentInput, false);
        }

        JumpCurrentPosition(-1);
        return GetCurrentItem();
    }

    public static bool SetFocusState(bool state) {
        if (state == false) {
            _currentPosition = 0;
        }

        _isFocused = state;
        return _isFocused;
    }

    public static bool IsFocused() {
        return _isFocused;
    }

    public static int JumpCurrentPosition(int jumps = 1) {
        _currentPosition += jumps;

        int length = _history.Count;
        if (_currentPosition >= length && length > 0) {
            _currentPosition = length - 1;
        } else if (_currentPosition < 0 || length == 0) {
            _currentPosition = 0;
        }

        return _currentPosition;
    }

    public static void SetCurrentPosition(int position) {
        _currentPosition = position;
    }

    public static int GetCurrentPosition() {
        return _currentPosition;
    }

    public static void Prepend(string message) {
        if (string.IsNullOrEmpty(message))
            return;

        _history.Insert(0, message);
    }
}
