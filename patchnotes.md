# System & Game Manager v<version>

## ✨ New Features
- (No new user-facing features detected in this patch)

## 🎨 User Interface
- (No UI changes detected based solely on this commit history)

## ⚡ Improvements
- Enhanced the Large Language Model processing limits to support more complex prompts and extended context windows.

## 🐞 Bug Fixes
- Resolved inconsistent behavior when loading prompt templates by switching from current working directory execution logic to a robust application base path detection method, ensuring stability across development and production environments.
- Corrected file asset dependencies within the project configuration to ensure case-sensitive matches work correctly on Linux/macOS systems (fixes filename typo).

## 🧹 Internal
- Increased token limit capacity for AI interactions from 32K to 64K tokens.