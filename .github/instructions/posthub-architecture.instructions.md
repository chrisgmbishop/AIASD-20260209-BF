---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "GitHub Copilot"
chat_id: "posthub-architecture-20260211"
prompt: |
  Create a comprehensive architecture guide for the PostHubAPI project,
  covering the layered architecture, code organization, and implementation guidelines.
started: "2026-02-11T00:00:00Z"
ended: "2026-02-11T00:30:00Z"
task_durations:
  - task: "architecture analysis and documentation"
    duration: "00:30:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/11/posthub-architecture-20260211/conversation.md"
source: "GitHub Copilot"
applyTo: "**/*.cs"
---

# PostHubAPI Architecture Guide

## Overview

PostHubAPI follows a **layered architecture pattern** with clear separation of concerns across Controllers, Services, and Data layers. This guide ensures consistent implementation and maintainability across the codebase.

**Target Audience**: Developers generating or refactoring code for PostHubAPI
**Scope**: Architectural patterns, code organization, implementation guidelines specific to this project
**Stack**: .NET 8+, ASP.NET Core, Entity Framework Core, C#

**Related Documentation**:

- [AI-Assisted Output Instructions](ai-assisted-output.instructions.md)
- [.NET Coding Standards](dotnet-coding-standards.instructions.md)

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Layer Responsibilities](#layer-responsibilities)
- [Controller Implementation](#controller-implementation)
- [Service Implementation](#service-implementation)
- [DTO Standards](#dto-standards)
- [Entity Models](#entity-models)
- [Data Access Patterns](#data-access-patterns)
- [Error Handling](#error-handling)
- [Code Generation Rules](#code-generation-rules)
- [Quality Checklist](#quality-checklist)

## Architecture Overview

PostHubAPI implements a three-layer architecture:

```
┌─────────────────────────────────┐
│     API Layer (Controllers)      │  HTTP/REST Interface
├─────────────────────────────────┤
│    Business Logic (Services)     │  Domain Logic & Orchestration
├─────────────────────────────────┤
│   Data Access (DbContext)        │  Database Operations
├─────────────────────────────────┤
│    Entity Models & DTOs          │  Data Contracts
└─────────────────────────────────┘
```

### Core Principles

1. **Unidirectional Dependency Flow**: Controllers → Services → Data, never reverse
2. **Separation of Concerns**: Each layer has distinct responsibilities
3. **Interface-Based Dependencies**: Services expose interfaces for testability
4. **DTO Boundary Pattern**: DTOs separate API contracts from internal models
5. **Async-First**: All I/O operations use async/await patterns

## Project Structure

```
PostHubAPI/
├── Controllers/                    # HTTP endpoint definitions
│   ├── UserController.cs
│   ├── PostController.cs
│   └── CommentController.cs
├── Services/                       # Business logic layer
│   ├── Interfaces/
│   │   ├── IUserService.cs
│   │   ├── IPostService.cs
│   │   └── ICommentService.cs
│   └── Implementations/
│       ├── UserService.cs
│       ├── PostService.cs
│       └── CommentService.cs
├── Models/                         # Domain entities
│   ├── User.cs
│   ├── Post.cs
│   └── Comment.cs
├── Dtos/                           # Data transfer objects
│   ├── User/
│   │   ├── RegisterUserDto.cs
│   │   ├── LoginUserDto.cs
│   │   └── ReadUserDto.cs
│   ├── Post/
│   │   ├── CreatePostDto.cs
│   │   ├── EditPostDto.cs
│   │   └── ReadPostDto.cs
│   └── Comment/
│       ├── CreateCommentDto.cs
│       ├── EditCommentDto.cs
│       └── ReadCommentDto.cs
├── Data/                           # Data access layer
│   └── ApplicationDbContext.cs
├── Exceptions/                     # Custom exceptions
│   └── NotFoundException.cs
├── Profiles/                       # AutoMapper configurations
│   ├── UserProfile.cs
│   ├── PostProfile.cs
│   └── CommentProfile.cs
├── Program.cs                      # Application startup & DI
├── appsettings.json               # Configuration
└── PostHubAPI.csproj              # Project file
```

## Layer Responsibilities

### Controllers Layer

**Responsibility**: HTTP request routing and response formatting

**Patterns**:

- Receive HTTP requests
- Validate HTTP-level constraints
- Call service methods
- Return appropriate HTTP status codes
- Handle cross-cutting concerns (auth, logging)

**Implementation Rules**:

```csharp
[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    // ✅ CORRECT: Async endpoint with proper error handling
    [HttpPost]
    public async Task<ActionResult<ReadPostDto>> CreatePost(
        [FromBody] CreatePostDto dto)
    {
        try
        {
            var result = await _postService.CreatePost(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                dto);
            return CreatedAtAction(nameof(GetPostById), 
                new { id = result.Id }, result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

**Do NOT**:

- ❌ Perform business logic in controllers
- ❌ Mix synchronous and asynchronous patterns
- ❌ Return domain models directly (use DTOs)
- ❌ Catch exceptions without logging
- ❌ Call multiple services without orchestration

### Services Layer

**Responsibility**: Business logic and orchestration

**Patterns**:

- Implement interfaces for testability
- Perform domain validation and invariant checks
- Coordinate multiple data access operations
- Handle domain-level errors
- Return DTOs to controllers

**Implementation Rules**:

```csharp
public interface IPostService
{
    Task<ReadPostDto> CreatePost(string userId, CreatePostDto dto);
    Task<ReadPostDto> GetPostById(int id);
    Task<IEnumerable<ReadPostDto>> GetAllPosts();
    Task<ReadPostDto> UpdatePost(int id, EditPostDto dto);
    Task DeletePost(int id);
}

public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public PostService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // ✅ CORRECT: Async service method with validation
    public async Task<ReadPostDto> CreatePost(string userId, CreatePostDto dto)
    {
        // Validate input
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("User ID is required.");
        
        if (string.IsNullOrEmpty(dto.Content))
            throw new ArgumentException("Post content cannot be empty.");
        
        // Create domain model
        var post = new Post
        {
            UserId = userId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };
        
        // Persist and return
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ReadPostDto>(post);
    }
    
    // ✅ CORRECT: Query with filtering and mapping
    public async Task<IEnumerable<ReadPostDto>> GetAllPosts()
    {
        var posts = await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<ReadPostDto>>(posts);
    }
}
```

**Do NOT**:

- ❌ Accept HTTP types (IActionResult, HttpContext)
- ❌ Mix database queries with business logic
- ❌ Return domain entities directly
- ❌ Throw HTTP exceptions (use domain exceptions)

### Data Layer

**Responsibility**: Database access and persistence

**Patterns**:

- DbContext manages entity operations
- LINQ queries are lazy (evaluated by controller/service)
- SaveChangesAsync commits transactions
- Use async patterns throughout

**Implementation Rules**:

```csharp
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure relationships
        builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

## Controller Implementation

### Endpoint Routing Convention

```csharp
[ApiController]
[Route("api/[controller]")]
public class EntityController : ControllerBase
{
    private readonly IEntityService _entityService;
    
    public EntityController(IEntityService entityService)
    {
        _entityService = entityService;
    }
}
```

**URL Patterns**:

```
GET    /api/entity              - List all
GET    /api/entity/{id}         - Get by ID
POST   /api/entity              - Create
PUT    /api/entity/{id}         - Update full
PATCH  /api/entity/{id}         - Update partial
DELETE /api/entity/{id}         - Delete
```

### HTTP Status Codes

**Map to standard codes**:

- `200 OK`: Successful GET, PUT, or operation returns data
- `201 Created`: POST successfully created resource
- `204 No Content`: DELETE successful
- `400 Bad Request`: Invalid input validation errors
- `401 Unauthorized`: Authentication required
- `403 Forbidden`: Authorization failed
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Unhandled exceptions

### Response Structure

```csharp
// Single resource
{ "id": 1, "name": "John", ... }

// Collection
[
    { "id": 1, "name": "John", ... },
    { "id": 2, "name": "Jane", ... }
]

// Error response
{
    "error": "Resource not found",
    "statusCode": 404
}
```

## Service Implementation

### Service Initialization

Implement both interface and concrete class:

```csharp
// Services/Interfaces/IEntityService.cs
public interface IEntityService
{
    Task<ReadEntityDto> CreateEntity(CreateEntityDto dto);
    Task<ReadEntityDto> GetEntityById(int id);
    Task<IEnumerable<ReadEntityDto>> GetAllEntities();
    Task<ReadEntityDto> UpdateEntity(int id, EditEntityDto dto);
    Task DeleteEntity(int id);
}

// Services/Implementations/EntityService.cs
public class EntityService : IEntityService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public EntityService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // Implement all methods from interface
}
```

### Register Services in Program.cs

```csharp
// In Program.cs, before build:
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
```

## DTO Standards

### DTO Naming Convention

```
[Operation][Entity]Dto

Create    -> CreatePostDto        (input from client)
Edit      -> EditPostDto          (input from client for updates)
Read      -> ReadPostDto          (output to client)
LoginUser -> LoginUserDto         (input for auth)
```

### DTO Design Rules

```csharp
// ✅ CORRECT: DTOs with validation attributes
public class CreatePostDto
{
    [Required(ErrorMessage = "Content is required")]
    [StringLength(500, MinimumLength = 1, 
        ErrorMessage = "Content must be 1-500 characters")]
    public string Content { get; set; }
}

public class ReadPostDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ❌ WRONG: DTOs without validation
public class CreatePostDto
{
    public string Content { get; set; }  // No validation
}

// ❌ WRONG: Using domain models as DTOs
public class GetPostResponse : Post  // Should not inherit from domain model
{
    // Properties
}
```

### Validation in DTOs

```csharp
public class CreateCommentDto
{
    [Required]
    public string Content { get; set; }
    
    [Range(1, int.MaxValue)]
    public int PostId { get; set; }
}

public class EditCommentDto
{
    [Required]
    [StringLength(500)]
    public string Content { get; set; }
}
```

**Model validation occurs automatically in controller**:

```csharp
[HttpPost]
public async Task<ActionResult<ReadCommentDto>> CreateComment(
    [FromBody] CreateCommentDto dto)  // Automatically validated
{
    // If validation fails, 400 Bad Request returned automatically
    var result = await _commentService.CreateComment(dto);
    return CreatedAtAction(nameof(GetCommentById), 
        new { id = result.Id }, result);
}
```

## Entity Models

### Model Design Rules

```csharp
// ✅ CORRECT: Domain model with proper structure
public class Post
{
    public int Id { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

// ❌ WRONG: Model with HTTP concerns
public class Post
{
    public IActionResult Result { get; set; }  // Don't put HTTP types in models
    [NotMapped]
    public string ApiVersion { get; set; }      // Don't mix concerns
}
```

## Data Access Patterns

### Query Patterns

```csharp
// ✅ CORRECT: Async queries with proper filtering
public async Task<ReadPostDto> GetPostById(int id)
{
    var post = await _context.Posts
        .Where(p => p.Id == id)
        .FirstOrDefaultAsync();
    
    if (post == null)
        throw new NotFoundException($"Post {id} not found");
    
    return _mapper.Map<ReadPostDto>(post);
}

// ✅ CORRECT: Include related entities when needed
public async Task<ReadPostDto> GetPostWithComments(int id)
{
    var post = await _context.Posts
        .Include(p => p.Comments)
        .FirstOrDefaultAsync(p => p.Id == id);
    
    if (post == null)
        throw new NotFoundException($"Post {id} not found");
    
    return _mapper.Map<ReadPostDto>(post);
}

// ❌ WRONG: Synchronous database access
public ReadPostDto GetPostById(int id)
{
    var post = _context.Posts.FirstOrDefault(p => p.Id == id);  // Blocks thread
}

// ❌ WRONG: N+1 query problem
public async Task<IEnumerable<ReadPostDto>> GetAllPosts()
{
    var posts = await _context.Posts.ToListAsync();
    var dtos = new List<ReadPostDto>();
    foreach (var post in posts)
    {
        var comments = await _context.Comments  // Multiple queries!
            .Where(c => c.PostId == post.Id)
            .ToListAsync();
    }
}
```

## Error Handling

### Exception Hierarchy

```csharp
// Custom exception for domain errors
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Standard exceptions for validation
public class ArgumentException : Exception  // Use for input validation
public class InvalidOperationException     // Use for state violations
```

### Error Handling Pattern

```csharp
[HttpPost]
public async Task<ActionResult<ReadPostDto>> CreatePost(
    [FromBody] CreatePostDto dto)
{
    try
    {
        var result = await _postService.CreatePost(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            dto);
        return CreatedAtAction(nameof(GetPostById), 
            new { id = result.Id }, result);
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        // Log unexpected exceptions
        return StatusCode(500, "An unexpected error occurred");
    }
}
```

## Code Generation Rules

### When Generating New Endpoints

1. **Create DTO files first** (Create, Edit, Read variants)
2. **Define service interface** with method signatures
3. **Implement service class** with business logic
4. **Create controller methods** with proper error handling
5. **Add AutoMapper profile** for model-to-DTO mapping
6. **Write unit tests** for service logic

### Feature Checklist

When adding a new feature, ensure:

- [ ] DTOs defined with proper validation attributes
- [ ] Service interface created
- [ ] Service implementation completed with async patterns
- [ ] Controller methods added with error handling
- [ ] HTTP status codes correct
- [ ] AutoMapper profile configured
- [ ] No synchronous database access
- [ ] All async methods properly awaited
- [ ] Custom exceptions used for domain errors
- [ ] Unit tests written

### Never Generate

- ❌ Controllers without services
- ❌ DTOs without validation attributes
- ❌ Synchronous database operations
- ❌ Unhandled exceptions in controllers
- ❌ Mixed async/sync patterns
- ❌ Direct entity returns (not mapped to DTOs)

## Quality Checklist

### Code Review Checklist

- [ ] All I/O operations are async
- [ ] DTOs have validation attributes
- [ ] Services are injected via interfaces
- [ ] Controllers don't contain business logic
- [ ] Proper HTTP status codes returned
- [ ] Custom exceptions used appropriately
- [ ] Error messages are descriptive
- [ ] No null pointer exceptions possible
- [ ] Relationships properly configured in DbContext
- [ ] AutoMapper profiles configured correctly

### Before Committing

- [ ] Code compiles without warnings
- [ ] Unit tests pass
- [ ] No TODO comments left behind
- [ ] XML documentation added to public methods
- [ ] Performance-critical queries verified
- [ ] Security concerns addressed (SQL injection, etc.)

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-11
**Next Review**: 2026-05-11
**Owner**: Development Team
**Scope**: PostHubAPI .NET layered architecture implementation
