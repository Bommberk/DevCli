# System & Game Manager – Changelog

### 🛠 Code Logic Refactoring
- **Path Resolution Fix:** Updated `Patchnotes.cs` to use `AppContext.BaseDirectory` instead of `Directory.GetCurrentDirectory()` when loading the prompt template. This ensures consistent behavior across different execution contexts (e.g., debugging vs. production).
- **Git Integration Stability:** Maintained existing git diff generation logic while correcting file path dependencies for markdown templates.

### 📁 Project Configuration & Hygiene
- **Typo Correction:** Renamed `patchnotes-promt.md` to `patchnotes-prompt.md` in `devCli.csproj`. This fixes the project-level typo and ensures correct case-sensitivity matching on Linux/macOS systems.
- **Asset Handling Consistency:** Kept `<CopyToOutputDirectory>` settings as `PreserveNewest` for both text files, ensuring updated prompts are picked up without rebuilding the entire project unnecessarily.