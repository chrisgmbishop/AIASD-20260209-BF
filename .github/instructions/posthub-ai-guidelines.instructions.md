---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "GitHub Copilot"
chat_id: "posthub-ai-guidelines-20260211"
prompt: |
  Create AI-specific guidelines for generating code in PostHubAPI, including
  common anti-patterns, async handling, and quick reference patterns.
started: "2026-02-11T00:00:00Z"
ended: "2026-02-11T00:30:00Z"
task_durations:
  - task: "AI guidelines and anti-patterns documentation"
    duration: "00:30:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/11/posthub-ai-guidelines-20260211/conversation.md"
source: "GitHub Copilot"
applyTo: "**/*.cs"
---

# AI Assistant Guidelines for PostHubAPI

## Purpose

This guide helps AI assistants (GitHub Copilot, Claude, ChatGPT) generate correct, production-ready code for PostHubAPI. It focuses on common mistakes to avoid and proven patterns to follow.

**Target Audience**: AI code generation tools
**Scope**: Code generation patterns, common mistakes, quick reference implementations

## Before You Start

When asked to generate code for PostHubAPI:

1. **Read the context** - Ask for the specific requirement and existing patterns
2. **Check for similar code** - Look at existing files (Controllers, Services) to maintain consistency
3. **Follow layered architecture** - Controllers→Services→Data, never reverse
4. **Use async/await** - All I/O operations must be async
5. **Always use DTOs** - Never return domain models directly
6. **Include error handling** - Map exceptions to HTTP status codes

## Critical Anti-Patterns (NEVER Generate These)

### ❌ Anti-Pattern #1: Synchronous Database Access

```csharp
// WRONG - These block threads and cause performance issues
public ReadPostDto GetPostById(int id)
{
    var post = _context.Posts.FirstOrDefault(p => p.Id == id);  // Blocks!
    return _mapper.Map<ReadPostDto>(post);
}

// CORRECT
public async Task<ReadPostDto> GetPostByIdAsync(int id)
{
    var post = await _context.Posts
        .FirstOrDefaultAsync(p => p.Id == id);
    return _mapper.Map<ReadPostDto>(post);
}
```

### ❌ Anti-Pattern #2: Not Awaiting Async Methods

```csharp
// WRONG - Returns Task instead of actual value
[HttpPost]
public ActionResult<ReadUserDto> CreateUser(CreateUserDto dto)
{
    var result = _userService.CreateUserAsync(dto);  // Missing await!
    return CreatedAtAction(nameof(GetUserById), result);
}

// CORRECT
[HttpPost]
public async Task<ActionResult<ReadUserDto>> CreateUser(CreateUserDto dto)
{
    var result = await _userService.CreateUserAsync(dto);  // Awaited
    return CreatedAtAction(nameof(GetUserById), result);
}
```

### ❌ Anti-Pattern #3: Returning Domain Models Without DTOs

```csharp
// WRONG - Direct entity return exposes internal structure
[HttpGet("{id}")]
public async Task<ActionResult<Post>> GetPost(int id)
{
    var post = await _context.Posts.FindAsync(id);
    return Ok(post);  // Returning domain model!
}

// CORRECT
[HttpGet("{id}")]
public async Task<ActionResult<ReadPostDto>> GetPost(int id)
{
    var post = await _context.Posts.FindAsync(id);
    if (post == null)
        return NotFound();
    return Ok(_mapper.Map<ReadPostDto>(post));
}
```

### ❌ Anti-Pattern #4: Missing Null Checks

```csharp
// WRONG - Potential null reference exceptions
public async Task<ReadUserDto> GetUserAsync(int id)
{
    var user = await _context.Users.FindAsync(id);
    return _mapper.Map<ReadUserDto>(user);  // user could be null!
}

// CORRECT
public async Task<ReadUserDto> GetUserAsync(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        throw new NotFoundException($"User {id} not found");
    return _mapper.Map<ReadUserDto>(user);
}
```

### ❌ Anti-Pattern #5: N+1 Query Problem

