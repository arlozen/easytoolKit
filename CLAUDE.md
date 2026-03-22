# EasyToolkit Coding Standards for Claude Code

This document outlines the coding standards that must be followed when working with the EasyToolkit Unity project.

## Project Overview

EasyToolkit is a Unity framework/library project divided into multiple Unity packages. The codebase is written in C# and follows strict coding standards to maintain consistency and readability.

## Coding Standards

### Code Style

- **Language:** C# for Unity
- **Declaration Style:** Default to using `var` for local variables. Only use explicit types when emphasizing interface abstractions.
- **Indentation:** 4 spaces (no tabs)
- **Braces:** K&R style (opening brace on same line)
- **Line Length:** Soft limit 120 chars, hard limit 150 chars
- **Break Before Operator:** Prefer breaking before operators for clarity
- **Chained Calls:** One fluent method per line, starting with `.`

#### Line Wrapping Examples

```csharp
// ✅ Break before operator
var result = longExpression1
    + longExpression2
    * longExpression3;

var isValid = condition1
    && condition2
    || condition3;

// ✅ Chained methods
var player = new PlayerBuilder()
    .WithName("Hero")
    .WithLevel(10)
    .Build();

// ✅ Method parameters
var result = CalculateDamage(
    attacker,
    target,
    DamageType.Physical,
    includeCritBonus: true
);
```

### Example: Current Code Style

```csharp
// Use var by default
var player = new Player();
var damage = CalculateDamage(attacker, target);

// Use explicit types for interface abstractions
IList<Player> list = new List<Player>();
IEnumerable<Player> collection = GetPlayers();
```

---

## Naming Conventions

### Variable Naming

| Type | Convention | Example |
|------|------------|---------|
| Local variables | camelCase | `playerId`, `moveSpeed` |
| Private fields | _camelCase | `_playerId`, `_moveSpeed` |
| Properties | PascalCase | `PlayerId`, `MoveSpeed` |
| Constants | PascalCase | `MaxHealth`, `DefaultSpawn` |

#### Special Cases

**Collections:** Use plural form
```csharp
List<Player> players;
Dictionary<int, Enemy> enemies;
```

**Dictionaries:** Use `XxxByYyy` pattern (ValueByKey)
```csharp
Dictionary<int, Player> playerById;
Dictionary<string, Item> itemByName;
```

**TaskCompletionSource:** Use action semantic + `Tcs` suffix
```csharp
TaskCompletionSource<int> clickCompletedTcs;
TaskCompletionSource<bool> loginTcs;
```

**Events:** Use `Xxxing` / `Xxxed` (without `On` prefix)
```csharp
public event Action Closing;    // Window is closing
public event Action Closed;     // Window has closed
```

### Function Naming

| Prefix | Semantic | Use Case |
|--------|----------|----------|
| `Get` | Retrieve, throws if not found | O(1) lookup, expects existence |
| `Find` | Search, returns null if not found | May require search/traversal |
| `TryGet` | Safe retrieve, returns bool | Exception-safe version of Get |
| `Compute` | Derive through algorithm/logic | Complex multi-step reasoning |
| `Calculate` | Compute via formula | Mathematical calculation |
| `Can` | Check if operation is feasible | Condition checking |
| `Is` | Check state/property | State checking |

#### Function Suffixes

| Suffix | Semantic | Example |
|--------|----------|---------|
| `ByXxx` | "according to Xxx" | `GetPlayerById`, `FindItemByName` |
| `AtXxx` | "at Xxx position/time" | `GetItemAt`, `FindEnemyAt` |
| `OfXxx` | "of Xxx" / "has Xxx property" | `GetSkillOfPlayer`, `FindItemsOfType` |

### Delegate Naming

Use appropriate suffixes based on usage:

| Suffix | Semantic | Use Case |
|--------|----------|----------|
| `XxxHandler` | Process event/message | Event callbacks |
| `XxxInvoker` | Execute/invoke logic | Command execution |
| `XxxResolver` | Resolve/find value | Dependency resolution |
| `XxxFactory` | Create instance | Object instantiation |
| `XxxGetter` | Get value | Property/data access |
| `XxxConverter` | Convert type/format | Type conversion |
| `XxxPredicate` | Evaluate condition | Conditional filtering |
| `XxxEvaluator` | Calculate/evaluate result | Scoring, damage calc |

---

## Class Member Organization

### Unity Classes Order

1. Constants / Static fields
2. Serialized fields (`[SerializeField] private`)
3. Private fields
4. Public properties
5. Unity lifecycle methods (in order: Awake, OnEnable, Start, FixedUpdate, Update, LateUpdate, OnDisable, OnDestroy)
6. Public methods
7. Private methods
8. Coroutines

---

## Comment Standards

**Language**: Use English for all comments.

### XML Documentation Comments

Required for: All public types/members, important protected members, complex private members.

#### Basic Format

