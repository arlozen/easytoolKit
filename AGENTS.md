## High-Level Working Rules

- Follow existing code patterns before introducing new abstractions.
- Keep module boundaries clean. Do not place gameplay logic into framework code or presentation code into entity code.
- Prefer extending an existing module over creating a new pattern in an unrelated folder.
- This repository is a Unity project used to develop and validate EasyToolKit packages. Package source lives under `Packages/`.
- Each EasyToolKit package under `Packages/` is a git submodule. Treat package changes as changes to the corresponding package repository.
- Do not manually create or edit Unity `.meta` files.
- Treat generated files as generated. If a file is auto-generated, edit the source asset or generator input instead of hand-editing generated output unless the user explicitly asks otherwise.
- When adding public APIs, include English XML documentation comments.
- Use English for code comments.
- Keep framework and reusable infrastructure language-neutral and business-agnostic.

## Repository And Package Boundaries

- `Packages/com.easytoolkit.core`: Core reusable APIs and Unity/editor utility infrastructure. Keep it broadly useful, language-neutral, and independent from higher-level packages.
- `Packages/com.easytoolkit.inspector`: Unity Inspector and editor tooling. Put inspector attributes, drawers, visual processors, and editor integration here.
- `Packages/com.easytoolkit.logging`: Logging abstractions, logger configuration, log event models, and log sinks. Keep logging concerns here instead of leaking them into Core.
- `Packages/com.easytoolkit.serialization`: Serialization infrastructure, processors, formatters, and serializers for byte streams, JSON, YAML, and XML.
- `Assets/` is the host Unity project area. Do not put reusable package implementation in `Assets/` unless the user explicitly asks for project-only examples or validation assets.
- `Documents/` contains documentation and reference material. Do not treat it as package runtime source.

### Package Dependency Direction

- Core should not depend on Inspector, Logging, or Serialization.
- Serialization should remain independent unless a package dependency is intentionally added and reflected in `package.json`.
- Inspector may depend on Core for shared editor/runtime utilities.
- Logging may depend on Core and Serialization.
- Avoid circular package dependencies. If two packages need the same helper, prefer moving the neutral helper into Core.
- When changing package dependencies, update the relevant `package.json` and verify matching `.asmdef` references.

### Runtime, Editor, And Tests

- Put runtime code in `Runtime/`, editor-only code in `Editor/`, and tests in `Tests/Runtime` or `Tests/Editor`.
- Do not reference `UnityEditor` from Runtime assemblies. Editor functionality belongs in `Editor/` assemblies.
- Keep runtime attributes or marker types that are consumed by editor tooling in `Runtime/`; keep drawing, reflection UI, and inspector rendering behavior in `Editor/`.
- Add or update tests inside the package that owns the behavior.
- Prefer package-local asmdefs and existing test assembly patterns over creating root-level assemblies.

### Package Metadata And Submodules

- Do not hand-edit Unity `.meta` files.
- Keep package identity consistent across `package.json`, asmdef names, namespaces, and folder names.
- Root `.csproj` and `.sln` files are Unity-generated. Do not make durable source changes there unless the user explicitly asks.
- Because packages are submodules, check package-local git status when preparing commits or summaries.

## Module-Specific Guidance

### Core

- Keep Core focused on reusable primitives, extensions, utilities, reflection helpers, threading helpers, and Unity/editor infrastructure that can serve multiple packages.
- Do not add business-specific workflows, logging policy, serializer formats, or inspector presentation rules to Core.
- If a Core API is public, document it carefully because downstream packages treat it as shared infrastructure.

### Inspector

- Keep inspector-facing attributes lightweight and safe for Runtime assemblies.
- Put drawer logic, visual processors, editor windows, serialized property handling, and GUI behavior in Editor code.
- Prefer Unity's serialized property patterns and existing EasyToolKit editor utilities before introducing new reflection or drawing systems.
- When adding inspector features, cover both the attribute/model contract and the editor rendering behavior when practical.

### Logging

- Keep logger abstractions, configuration, levels, event models, and sink composition inside Logging.
- Do not make Core depend on Logging for diagnostics. Shared infrastructure should stay usable without a logging package dependency.
- Serialization use in Logging should remain limited to log context or event payload formatting concerns.
- Prefer adding new sinks through existing sink abstractions and configuration extension patterns.

### Serialization

- Serialization owns byte stream serialization plus JSON, YAML, and XML serialization support.
- Keep format-specific code in format-specific serializers, formatters, or contexts. Shared processor behavior should stay format-neutral.
- Prefer processors for type-specific behavior rather than special-casing types inside serializers.
- Keep primitive, system, collection, and Unity processors organized under the existing processor folders.
- When adding a new format capability, maintain equivalent read/write coverage and tests where the existing format pattern expects both directions.

## Coding Standards

- Language: C# for Unity
- Use `var` for local variables by default. Use explicit types when the abstraction should be emphasized.
- Indentation: 4 spaces, no tabs
- Braces: K&R style
- Soft line limit: 120 characters
- Hard line limit: 150 characters
- Prefer breaking before operators when wrapping long expressions
- For fluent chains, place one chained call per line starting with `.`

