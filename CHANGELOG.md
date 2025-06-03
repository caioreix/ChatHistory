# Changelog

All notable changes to this project will be documented in this file.

## `1.1.1`
### Fixed
- An issue where the plugin did not properly unload by removing the `MonoBehaviour` using `UnityEngine.GameObject.Destroy`. This issue affected only cases where a hot reload was performed during gameplay. This fix was implemented by @ZeeWanderer. Thank you for your contribution!

## `1.1.0`
### Fixed
- Plugin now only starts in client world, preventing unintended activation in the server.
- History is saved only when input is submitted, avoiding unwanted entries from being recorded.

### Changed
- History is now saved as a URL-escaped string instead of base64, resolving Thunderstore upload rejections.
- History is now saved in the plugin's config folder instead of the plugins folder.

## `1.0.0`
### Added
- Support for history persistence, allowing history entries to be saved and restored.

## `0.0.1`
### Added
- Early preview release.
