---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "GitHub Copilot"
chat_id: "dotnet-coding-standards-20260211"
prompt: |
  Create .NET coding standards guide for PostHubAPI project covering naming conventions,
  async patterns, error handling, and best practices specific to ASP.NET Core.
started: "2026-02-11T00:00:00Z"
ended: "2026-02-11T00:30:00Z"
task_durations:
  - task: "coding standards documentation"
    duration: "00:30:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/11/dotnet-coding-standards-20260211/conversation.md"
source: "GitHub Copilot"
applyTo: "**/*.cs"
---

# .NET Coding Standards for PostHubAPI

## Overview

This document establishes coding standards and best practices for the PostHubAPI project. All code generated or modified must adhere to these standards.

**Target Audience**: Developers, code reviewers, and AI assistants
**Scope**: Naming conventions, async patterns, error handling, type safety, and C# idioms
**Framework**: .NET 8+, ASP.NET Core, C# 12+

**Related Documentation**:

- [PostHubAPI Architecture Guide](posthub-architecture.instructions.md)
- [AI-Assisted Output Instructions](ai-assisted-output.instructions.md)

## Table of Contents

- [Naming Conventions](#naming-conventions)
- [Async/Await Patterns](#asyncawait-patterns)
- [Type Safety and Nullability](#type-safety-and-nullability)
- [Exception Handling](#exception-handling)
- [LINQ Guidelines](#linq-guidelines)
- [Dependency Injection](#dependency-injection)
- [XML Documentation](#xml-documentation)
- [Code Style](#code-style)
- [Performance Considerations](#performance-considerations)
- [Security Best Practices](#security-best-practices)

## Naming Conventions

### Class and Interface Names

```csharp
// ✅ CORRECT: PascalCase for classes
public class UserController { }
public class UserService { }
public class ApplicationDbContext { }

// ✅ CORRECT: Interface names start with I
public interface IUserService { }
public interface IRepository { }
public interface ILogger { }

// ❌ WRONG: camelCase for classes
public class userController { }

// ❌ WRONG: Interface without I prefix
public class UserService { }  // Should be IUserService for interface
```

### Property and Field Names

```csharp
// ✅ CORRECT: PascalCase for public properties
public class Post
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ✅ CORRECT: camelCase with underscore prefix for private fields
public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
}

// ❌ WRONG: camelCase for public properties
public class Post
{
    public int id { get; set; }
    public string content { get; set; }
}

// ❌ WRONG: Inconsistent field naming
private IUserRepository userRepository;  // Missing underscore
private readonly IMapper mapper_;       // Wrong position
```

### Method and Parameter Names

```csharp
// ✅ CORRECT: PascalCase for methods, camelCase for parameters
public async Task<User> CreateUser(UserDto userDto)
{
    return await _userService.Create(userDto);
}

// ✅ CORRECT: Descriptive method names with action verbs
public async Task<bool> IsUserExists(string username)
public async Task DeleteUserById(int id)
public async Task<User> UpdateUser(int id, UpdateUserDto dto)

// ❌ WRONG: camelCase for methods
public async Task<User> createUser(UserDto userDto)

// ❌ WRONG: Non-actionable method names
public async Task GetData()           // What data?
public async Task Process()           // Process what?
public bool Check(string value)       // Check what?
```

### DTO Naming

```csharp
// ✅ CORRECT: Descriptive DTO names
public class CreateUserDto { }        // For POST requests
public class UpdateUserDto { }        // For PUT/PATCH requests
public class ReadUserDto { }          // For GET responses
public class LoginUserDto { }         // For authentication
public class ChangePasswordDto { }    // For specific operations

// ❌ WRONG: Generic or ambiguous names
public class UserDto { }              // Unclear purpose
public class RequestUser { }          // Not following convention
public class UserResponse { }         // Should be ReadUserDto
```

### Constant and Enum Names

```csharp
// ✅ CORRECT: UPPER_SNAKE_CASE for constants
public const int MAX_POST_LENGTH = 500;
public const string DEFAULT_SORT_ORDER = "CreatedAt";

// ✅ CORRECT: PascalCase for enum values
public enum UserRole
{
    Admin,
    Moderator,
    User,
    Guest
}

// ❌ WRONG: camelCase for constants
private const int maxPostLength = 500;

// ❌ WRONG: lowercase enum values
public enum UserRole
{
    admin,
    moderator,
    user
}
```

## Async/Await Patterns

### Async Method Naming

```csharp
// ✅ CORRECT: Async methods end with Async suffix
public async Task<User> GetUserAsync(int id)
public async Task<List<Post>> GetAllPostsAsync()
public async Task SaveChangesAsync()

// ✅ CORRECT: Return Task for void operations
public async Task DeleteUserAsync(int id)

// ✅ CORRECT: Return Task<T> for operations with results
public async Task<ReadUserDto> CreateUserAsync(CreateUserDto dto)

// ❌ WRONG: Missing Async suffix (confuses developers)
public async Task<User> GetUser(int id)

// ❌ WRONG: Using Task.Result (blocks thread)
var user = GetUserAsync(id).Result;

// ❌ WRONG: Combining sync and async
public User GetUser(int id)
{
    return GetUserAsync(id).Result;  // Never do this!
}
```

### Async Implementation Pattern

```csharp
// ✅ CORRECT: Proper async/await usage
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    
    public async Task<ReadUserDto> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
            throw new NotFoundException($"User {id} not found");
        
        return new ReadUserDto { /* Map properties */ };
    }
    
    public async Task<IEnumerable<ReadUserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .OrderBy(u => u.Username)
            .ToListAsync();
        
        return users.Select(u => new ReadUserDto { /* Map */ });
    }
}

// ✅ CORRECT: Async controller actions
[HttpPost]
public async Task<ActionResult<ReadUserDto>> CreateUser(
    [FromBody] CreateUserDto dto)
{
    var result = await _userService.CreateUserAsync(dto);
    return CreatedAtAction(nameof(GetUserById), 
        new { id = result.Id }, result);
}

// ❌ WRONG: Synchronous database access
public ReadUserDto GetUserById(int id)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == id);  // Blocks!
}

// ❌ WRONG: Not awaiting async call
public async Task<ReadUserDto> GetUserById(int id)
{
    var user = _userService.GetUserByIdAsync(id);  // Returns Task, not User!
}

// ❌ WRONG: Async void methods (except event handlers)
public async void CreateUserAsync(CreateUserDto dto)  // Never use async void!
{
    // This makes error handling nearly impossible
}
```

### ConfigureAwait Pattern

```csharp
// ✅ CORRECT: Use ConfigureAwait(false) in libraries
public async Task<User> GetUserAsync(int id)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id)
        .ConfigureAwait(false);
    
    return user;
}

// Note: In ASP.NET Core, ConfigureAwait is less critical due to
// synchronization context, but it's still good practice
```

## Type Safety and Nullability

### Nullable Reference Types

```csharp
// ✅ CORRECT: Enable nullable reference types in .csproj
// <PropertyGroup>
//   <Nullable>enable</Nullable>
// </PropertyGroup>

// ✅ CORRECT: Explicit nullable types
public class Post
{
    public int Id { get; set; }
    
    public string Content { get; set; }  // Non-nullable string
    public string? Description { get; set; }  // Nullable string
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }  // Nullable nullable types
    
    public User User { get; set; }  // Non-nullable reference
    public List<Comment>? Comments { get; set; }  // Nullable collection
}

// ✅ CORRECT: Null checks before use
public void ProcessPost(Post? post)
{
    if (post == null)
        throw new ArgumentNullException(nameof(post));
    
    var content = post.Content;  // Now safe to use
}

// ✅ CORRECT: Null-coalescing operator
public string GetPostTitle(Post? post)
{
    return post?.Content ?? "No content";
}

// ❌ WRONG: Missing null checks
public void ProcessPost(Post post)
{
    var content = post.Content;  // Could be null if not validated
}

// ❌ WRONG: Null-assertion operator (non-null assertion)
public string GetPostContent(Post? post)
{
    return post!.Content;  // Never use ! unless absolutely certain
}

// ❌ WRONG: Ignoring nullable warnings
#pragma warning disable CS8602
var length = post.Content.Length;  // Don't suppress warnings
#pragma warning restore CS8602
```

### Type Validation

```csharp
// ✅ CORRECT: Explicit type checking
public void ProcessEntity<T>(T? entity) where T : class
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity));
    
    if (entity is not Post post)
        throw new InvalidOperationException($"Expected Post, got {entity.GetType().Name}");
    
    // Now 'post' is definitely a Post
    DoSomethingWithPost(post);
}

// ✅ CORRECT: Generic type constraints
public class Repository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>()
            .FindAsync(id)
            .ConfigureAwait(false);
    }
}

// ❌ WRONG: Unchecked type casting
object entity = new Post();
var post = (Post)entity;  // Won't catch type mismatch at compile time

// ❌ WRONG: Using 'as' without null check
var post = entity as Post;
var content = post.Content;  // Could throw null reference
```

## Exception Handling

### Exception Patterns

```csharp
// ✅ CORRECT: Specific exception types
public async Task<User> GetUserAsync(int id)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == id);
    
    if (user == null)
        throw new NotFoundException($"User with ID {id} not found");
    
    return user;
}

// ✅ CORRECT: Null checks with meaningful exceptions
public UserService(IRepository repository, IMapper mapper)
{
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
}

// ✅ CORRECT: Proper exception handling in controllers
[HttpPost]
public async Task<ActionResult> DeleteUser(int id)
{
    try
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        // Log and return generic error
        return StatusCode(500, "An unexpected error occurred");
    }
}

// ❌ WRONG: Catching base Exception
try
{
    var user = GetUserAsync(id).Result;
}
catch (Exception)  // Too broad
{
    // Can't handle appropriately
}

// ❌ WRONG: Ignoring exceptions
public User GetUser(int id)
{
    try
    {
        return _context.Users.First(u => u.Id == id);
    }
    catch { }  // Silent failure
}

// ❌ WRONG: Throwing generic exceptions
if (user == null)
    throw new Exception("User not found");  // Use NotFoundException
```

### Custom Exception Pattern

```csharp
// ✅ CORRECT: Define custom exceptions in Exceptions folder
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// Usage
if (user == null)
    throw new NotFoundException($"User {id} not found");
```

## LINQ Guidelines

### Query Patterns

```csharp
// ✅ CORRECT: Method syntax for clarity
public async Task<ReadPostDto> GetPostAsync(int id)
{
    var post = await _context.Posts
        .Where(p => p.Id == id)
        .Select(p => new ReadPostDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt
        })
        .FirstOrDefaultAsync();
    
    return post ?? throw new NotFoundException($"Post {id} not found");
}

// ✅ CORRECT: Using Include for related entities
public async Task<List<ReadPostDto>> GetPostsWithCommentsAsync()
{
    var posts = await _context.Posts
        .Include(p => p.Comments)
        .Include(p => p.User)
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();
    
    return _mapper.Map<List<ReadPostDto>>(posts);
}

// ✅ CORRECT: Filtering before projection
public async Task<List<ReadPostDto>> GetUserPostsAsync(string userId)
{
    var posts = await _context.Posts
        .Where(p => p.UserId == userId)
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();
    
    return _mapper.Map<List<ReadPostDto>>(posts);
}

// ❌ WRONG: LINQ to Objects (N+1 queries)
var allPosts = _context.Posts.ToList();  // Fetches all
var userPosts = allPosts.Where(p => p.UserId == userId).ToList();  // Filtered in memory

// ❌ WRONG: Complex Where clauses without filtering first
var posts = _context.Posts
    .Include(p => p.Comments)
    .Where(p => p.User.Username == "john" && p.Comments.Count > 5)
    .ToListAsync();  // Inefficient query

// ❌ WRONG: Multiple database roundtrips
var posts = _context.Posts.ToList();
foreach (var post in posts)
{
    post.Comments = _context.Comments
        .Where(c => c.PostId == post.Id)
        .ToList();  // Query inside loop!
}
```

### Query Optimization

```csharp
// ✅ CORRECT: Explicit select to minimize data transfer
public async Task<List<PostSummaryDto>> GetPostSummariesAsync()
{
    return await _context.Posts
        .Select(p => new PostSummaryDto
        {
            Id = p.Id,
            Content = p.Content.Substring(0, 100),  // Summary only
            CreatedAt = p.CreatedAt
        })
        .ToListAsync();
}

// ❌ WRONG: Selecting entire entities when dto properties needed
public async Task<List<ReadPostDto>> GetPostsAsync()
{
    return await _context.Posts
        .Select(p => new ReadPostDto
        {
            Id = p.Id,
            Content = p.Content,
            User = _mapper.Map<ReadUserDto>(p.User)  // Loads too much data
        })
        .ToListAsync();
}
```

## Dependency Injection

### Service Registration

```csharp
// ✅ CORRECT: In Program.cs
// Register services with appropriate lifetimes
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
```

### Constructor Injection

```csharp
// ✅ CORRECT: Inject dependencies via constructor
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UserService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
}

// ❌ WRONG: Service locator pattern
private static IUserService _userService = ServiceLocator.GetService<IUserService>();

// ❌ WRONG: Static dependencies
public class UserService
{
    private static readonly IMapper Mapper = new Mapper(/*...*/);  // Hard to test
}
```

## XML Documentation

### Documentation Standards

```csharp
// ✅ CORRECT: Comprehensive XML documentation
/// <summary>
/// Creates a new user account with the provided registration data.
/// </summary>
/// <param name="dto">The registration data for the new user.</param>
/// <returns>A task that represents the asynchronous operation. 
/// The task result contains the read user DTO.</returns>
/// <exception cref="ArgumentException">Thrown when the email is already in use.</exception>
/// <exception cref="ArgumentNullException">Thrown when dto is null.</exception>
public async Task<ReadUserDto> RegisterUserAsync(RegisterUserDto dto)
{
    // Implementation
}

// ✅ CORRECT: Property documentation
/// <summary>
/// Gets or sets the user's email address. Must be unique in the system.
/// </summary>
public string Email { get; set; }

// ✅ CORRECT: Exception documentation
/// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
/// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permissions.</exception>

// ❌ WRONG: Missing documentation
public async Task<ReadUserDto> RegisterUserAsync(RegisterUserDto dto)
{
    // No documentation
}

// ❌ WRONG: Inaccurate documentation
/// <summary>
/// Gets users. (Too vague)
/// </summary>
```

## Code Style

### Code Formatting

```csharp
// ✅ CORRECT: Consistent formatting
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadUserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

// ✅ CORRECT: Use var for obvious types
var user = new User();
var userService = new UserService(_context, _mapper);

// ❌ WRONG: Inconsistent spacing
public class UserController:ControllerBase{
    private readonly IUserService _userService;
    public UserController(IUserService userService){_userService=userService;}
}

// ❌ WRONG: Not using var where clear
User user = new User();
UserService service = new UserService();
```

### Expression Bodied Members

```csharp
// ✅ CORRECT: For simple property getters
public int Age => DateTime.Now.Year - BirthYear;

// ✅ CORRECT: For simple methods
public bool IsActive() => Status == UserStatus.Active;

// ❌ WRONG: For complex logic (use regular methods)
public async Task<ReadUserDto> GetUserAsync(int id) => 
    await _context.Users.FirstOrDefaultAsync(u => u.Id == id) 
        ?? throw new NotFoundException($"User {id} not found");
```

## Performance Considerations

### Database Query Performance

```csharp
// ✅ CORRECT: Query optimization with indexes consideration
public async Task<List<ReadPostDto>> GetRecentPostsAsync(int count = 10)
{
    var posts = await _context.Posts
        .OrderByDescending(p => p.CreatedAt)
        .Take(count)  // Limit results
        .Select(p => new ReadPostDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt
            // Only select needed fields
        })
        .ToListAsync();
    
    return posts;
}

// ❌ WRONG: Loading all data then filtering
var allPosts = await _context.Posts.ToListAsync();
var recent = allPosts.OrderByDescending(p => p.CreatedAt).Take(10);
```

### Connection Pooling

```csharp
// ✅ CORRECT: Connection pooling configured by default in EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    // Pooling is enabled by default
);
```

## Security Best Practices

### Input Validation

```csharp
// ✅ CORRECT: Validate all user input
[HttpPost]
public async Task<ActionResult<ReadPostDto>> CreatePost(
    [FromBody] CreatePostDto dto)
{
    // Validation attributes handle validation
    // ModelState.IsValid checked automatically
    if (!ModelState.IsValid)
        return BadRequest(ModelState);
    
    var result = await _postService.CreatePostAsync(dto);
    return CreatedAtAction(nameof(GetPostById), 
        new { id = result.Id }, result);
}

// ✅ CORRECT: Additional business validation in service
public async Task<ReadPostDto> CreatePostAsync(CreatePostDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Content))
        throw new ArgumentException("Content cannot be empty.");
    
    // Create post...
}

// ❌ WRONG: Trusting user input
var post = new Post { Content = userInput };  // No validation!
```

### SQL Injection Prevention

```csharp
// ✅ CORRECT: LINQ protects against SQL injection
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Username == username);  // Parameterized

// ❌ WRONG: String concatenation (never do this)
var query = $"SELECT * FROM Users WHERE Username = '{username}'";  // SQL injection risk!
```

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-11
**Next Review**: 2026-05-11
**Owner**: Development Team