```csharp
/// <summary>
/// Brief description of what the class/method does.
/// </summary>
/// <param name="paramName">Description of parameter.</param>
/// <returns>Description of return value.</returns>
/// <remarks>
/// Additional information about usage, behavior, or implementation details.
/// </remarks>
/// <exception cref="ExceptionType">Description of when this exception is thrown.</exception>
```

#### Type References

Use `<see cref=""/>` to reference other types:

```csharp
/// <summary>
/// Converts a <see cref="List{T}"/> to a read-only <see cref="IEnumerable{T}"/>.
/// </summary>
/// <seealso cref="ArrayConverter"/>
public IEnumerable<T> ToReadOnly<T>(List<T> source) { }
```

#### Derived Members

Use `<inheritdoc/>` for overrides/interface implementations when behavior matches base:

```csharp
public class PlayerController : CharacterController<PlayerMotor, PlayerAnimator>
{
    /// <inheritdoc/>
    /// <remarks>
    /// Player-specific input handling using <see cref="GameInput.IGameplayActions"/>.
    /// </remarks>
    public override void ProcessInput() { }
}
```

#### XML Special Characters

Escape `<` and `>`: `&lt;` and `&gt;`

```csharp
/// <summary>
/// Compares two values and returns true if left &lt; right.
/// </summary>
public bool IsLessThan(int left, int right) { }
```

### Function Comment Guidelines

**Describe what the function does, not command it**

```csharp
// ✅ Good: Describes behavior
/// <summary>
/// Applies damage to the player and triggers death if health reaches zero.
/// </summary>

// ❌ Bad: Commands the function
/// <summary>
/// Apply damage to the player. Trigger death when health is zero.
/// </summary>
```

**Exception comments**: Only document exceptions users need to handle in normal usage. Skip defensive checks (null validation, type checks).

```csharp
// ✅ Document this - user may encounter
/// <exception cref="InsufficientFundsException">
/// Thrown when balance is less than transfer amount.
/// </exception>

// ❌ Don't document - defensive check
if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
```

### Variable Comments

**Private fields**: Add comments only when purpose isn't obvious from name

```csharp
// ✅ Needs comment - purpose not clear
// Cache of previously computed results to avoid redundant calculations
private readonly Dictionary<string, object> _cache;

// ❌ No comment needed - name is self-explanatory
private string _playerName;
```

**Public properties**: Always add XML documentation

```csharp
/// <summary>
/// Gets the maximum number of players supported by this server instance.
/// </summary>
public int MaxPlayers { get; }
```

### Implementation Comments

Explain **why**, not **what**:

```csharp
// ✅ Good: Explains reasoning
// Use a HashSet for O(1) lookups instead of List's O(n)
private readonly HashSet<string> _cachedItems;

// Clone the list to prevent modification during enumeration
var itemsCopy = _items.ToList();

// ❌ Bad: Just repeats the code
// Create a HashSet
private readonly HashSet<string> _cachedItems;
```

### Special Comment Tags

| Tag | Format | Purpose |
|-----|--------|---------|
| `TODO` | `TODO(username): Description` | Pending features or temporary solutions |
| `FIXME` | `FIXME(username): Description` | Known issues or bugs to fix |
| `HACK` | `HACK(username): Reason` | Non-elegant solutions with explanation |
| `NOTE` | `NOTE: Description` | Important implementation details |
| `DEPRECATED` | `[Obsolete]` + XML comment | Deprecated APIs |

### Logging and Type Printing

When printing types in logs, exceptions, or debug output, use framework extension methods:

```csharp
using EasyToolkit.Core.Reflection;

// For Type: use TypeExtensions.ToCodeString()
Debug.Error($"Invalid type: {type.ToCodeString()}");

// For MemberInfo: use ReflectionExtensions.ToCodeString()
Debug.Warning($"Member {memberInfo.ToCodeString()} is not accessible.");
```

---

## Member Variable Access Guidelines

### Basic Principle

In Unity classes, prefer **direct field access** internally, unless the property contains logic side effects (validation, events, lazy loading, etc.).

### Pure Data Properties

If properties are simple data wrappers without logic:
- Class internal: Use direct field access
- External access: Use properties

```csharp
public class PlayerData : MonoBehaviour
{
    [SerializeField] private int _playerId;

    public int PlayerId => _playerId;  // Pure data property

    private void Awake()
    {
        _playerId = 1001;  // ✅ Direct field access
        // PlayerId = 1001;  // ❌ Avoid property access for pure data
    }
}
```

### Logic Properties

If properties contain validation, events, or side effects:
- Must use property access internally to ensure logic executes

```csharp
public class HealthSystem : MonoBehaviour
{
    private float _currentHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }

    private void Awake()
    {
        CurrentHealth = _maxHealth;  // ✅ Must use property
        // _currentHealth = _maxHealth;  // ❌ Skips validation and events
    }
}
```

---

## Exception Guidelines

### Error Message Structure

`What happened? Why it happened? How to fix?`

```csharp
throw new InvalidOperationException(
    "Cannot attack while dead. Player health is 0. Revive player before attacking.");
```

### Custom Exception Naming

`XxxException`

