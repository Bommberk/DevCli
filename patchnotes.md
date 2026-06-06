# 🚀 Development CLI – Patch Notes ([Version Tag])

> **Release Type:** Major Infrastructure & AI Integration Update

We have completely overhauled the core service architecture to improve debugging workflows, enhance error handling for our LLM integrations, and add robust versioning support.

### ✨ New Features
*   **🛠️ Enhanced `GlobalFunctions`:** Replaced legacy logging with a comprehensive utility suite.
    *   Added typed loggers (`ConsoleLog`, `ConsoleError`, `ConsoleWarning`).
    *   Introduced dynamic debug helpers: `slog()` (separator), `clog()` (call count tracker), and `mlog()` (custom messages).
*   **🧐 Deep Debugging:** New object dumper functionality allows recursive inspection of complex objects with indentation support.
*   **💾 File Utilities:** Added direct file writing capabilities (`writeFile`) to output logs or intermediate data directly to disk.

### 🤖 AI Integration Improvements (Ollama Service)
*   **Robust Error Handling:** Wrapped LLM calls in `try/catch` blocks to gracefully handle network failures and API errors, returning descriptive error messages instead of crashing the app.
*   **Thinking Model Support:** Added support for "thinking" models via a new JSON property capture (`Thinking`).
*   **Raw Response Logging:** Automatically saves raw AI responses to `ollama_raw_response.json` for analysis.
*   **Input Concatenation Logic:** Improved prompt construction logic when additional input is provided alongside the base system prompt.

### 🔧 Under-the-Hood & Fixes
*   **Architecture Refactor:** Consolidated logging and utility classes into a new dedicated namespace (`DevCli.Functions`) to avoid polluting global scopes unnecessarily.
*   **Git Versioning Logic:** Implemented automatic detection of the latest Git tag using `git describe --tags`. The tool now generates diffs relative to this specific version anchor, ensuring accurate changelogs per release cycle.
*   **Input Validation:** Added checks for empty git diff outputs and missing repository paths before attempting AI generation or file operations.

### 📝 Developer Experience (DX)
*   **Better CLI Feedback:** Console output now clearly distinguishes between success logs, warnings, and critical errors with color coding.
*   **Safe Defaults:** `die()` method ensures immediate exit on fatal internal logic checks without requiring verbose messages every time.