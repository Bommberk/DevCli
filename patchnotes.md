Generate professional release notes based on the provided git diff.

## General Guidelines
- Write user-focused release notes, not commit logs.
- Describe what changed and how it benefits the user.
- Only include changes that can be inferred from the git diff.
- Do not invent features, fixes, or improvements.
- Keep the wording clear, concise, and professional.
- Use Markdown formatting.
- Use emojis only for section headers.
- Write in fluent English.

## Content Guidelines
- Avoid mentioning implementation details such as:
  - class names
  - methods
  - variables
  - namespaces
  - file names
  - folder names
  - project structure
  - internal APIs
- Instead, describe the visible result or improvement.
- Merge related commits into a single bullet point whenever possible.
- Skip insignificant changes such as formatting, renaming, or trivial code cleanup unless they noticeably improve maintainability or stability.
- If a change is purely internal, summarize it in one short bullet.

## Section Order
1. ✨ New Features
2. 🎨 User Interface
3. ⚡ Improvements
4. 🐞 Bug Fixes
5. 🧹 Internal

Only include sections that contain relevant changes.

## Output Format

# System & Game Manager v<version>

## ✨ New Features
- ...

## 🎨 User Interface
- ...

## ⚡ Improvements
- ...

## 🐞 Bug Fixes
- ...

## 🧹 Internal
- ...

## Style
- Prefer "Added", "Improved", "Fixed", "Updated", "Redesigned", "Enhanced".
- Keep each bullet to one or two sentences.
- Avoid repeating the same information in multiple sections.
- Focus on outcomes instead of implementation.