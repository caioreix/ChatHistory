# ChatHistory

A VRising plugin that enhances chat functionality by allowing players to navigate and manage their chat history.

## Features

- **Message History Navigation**: Browse through previously sent messages using arrow keys
- **Auto-completion**: Complete messages from your chat history with a single keypress
- **Custom KeyBinds**: Configure navigation keys to your preference
- **History Persistence**: Save the history entries locally

## Installation

- Install [BepInEx](https://thunderstore.io/c/v-rising/p/BepInEx/BepInExPack_V_Rising/)
- Extract [ChatHistory.dll](https://github.com/caioreix/ChatHistory/releases) into (VRising client folder)/BepInEx/plugins

## Usage

While the chat open and focused:

- Press **Up Arrow** to navigate up through your message history
- Press **Down Arrow** to navigate down through your message history
- Press **Right Arrow** to complete the currently suggested message

## Configuration

After running the plugin once, a configuration file will be created at:
`<VRising folder>/BepInEx/config/ChatHistory.cfg`

```
[0.⚙️ Settings]

## The maximum number of messages to keep in the chat history. If the number of messages exceeds this value, the oldest messages will be removed. If set to <= 0, the history will not be limited (use with caution).
# Setting type: Int32
# Default value: 50
MaximumHistorySize = 50

## If true, the chat history will be saved and loaded on restart. If false, the chat history will be lost on restart.
# Setting type: Boolean
# Default value: true
PersistOnRestart = true

## Down Arrow Key to scroll down the chat history.
# Setting type: KeyCode
# Default value: DownArrow
DownHistoryKey = DownArrow

## Up Arrow Key to scroll up the chat history.
# Setting type: KeyCode
# Default value: UpArrow
UpHistoryKey = UpArrow

## The key to complete the current chat message.
# Setting type: KeyCode
# Default value: RightArrow
CompleteHistoryKey = RightArrow
```

### KeyCodes

| KeyCodes     | KeyCodes          | KeyCodes     | KeyCodes          | KeyCodes          | KeyCodes          | KeyCodes          |
| ------------ | ----------------- | ------------ | ----------------- | ----------------- | ----------------- | ----------------- |
| None         | D                 | Insert       | JoystickButton0   | Joystick2Button7  | Joystick4Button15 | Joystick7Button0  |
| Backspace    | E                 | Home         | JoystickButton1   | Joystick2Button8  | Joystick4Button16 | Joystick7Button1  |
| Tab          | F                 | End          | JoystickButton2   | Joystick2Button9  | Joystick4Button17 | Joystick7Button2  |
| Clear        | G                 | PageUp       | JoystickButton3   | Joystick2Button10 | Joystick4Button18 | Joystick7Button3  |
| Return       | H                 | PageDown     | JoystickButton4   | Joystick2Button11 | Joystick4Button19 | Joystick7Button4  |
| Pause        | I                 | F1           | JoystickButton5   | Joystick2Button12 | Joystick5Button0  | Joystick7Button5  |
| Escape       | J                 | F2           | JoystickButton6   | Joystick2Button13 | Joystick5Button1  | Joystick7Button6  |
| Space        | K                 | F3           | JoystickButton7   | Joystick2Button14 | Joystick5Button2  | Joystick7Button7  |
| Exclaim      | L                 | F4           | JoystickButton8   | Joystick2Button15 | Joystick5Button3  | Joystick7Button8  |
| DoubleQuote  | M                 | F5           | JoystickButton9   | Joystick2Button16 | Joystick5Button4  | Joystick7Button9  |
| Hash         | N                 | F6           | JoystickButton10  | Joystick2Button17 | Joystick5Button5  | Joystick7Button10 |
| Dollar       | O                 | F7           | JoystickButton11  | Joystick2Button18 | Joystick5Button6  | Joystick7Button11 |
| Percent      | P                 | F8           | JoystickButton12  | Joystick2Button19 | Joystick5Button7  | Joystick7Button12 |
| Ampersand    | Q                 | F9           | JoystickButton13  | Joystick3Button0  | Joystick5Button8  | Joystick7Button13 |
| Quote        | R                 | F10          | JoystickButton14  | Joystick3Button1  | Joystick5Button9  | Joystick7Button14 |
| LeftParen    | S                 | F11          | JoystickButton15  | Joystick3Button2  | Joystick5Button10 | Joystick7Button15 |
| RightParen   | T                 | F12          | JoystickButton16  | Joystick3Button3  | Joystick5Button11 | Joystick7Button16 |
| Asterisk     | U                 | F13          | JoystickButton17  | Joystick3Button4  | Joystick5Button12 | Joystick7Button17 |
| Plus         | V                 | F14          | JoystickButton18  | Joystick3Button5  | Joystick5Button13 | Joystick7Button18 |
| Comma        | W                 | F15          | JoystickButton19  | Joystick3Button6  | Joystick5Button14 | Joystick7Button19 |
| Minus        | X                 | Numlock      | Joystick1Button0  | Joystick3Button7  | Joystick5Button15 | Joystick8Button0  |
| Period       | Y                 | CapsLock     | Joystick1Button1  | Joystick3Button8  | Joystick5Button16 | Joystick8Button1  |
| Slash        | Z                 | ScrollLock   | Joystick1Button2  | Joystick3Button9  | Joystick5Button17 | Joystick8Button2  |
| Alpha0       | LeftCurlyBracket  | RightShift   | Joystick1Button3  | Joystick3Button10 | Joystick5Button18 | Joystick8Button3  |
| Alpha1       | Pipe              | LeftShift    | Joystick1Button4  | Joystick3Button11 | Joystick5Button19 | Joystick8Button4  |
| Alpha2       | RightCurlyBracket | RightControl | Joystick1Button5  | Joystick3Button12 | Joystick6Button0  | Joystick8Button5  |
| Alpha3       | Tilde             | LeftControl  | Joystick1Button6  | Joystick3Button13 | Joystick6Button1  | Joystick8Button6  |
| Alpha4       | Delete            | RightAlt     | Joystick1Button7  | Joystick3Button14 | Joystick6Button2  | Joystick8Button7  |
| Alpha5       | Keypad0           | LeftAlt      | Joystick1Button8  | Joystick3Button15 | Joystick6Button3  | Joystick8Button8  |
| Alpha6       | Keypad1           | RightApple   | Joystick1Button9  | Joystick3Button16 | Joystick6Button4  | Joystick8Button9  |
| Alpha7       | Keypad2           | RightMeta    | Joystick1Button10 | Joystick3Button17 | Joystick6Button5  | Joystick8Button10 |
| Alpha8       | Keypad3           | RightCommand | Joystick1Button11 | Joystick3Button18 | Joystick6Button6  | Joystick8Button11 |
| Alpha9       | Keypad4           | LeftApple    | Joystick1Button12 | Joystick3Button19 | Joystick6Button7  | Joystick8Button12 |
| Colon        | Keypad5           | LeftMeta     | Joystick1Button13 | Joystick4Button0  | Joystick6Button8  | Joystick8Button13 |
| Semicolon    | Keypad6           | LeftCommand  | Joystick1Button14 | Joystick4Button1  | Joystick6Button9  | Joystick8Button14 |
| Less         | Keypad7           | LeftWindows  | Joystick1Button15 | Joystick4Button2  | Joystick6Button10 | Joystick8Button15 |
| Equals       | Keypad8           | RightWindows | Joystick1Button16 | Joystick4Button3  | Joystick6Button11 | Joystick8Button16 |
| Greater      | Keypad9           | AltGr        | Joystick1Button17 | Joystick4Button4  | Joystick6Button12 | Joystick8Button17 |
| Question     | KeypadPeriod      | Help         | Joystick1Button18 | Joystick4Button5  | Joystick6Button13 | Joystick8Button18 |
| At           | KeypadDivide      | Print        | Joystick1Button19 | Joystick4Button6  | Joystick6Button14 | Joystick8Button19 |
| LeftBracket  | KeypadMultiply    | SysReq       | Joystick2Button0  | Joystick4Button7  | Joystick6Button15 |                   |
| Backslash    | KeypadMinus       | Break        | Joystick2Button1  | Joystick4Button8  | Joystick6Button16 |                   |
| RightBracket | KeypadPlus        | Menu         | Joystick2Button2  | Joystick4Button9  | Joystick6Button17 |                   |
| Caret        | KeypadEnter       | Mouse0       | Joystick2Button3  | Joystick4Button10 | Joystick6Button18 |                   |
| Underscore   | KeypadEquals      | Mouse1       | Joystick2Button4  | Joystick4Button11 | Joystick6Button19 |                   |
| BackQuote    | UpArrow           | Mouse2       | Joystick2Button5  | Joystick4Button12 |                   |                   |
| A            | DownArrow         | Mouse3       | Joystick2Button6  | Joystick4Button13 |                   |                   |
| B            | RightArrow        | Mouse4       |                   | Joystick4Button14 |                   |                   |
| C            | LeftArrow         | Mouse5       |                   |                   |                   |                   |
|              |                   | Mouse6       |                   |                   |                   |                   |
```
