using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using @true.code.testtask.Application.Contracts;
using @true.code.testtask.Domain;
using @true.code.testtask.Domain.Models;
using @true.code.testtask.Infrastructure;

namespace @true.code.testtask.Application.Services;

public class TodoService(ILogger<TodoService> logger, TodoDbContext context)
{
    private const string InternalError = "An unexpected error occurred. Please try again later.";

    public async Task<DefaultResult> CreateTodo(TodoItem todo)
    {
        using var _ = logger.BeginScope("{action}", "create-todo");
        try
        {
            var validationResult = todo.Validate();
            if (!validationResult.IsSuccess) return DefaultResult.CreateError(validationResult.Error);

            context.TodoItems.Add(todo);
            await context.SaveChangesAsync();

            logger.LogInformation("todo created successfully");
            return DefaultResult.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "TodoService.AssignTodo handle exception");
            return DefaultResult.CreateError(InternalError);
        }
    }

    public async Task<DefaultResult> AssignTodo(AssignRequest request)
    {
        using var _ = logger.BeginScope("{action} {taskId} {userId}", "assign-todo", request.UserId, request.TodoId);
        try
        {
            var todo = await context.TodoItems.FirstOrDefaultAsync(x => x.Id == request.TodoId);
            if (todo == null)
            {
                logger.LogError("todo item not found");
                return DefaultResult.CreateError("Todo not found");
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                logger.LogError("user not found");
                return DefaultResult.CreateError("User not found");
            }

            var oldUser = todo.UserId;
            todo.User = user;
            await context.SaveChangesAsync();

            logger.LogInformation("todo assigned to user {newUserId}. Old assignee {oldUserId}", user.Id, oldUser);
            return DefaultResult.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "TodoService.AssignTodo handle exception");
            return DefaultResult.CreateError(InternalError);
        }
    }

    public async Task<DefaultResult> UnassignTodo(int todoId)
    {
        using var _ = logger.BeginScope("{action} {taskId}", "unassign-todo", todoId);
        try
        {
            var todo = await context.TodoItems.Include(todoItem => todoItem.User)
                .FirstOrDefaultAsync(x => x.Id == todoId);
            if (todo == null)
            {
                logger.LogError("todo item not found");
                return DefaultResult.CreateError("Todo not found");
            }

            var oldUser = todo.UserId;
            todo.User = null;
            await context.SaveChangesAsync();

            logger.LogInformation("todo {todoId} unassigned. Old assignee {oldUserId}", todoId, oldUser);
            return DefaultResult.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "TodoService.AssignTodo handle exception");
            return DefaultResult.CreateError(InternalError);
        }
    }

    public async Task<DefaultResult> SetPriority(int todoId, PriorityLevel priorityLevel)
    {
        using var _ = logger.BeginScope("{action} {taskId}", "unassign-todo", todoId);
        try
        {
            var todo = await context.TodoItems.Include(todoItem => todoItem.User)
                .FirstOrDefaultAsync(x => x.Id == todoId);
            if (todo == null)
            {
                logger.LogError("todo item not found");
                return DefaultResult.CreateError("Todo not found");
            }

            var priority = await context.Priorities.FirstOrDefaultAsync(x => x.Level == priorityLevel);
            if (priority == null)
            {
                logger.LogError("priority {level} not found", priorityLevel);
                return DefaultResult.CreateError("Priority not found");
            }

            todo.Priority = priority;
            await context.SaveChangesAsync();

            logger.LogInformation("priority {priority} set todo {todoId}", priorityLevel, todoId);
            return DefaultResult.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "TodoService.AssignTodo handle exception");
            return DefaultResult.CreateError(InternalError);
        }
    }

    public async Task<DefaultResult<TodoItem[]>> GetFiltered(TodoFilter filter)
    {
        using var _ = logger.BeginScope("{action}", "get-filtered");
        logger.LogInformation("filter {filter}", JsonSerializer.Serialize(filter));
        try
        {
            var filtered = context.TodoItems.AsNoTracking().AsQueryable();
            if (filter.PriorityLevel != null)
            {
                var priority = await context.Priorities.FirstOrDefaultAsync(x => x.Level == filter.PriorityLevel);
                if (priority == null)
                {
                    logger.LogError("priority {level} not found", filter.PriorityLevel);
                    return DefaultResult<TodoItem[]>.CreateError("priority not found");
                }

                filtered = filtered.Where(x => x.Priority.Level == filter.PriorityLevel);
            }

            if (filter.UserId != null)
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == filter.UserId);
                if (user == null)
                {
                    logger.LogError("user {id} not found", filter.UserId);
                    return DefaultResult<TodoItem[]>.CreateError("user not found");
                }

                filtered = filtered.Where(x => x.UserId == filter.UserId);
            }

            if (filter.IsCompleted != null) filtered = filtered.Where(x => x.IsCompleted == filter.IsCompleted);

            var result = await filtered.ToArrayAsync();

            return DefaultResult<TodoItem[]>.CreateSuccess(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "TodoService.GetFiltered handle exception");
            return DefaultResult<TodoItem[]>.CreateError(InternalError);
        }
    }
}