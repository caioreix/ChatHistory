
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using ChatHistory.Settings;
using Newtonsoft.Json;
using RTLTMPro;
using UnityEngine;
using Utils.Database;
using Utils.Logger;

namespace ChatHistory.Systems;

public class History {
    public static TMPro.TMP_InputField InputField = null;
    public static UnityEngine.UI.Graphic PlaceholderClone = null;
    public static string LastNonEmptyMessage = "";

    public static bool CanComplete() {
        return !string.IsNullOrEmpty(PlaceholderClone.GetComponent<RTLTextMeshPro>().text.ToString());
    }

    public static void SetInputField(TMPro.TMP_InputField inputField) {
        if (!Cache.Exists("PlaceholderClone")) {
            PlaceholderClone = UnityEngine.Object.Instantiate(inputField.placeholder);
            PlaceholderClone.transform.SetParent(inputField.placeholder.transform.parent);
            PlaceholderClone.transform.position = inputField.placeholder.transform.position;
            PlaceholderClone.transform.localPosition = inputField.placeholder.transform.localPosition;
            PlaceholderClone.enabled = true;

            inputField.placeholder.transform.SetParent(inputField.placeholder.transform.parent.parent);
            inputField.placeholder.transform.localPosition = new Vector3(0, PlaceholderClone.transform.localPosition.y + 21, PlaceholderClone.transform.localPosition.z);
            RTLTextMeshPro textMesh = inputField.placeholder.GetComponent<RTLTextMeshPro>();
            textMesh.color = new Color(1, 1, 1, 1);
            Cache.Key("PlaceholderClone", true);
        }

        SetPlaceHolderEmpty();
        InputField = inputField;
    }

    private static List<string> _history = [];
    private static bool _isFocused = false;
    private static int _currentPosition = -1;
    public static bool _canComplete = false;
    public static string _defaultPlaceholder = "";

    public static void SetDefaultPlaceholder() {
        if (PlaceholderClone == null) {
            return;
        }

        _defaultPlaceholder = PlaceholderClone.GetComponent<RTLTextMeshPro>().text.ToString();
        PlaceholderClone.enabled = true;
    }

    public static void Reset(string text) {
        if (!string.IsNullOrEmpty(text)) {
            LastNonEmptyMessage = text;
        }

        PlaceholderClone.GetComponent<RTLTextMeshPro>().text = "";

        InputField.placeholder.enabled = true;

        _currentPosition = -1;
        _canComplete = false;
    }

    public static string GetHistory(int index) {
        if (_history.Count == 0 || index == -1) {
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

        int direction = searchUp ? 1 : -1;
        int checkedItems = 0;
        int originalPosition = _currentPosition;

        while (checkedItems < _history.Count) {
            // Calculate new position
            int newPos = _currentPosition + direction;

            // Handle boundaries
            if (searchUp && newPos >= _history.Count) {
                // We've hit the end while going up, restore position
                _currentPosition = originalPosition;
                return string.Empty;
            }
            if (!searchUp && newPos < -1) {
                _currentPosition = -1; // Stay at -1 position when going down
                return string.Empty;
            }

            _currentPosition = newPos;

            // If we've reached -1 when navigating down, return empty string
            if (_currentPosition == -1)
                return string.Empty;

            var current = GetCurrentItem();

            // Skip if current exactly matches the prefix, only return if it starts with but is longer
            if (current.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(current, prefix, StringComparison.OrdinalIgnoreCase))
                return current;

            checkedItems++;
        }

        // If no match was found, restore original position
        _currentPosition = originalPosition;
        return string.Empty;
    }

    public static string NavigateUp(string currentInput = "") {
        if (_history.Count == 0)
            return string.Empty;

        if (!string.IsNullOrEmpty(currentInput)) {
            return FindAndNavigateHistory(currentInput, true);
        }

        JumpCurrentPosition(1);
        return GetCurrentItem();
    }

    public static string NavigateDown(string currentInput = "") {
        if (_history.Count == 0)
            return string.Empty;

        if (!string.IsNullOrEmpty(currentInput)) {
            return FindAndNavigateHistory(currentInput, false);
        }

        JumpCurrentPosition(-1);
        return GetCurrentItem();
    }

    public static void SetPlaceHolderEmpty() {
        if (PlaceholderClone == null) {
            return;
        }

        PlaceholderClone.GetComponent<RTLTextMeshPro>().text = "";
        PlaceholderClone.enabled = false;
    }

    public static bool SetFocusState(bool state) {
        if (state == false) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                Prepend(LastNonEmptyMessage);
            }

            LastNonEmptyMessage = "";
            _currentPosition = -1;
            SetPlaceHolderEmpty();
        }

