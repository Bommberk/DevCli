# Changelog

### 🧹 Code Hygiene & Refactoring
- **Variable Management:** Introduced `MAX_PROMPT_TOKENS` constant in `OllamaService.cs`, centralizing context limit configuration and removing hardcoded magic numbers.
- **Naming Consistency:** Corrected typo in console warning messages ("Promt" → "Prompt") for improved code readability.

### 🖥️ Enhanced Debugging & Logging (OllamaService)
- **Context Transparency:** Added specific logging output for `Context Size` and token usage percentage during LLM inference to monitor resource consumption.
- **Dynamic Configuration:** Updated Ollama request options to utilize the new configuration constant instead of static values, ensuring consistency across configurations.

### 📦 Asset & Data Handling
- **Prompt Construction:** Improved prompt building logic in `Patchnotes.cs` to correctly concatenate additional input with base prompts when provided.