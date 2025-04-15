# Overall Design

FSD (Feature-Sliced Design):

- `/app`
  App initialization logic.
- `/entities`
  Business entities.
- `/features`
  User stories.
- `/pages`
  Application pages. Should not contain complex logic, just aggregate components into pages.
- `/shared`
  Shared objects, no business logic.

Each layer may use layers below it and should avoid using layers above it.

---

PWA

Internalization (i18n)

# Using VSCode

Using VSCode, install the following extensions for the best experience:

- Angular Language Service
- EditorConfig for VS Code
- Prettier - Code formatter
- Tailwind CSS IntelliSense

# Style Guides

When writing in TypeScript, stick to [Google TypeScript Style Guide](https://google.github.io/styleguide/tsguide.html).