        _isFocused = state;
        return _isFocused;
    }

    public static bool IsFocused() {
        return _isFocused;
    }

    public static int JumpCurrentPosition(int jumps = 1) {
        int length = _history.Count;
        if (length == 0) {
            _currentPosition = -1;
            return _currentPosition;
        }

        _currentPosition += jumps;

        if (_currentPosition >= length) {
            _currentPosition = length - 1;
        } else if (_currentPosition < 0) {
            _currentPosition = -1;
        }

        return _currentPosition;
    }

    public static void SetCurrentPosition(int position) {
        _currentPosition = position;
    }

    public static int GetCurrentPosition() {
        return _currentPosition;
    }

    private static readonly string HistoryFilePath = Path.Combine(Paths.PluginPath, "ChatHistory", "chat.history");
    private static int MaxHistorySize => ENV.MaximumHistorySize.Value;
    private static int TrimThreshold => ENV.MaximumHistorySize.Value * 2 < 100 ? ENV.MaximumHistorySize.Value * 2 : ENV.MaximumHistorySize.Value + 100; // Trim when file is 2x the max size
    private static int _entryCounter = 0; // Count entries between checks

    public static void SaveHistoryEntry(string text) {
        try {
            // Ensure directory exists
            string directory = Path.GetDirectoryName(HistoryFilePath);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // Encode the string to handle special characters and line breaks
            string encodedText = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));

            // Use StreamWriter with append mode to add a single entry
            using (StreamWriter writer = new StreamWriter(HistoryFilePath, true)) {
                writer.WriteLine(encodedText);
            }

            // Increment counter and check if we should verify file size

            CheckAndTrimIfNeeded();
        } catch (Exception ex) {
            Log.Error($"Failed to save history entry: {ex.Message}");
        }
    }

    private static void CheckAndTrimIfNeeded() {
        if (MaxHistorySize <= 0) {
            return;
        }
        _entryCounter++;
        if (_entryCounter <= TrimThreshold) {
            return;
        }

        _entryCounter = _history.Count;

        try {
            if (!File.Exists(HistoryFilePath)) {
                return;
            }

            // Count lines
            int lineCount = 0;
            using (StreamReader reader = new StreamReader(HistoryFilePath)) {
                while (reader.ReadLine() != null) {
                    lineCount++;

                    // Early exit if we're already over threshold
                    if (lineCount > TrimThreshold) {
                        break;
                    }
                }
            }

            // If we're over the overflow threshold, trim the file
            if (lineCount > TrimThreshold) {
                Log.Trace($"File exceeded threshold ({lineCount} > {TrimThreshold}), trimming...");

                string[] allLines = File.ReadAllLines(HistoryFilePath);
                string[] trimmedLines = allLines.Skip(allLines.Length - MaxHistorySize).ToArray();

                File.WriteAllLines(HistoryFilePath, trimmedLines);

                Log.Trace($"Trimmed history file from {allLines.Length} to {trimmedLines.Length} entries");
            }
        } catch (Exception ex) {
            Log.Error($"Failed to check/trim history file: {ex.Message}");
        }
    }

    public static void LoadHistory() {
        try {
            _history.Clear();

            if (File.Exists(HistoryFilePath)) {
                // Read all lines and decode them
                string[] encodedLines = File.ReadAllLines(HistoryFilePath);
                bool needsTrimming = encodedLines.Length > MaxHistorySize;

                // Process lines in reverse order (newest first)
                foreach (string encodedLine in encodedLines.Reverse()) {
                    try {
                        byte[] bytes = Convert.FromBase64String(encodedLine);
                        string message = System.Text.Encoding.UTF8.GetString(bytes);
                        _history.Add(message);

                        // Stop if we've reached the maximum size
                        if (MaxHistorySize > 0 && _history.Count >= MaxHistorySize)
                            break;
                    } catch {
                        // Skip corrupted entries
                        continue;
                    }
                }

                // Trim the file only once during load if needed
                if (MaxHistorySize > 0 && needsTrimming) {
                    string[] trimmedLines = encodedLines.Skip(encodedLines.Length - MaxHistorySize).ToArray();
                    File.WriteAllLines(HistoryFilePath, trimmedLines);
                    Log.Trace($"Trimmed history file from {encodedLines.Length} to {trimmedLines.Length} entries");
                }
            }
        } catch (Exception ex) {
            Log.Error($"Failed to load history: {ex.Message}");
        }
    }

    public static void Prepend(string text) {
        if (string.IsNullOrEmpty(text))
            return;

        if (_history.Count > 0 && _history[0] == text)
            return;

        _history.Insert(0, text);

        // Trim in-memory history if needed
        if (MaxHistorySize > 0 && _history.Count > MaxHistorySize) {
            _history.RemoveAt(_history.Count - 1);
        }

        if (!ENV.PersistOnRestart.Value) {
            return;
        }

        // Save just the new entry
        SaveHistoryEntry(text);
    }

    public static void Initialize() {
        if (!ENV.PersistOnRestart.Value) {
            return;
        }

        var startTime = DateTime.Now;
        Log.Trace($"Loading history from {HistoryFilePath}");

        LoadHistory();
        _entryCounter = _history.Count;

        var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
        string limit = MaxHistorySize > 0 ? MaxHistorySize.ToString() : "∞";
        Log.Trace($"Loaded history with {_entryCounter} entries in {elapsed:F2}ms. Will trim if it exceeds {limit} entries.");
    }
}