## Naming Conventions

- Local variables: `camelCase`
- Private fields: `_camelCase`
- Properties: `PascalCase`
- Constants: `PascalCase`
- Collections: plural nouns such as `players`
- Dictionaries: `ValueByKey` style such as `playerById`
- `TaskCompletionSource` fields: action semantic plus `Tcs`
- Events: `Xxxing` or `Xxxed` without `On` prefix

### Function Naming

- `Get`: retrieve and expect existence
- `Find`: search and allow not found
- `TryGet`: safe retrieval with boolean success
- `Compute`: derive through logic or algorithm
- `Calculate`: compute via formula
- `Can`: check whether an operation is feasible
- `Is`: check state or property

### Common Suffixes

- `ByXxx`
- `AtXxx`
- `OfXxx`

### Type Naming

- Exceptions: `XxxException`
- Event args: `XxxEventArgs`
- Delegates should use a semantic suffix such as `Handler`, `Factory`, `Resolver`, `Predicate`, or `Evaluator`

## Class Member Order

- Arrange members by responsibility first, then by visibility. Show the public contract before implementation details.
- Within the same group, prefer this visibility order: `public` -> `protected` -> `internal` -> `private`.

### Regular Classes

Prefer this top-to-bottom order for non-Unity classes:

1. Constants (`const`)
2. Static readonly fields
3. Static fields
4. Instance fields
5. Constructors
6. Properties and events
7. Public methods
8. Protected methods
9. Explicit interface implementations
10. Private methods
11. Local helper types

- Keep explicit interface implementations as a separate group after public and protected members, and before private implementation details.
- Do not interleave explicit interface implementations with same-name public APIs.

### Unity Classes

For Unity classes, always keep this fixed preamble:

1. Constants (`const`)
2. Static readonly fields
3. Static fields
4. Serialized fields (`[SerializeField] private`)
5. Non-serialized instance fields
6. Properties and events

After that preamble, choose the template that matches the component's primary usage:

- Self-driven components:
  1. Unity lifecycle methods
  2. Public methods
  3. Protected methods
  4. Explicit interface implementations
  5. Private methods
  6. Coroutines
- External API components:
  1. Public methods
  2. Protected methods
  3. Unity lifecycle methods
  4. Explicit interface implementations
  5. Private methods
  6. Coroutines

- Use the self-driven template when the main entry points are lifecycle methods such as `Awake`, `Update`, or `OnEnable`.
- Use the external API template when outside callers primarily drive the component through public methods and lifecycle methods mainly support initialization, registration, or cleanup.
- Keep Unity lifecycle methods in engine call order: `Reset`, `OnValidate`, `Awake`, `OnEnable`, `Start`, `FixedUpdate`, `Update`, `LateUpdate`, `OnApplicationFocus`, `OnApplicationPause`, `OnApplicationQuit`, `OnDisable`, `OnDestroy`.
- Omit empty groups instead of leaving placeholders.
- Preserve a clear and stable local pattern when nearby files already follow one of these templates.

## Comments And Documentation

- Use English for comments.
- Add XML documentation for all public types and public members.
- Add XML documentation for important protected members.
- Add XML documentation for complex private members when their purpose is not obvious.
- Public methods should document `summary`, every `param`, and `returns` for non-void members; add `remarks` only when extra behavior or usage notes matter.
- Use `<exception>` only for exceptions callers may reasonably encounter and handle during normal use. Do not list defensive guard exceptions such as null, range, or type checks unless they are part of the intended contract.
- Prefer `<see cref="..."/>` and `<inheritdoc/>` to avoid duplicated prose, and escape XML special characters such as `&lt;` and `&gt;` in documentation comments.
- Use `<inheritdoc/>` when behavior matches the base type or interface contract.
- Prefer comments that explain why, not what.
- Describe behavior in statements, not instructions. Focus on what the code does and why a choice exists, not how the implementation is spelled out line by line.
- Keep implementation comments for complex logic, business rules, or important tradeoffs. Update or remove stale comments together with the code.
- Only comment private fields when the name alone is insufficient.

### Special Comment Tags

- `TODO(username): ...`
- `FIXME(username): ...`
- `HACK(username): ...`
- `NOTE: ...`

## Property And Field Access

- Inside Unity classes, prefer direct field access for pure data properties.
- If a property performs validation, event dispatch, lazy initialization, or any side effect, use the property internally as well.

## Unity-Specific Guidance

- Serialized fields should be `[SerializeField] private`
- Prefer public properties for controlled external access
- Keep lifecycle code in the standard Unity order
- Put coroutine methods at the end of the class
- Respect inspector-facing serialization patterns already used in adjacent files

## Verification

- Prefer Unity Test Framework tests for package behavior. Use Runtime tests for runtime APIs and Editor tests for editor-only behavior.
- When possible, run the narrowest relevant test assembly before broad validation.
- For package dependency or asmdef changes, let Unity regenerate project files and verify the affected assemblies compile.
- For serialization changes, test representative primitive, system, collection, Unity, and custom object cases that touch the changed processor or format.
- For inspector changes, verify editor tests or manual inspector behavior when UI rendering is involved.