```csharp
public class PlayerNotFoundException : Exception
{
    public int PlayerId { get; }

    public PlayerNotFoundException(int playerId)
        : base($"Player with ID {playerId} was not found.")
    {
        PlayerId = playerId;
    }
}
```

---

## Code Quality Principles

1. **Do not over-engineer** - Make only requested changes
2. **Avoid backwards compatibility hacks** - Delete unused code completely
3. **Security awareness** - Prevent injection vulnerabilities, validate at boundaries
4. **Avoid premature abstraction** - Three similar lines > premature abstraction
5. **No unnecessary features** - Don't add features/refactoring beyond what's asked

---

## Unity-Specific Guidelines

- **DO NOT create `.meta` files** - Unity automatically generates `.meta` files for all assets. Never manually create or include `.meta` files when writing new code or assets.
- Serialized fields should be `[SerializeField] private` with public properties for access
- Properties can include validation logic
- Organize Unity lifecycle methods in proper call order
- Use coroutines at the end of the class after all methods
- Prefer `_camelCase` for private fields with leading underscore

---

## Unity Testing Standards

### Organization
- **Location**: Each package has `Tests/Runtime/` and `Tests/Editor/` folders
- **Namespace**: `{TestedNamespace}.Tests` (e.g., `EasyToolkit.Serialization.Tests`)
- **File naming**: `Test[Category].[Feature].cs` (e.g., `TestSerializationBinary.cs`)

### Assembly Definition
- **Runtime tests**: `EasyToolkit.[ModuleName].Tests.asmdef`, `includePlatforms: []`
- **Editor tests**: `EasyToolkit.[ModuleName].Editor.Tests.asmdef`, `includePlatforms: ["Editor"]`
- Both require: `overrideReferences: true`, `defineConstraints: ["UNITY_INCLUDE_TESTS"]`

### Test Structure
- **Attributes**: `[TestFixture]` on classes, `[Test]` on methods
- **Pattern**: AAA (Arrange-Act-Assert) with `#region` grouping by feature
- **Method naming**: `MethodName_Scenario_ExpectedResult()` (e.g., `GetPlayerById_ValidId_ReturnsPlayer`)
- **Documentation**: XML comments on test classes and methods

### What to Test

| Test | Don't Test |
|------|------------|
| Public APIs and edge cases | Compiler guarantees |
| Boundary conditions (null, empty, min/max) | Simple getter/setters |
| Exception paths | Private implementation details |
| Properties with logic | Third-party library functions |
| State changes and events | Pure data classes |

### Best Practices

| Guideline | Description |
|-----------|-------------|
| **Independence** | Each test runs independently using `[SetUp]`/`[TearDown]` |
| **Single responsibility** | One behavior per test for easy debugging |
| **Meaningful data** | Use descriptive test data that expresses intent |
| **Avoid conditionals** | Split branched logic into separate tests |
| **Float tolerance** | `Assert.AreEqual(expected, actual, 0.001f)` |

### Assert Guidelines

```csharp
// Float comparison with tolerance
Assert.AreEqual(expected, actual, 0.001f);

// Exception testing
var ex = Assert.Throws<ArgumentOutOfRangeException>(() => obj.GetPlayerById(-1));
Assert.AreEqual("id", ex.ParamName);
```

---

## Architecture Design Standards

### Core Principles

- **Dependency Inversion**: User code depends on interfaces, not implementations
- **Layer Separation**: Abstractions (public) vs Implementations (internal `.Implementations` namespace)
- **Extension-Friendly**: Provide extension points through interfaces and abstract classes

### Module Structure

```
[Module]/
    ├── [Entry].cs              # Entry point
    ├── Abstractions/              # Public interfaces (IXxx.cs)
    ├── Implementations/           # Internal implementations (.Implementations namespace)
    ├── Models/                    # Public data structures
    └── Extensions/                # Extension methods for fluent APIs
```

### Key Patterns

- **Builder**: Fluent chain API for complex configuration (`WithXxx()` methods)
- **Strategy**: Interface-based replaceable algorithms

### Extension Methods

Two purposes:
1. **Chain APIs**: `WithXxx()` returns `this` for fluent configuration
2. **Encapsulation**: Wrap low-level APIs into strongly-typed helpers

### Design Principles

- **SOLID**: SRP, OCP, LSP, ISP, DIP
- **DRY/KISS/YAGNI**: Avoid duplication, keep simple, don't over-engineer

---

## Related Documentation

For detailed specifications, refer to:
- [命名规范](Documents/CodingStandards/命名规范.md)
- [类成员排列规范](Documents/CodingStandards/类成员排列规范.md)
- [成员变量访问层次原则](Documents/CodingStandards/成员变量访问层次原则.md)
- [代码风格指南](Documents/CodingStandards/代码风格指南.md)
- [注释规范](Documents/CodingStandards/注释规范.md)
- [日志和错误信息规范](Documents/CodingStandards/日志和错误信息规范.md)
- [框架架构规范](Documents/CodingStandards/框架架构规范.md)