```csharp
// WRONG - Multiple database queries in a loop
public async Task<List<ReadPostDto>> GetPostsAsync()
{
    var posts = await _context.Posts.ToListAsync();
    var results = new List<ReadPostDto>();
    
    foreach (var post in posts)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == post.Id)
            .ToListAsync();  // Query inside loop!
        
        var dto = _mapper.Map<ReadPostDto>(post);
        dto.Comments = _mapper.Map<List<ReadCommentDto>>(comments);
        results.Add(dto);
    }
    return results;
}

// CORRECT - Single query with Include
public async Task<List<ReadPostDto>> GetPostsAsync()
{
    var posts = await _context.Posts
        .Include(p => p.Comments)
        .ToListAsync();
    
    return _mapper.Map<List<ReadPostDto>>(posts);
}
```

### ❌ Anti-Pattern #6: No Error Handling in Controllers

```csharp
// WRONG - Unhandled exceptions return 500 errors
[HttpPost]
public async Task<ActionResult<ReadPostDto>> CreatePost(CreatePostDto dto)
{
    var result = await _postService.CreatePostAsync(dto);
    return CreatedAtAction(nameof(GetPostById), result);
}

// CORRECT - Explicit error handling
[HttpPost]
public async Task<ActionResult<ReadPostDto>> CreatePost(CreatePostDto dto)
{
    try
    {
        var result = await _postService.CreatePostAsync(dto);
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
```

### ❌ Anti-Pattern #7: DTOs Without Validation

```csharp
// WRONG - No validation attributes
public class CreatePostDto
{
    public string Content { get; set; }
    public int UserId { get; set; }
}

// CORRECT - With validation attributes
public class CreatePostDto
{
    [Required(ErrorMessage = "Content is required")]
    [StringLength(500, MinimumLength = 1,
        ErrorMessage = "Content must be 1-500 characters")]
    public string Content { get; set; }
    
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}
```

### ❌ Anti-Pattern #8: Calling Services Without Interface Injection

```csharp
// WRONG - Hard-coded dependency, can't unit test
public class PostController : ControllerBase
{
    private readonly PostService _service = new PostService();
}

// CORRECT - Injected interface for testability
public class PostController : ControllerBase
{
    private readonly IPostService _service;
    
    public PostController(IPostService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
}
```

## Quick Reference: Common Code Patterns
<function_calls>
<invoke name="create_file">
<parameter name="content">### Pattern 1: GET Endpoint (Return Single Resource)

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<ReadEntityDto>> GetEntityById(int id)
{
    try
    {
        var entity = await _entityService.GetEntityByIdAsync(id);
        return Ok(entity);
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
```

### Pattern 2: POST Endpoint (Create Resource)

```csharp
[HttpPost]
public async Task<ActionResult<ReadEntityDto>> CreateEntity(
    [FromBody] CreateEntityDto dto)
{
    try
    {
        var result = await _entityService.CreateEntityAsync(dto);
        return CreatedAtAction(nameof(GetEntityById), 
            new { id = result.Id }, result);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}
```

### Pattern 3: PUT Endpoint (Update Full Resource)

```csharp
[HttpPut("{id}")]
public async Task<ActionResult<ReadEntityDto>> UpdateEntity(
    int id, [FromBody] EditEntityDto dto)
{
    try
    {
        var result = await _entityService.UpdateEntityAsync(id, dto);
        return Ok(result);
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
```

### Pattern 4: DELETE Endpoint

```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteEntity(int id)
{
    try
    {
        await _entityService.DeleteEntityAsync(id);
        return NoContent();
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
```

### Pattern 5: GET Collection (List Resources)

```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<ReadEntityDto>>> GetAllEntities()
{
    try
    {
        var entities = await _entityService.GetAllEntitiesAsync();
        return Ok(entities);
    }
    catch (Exception)
    {
        return StatusCode(500, "An error occurred while retrieving entities");
    }
}
```

### Pattern 6: Service Method (CRUD Create)

```csharp
public async Task<ReadEntityDto> CreateEntityAsync(CreateEntityDto dto)
{
    // Validate input
    if (string.IsNullOrWhiteSpace(dto.Name))
        throw new ArgumentException("Name is required");
    
    // Create domain model
    var entity = new Entity
    {
        Name = dto.Name,
        Description = dto.Description,
        CreatedAt = DateTime.UtcNow
    };
    
    // Persist
    _context.Entities.Add(entity);
    await _context.SaveChangesAsync();
    
    // Return DTO
    return _mapper.Map<ReadEntityDto>(entity);
}
```

### Pattern 7: Service Method (CRUD Read)

```csharp
public async Task<ReadEntityDto> GetEntityByIdAsync(int id)
{
    var entity = await _context.Entities
        .FirstOrDefaultAsync(e => e.Id == id);
    
    if (entity == null)
        throw new NotFoundException($"Entity {id} not found");
    
    return _mapper.Map<ReadEntityDto>(entity);
}
```

### Pattern 8: Service Method (CRUD Update)

```csharp
public async Task<ReadEntityDto> UpdateEntityAsync(int id, EditEntityDto dto)
{
    var entity = await _context.Entities
        .FirstOrDefaultAsync(e => e.Id == id);
    
    if (entity == null)
        throw new NotFoundException($"Entity {id} not found");
    
    // Update properties
    entity.Name = dto.Name ?? entity.Name;
    entity.Description = dto.Description ?? entity.Description;
    entity.UpdatedAt = DateTime.UtcNow;
    
    // Persist changes
    _context.Entities.Update(entity);
    await _context.SaveChangesAsync();
    
    return _mapper.Map<ReadEntityDto>(entity);
}
```

### Pattern 9: Service Method (CRUD Delete)

```csharp
public async Task DeleteEntityAsync(int id)
{
    var entity = await _context.Entities
        .FirstOrDefaultAsync(e => e.Id == id);
    
    if (entity == null)
        throw new NotFoundException($"Entity {id} not found");
    
    _context.Entities.Remove(entity);
    await _context.SaveChangesAsync();
}
```

## Common Mistakes When Generating Code

### Mistake 1: Wrong HTTP Status Codes

```csharp
// ❌ WRONG - Using wrong status codes
[HttpPost]
public async Task<ActionResult<ReadPostDto>> CreatePost(CreatePostDto dto)
{
    var result = await _postService.CreatePostAsync(dto);
    return Ok(result);  // Should be CreatedAtAction with 201
}

// ✅ CORRECT
return CreatedAtAction(nameof(GetPostById), 
    new { id = result.Id }, result);  // 201 Created
```

### Mistake 2: Forgetting UpdatedAt Timestamp

```csharp
// ❌ WRONG - Not tracking when entity was updated
public async Task<ReadPostDto> UpdatePostAsync(int id, EditPostDto dto)
{
    var post = await _context.Posts.FindAsync(id);
    post.Content = dto.Content;
    // Missing: post.UpdatedAt = DateTime.UtcNow;
}

// ✅ CORRECT
post.UpdatedAt = DateTime.UtcNow;
_context.Posts.Update(post);
await _context.SaveChangesAsync();
```

### Mistake 3: Not Closing DbContext Connection

```csharp
// ❌ WRONG - Not explicitly awaiting SaveChangesAsync
_context.Posts.Add(post);
_context.SaveChanges();  // Synchronous!

// ✅ CORRECT
_context.Posts.Add(post);
await _context.SaveChangesAsync();  // Async
```

### Mistake 4: Inline Object Creation in Queries

```csharp
// ❌ WRONG - Can't translate to SQL
var posts = await _context.Posts
    .Where(p => p.UserId == GetCurrentUserId())  // Method call in query!
    .ToListAsync();

// ✅ CORRECT
var userId = GetCurrentUserId();
var posts = await _context.Posts
    .Where(p => p.UserId == userId)
    .ToListAsync();
```

## Type Safety and Nullability Checks

Always ensure:

```csharp
// ✅ Check for null user IDs from auth context
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
if (string.IsNullOrEmpty(userId))
    return Unauthorized("User ID not found in claims");

// ✅ Validate collection not empty before accessing
if (!dto.Tags?.Any() ?? true)  // null-coalescing pattern
    throw new ArgumentException("At least one tag required");

// ✅ Range checks for numeric IDs
if (id <= 0)
    return BadRequest("ID must be positive");
```

## Configuration and Dependency Injection Patterns

When generating service registration code, use this pattern:

```csharp
// In Program.cs
builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<IPostService, PostService>()
    .AddScoped<ICommentService, CommentService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));
```

## When in Doubt

1. **Check existing files** for similar patterns
2. **Verify method signatures** match the interface
3. **Test all async paths** thoroughly
4. **Include null checks** for all external inputs
5. **Map exceptions to HTTP status codes** appropriately
6. **Always use DTOs** in API responses
7. **Validate input** at both DTO level and service level

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-11
**Essential Reference**: Yes
