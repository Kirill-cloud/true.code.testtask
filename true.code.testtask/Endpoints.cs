using @true.code.testtask.Application.Contracts;
using @true.code.testtask.Application.Services;
using @true.code.testtask.Domain.Models;

namespace @true.code.testtask;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("api/todo/create", async (TodoItem request, TodoService todoService) =>
            await todoService.CreateTodo(request)
        );

        app.MapPost("api/todo/assign", async (AssignRequest request, TodoService todoService) =>
            await todoService.AssignTodo(request)
        );

        app.MapPost("api/todo/unassign/{id:int}", async (int id, TodoService todoService) =>
            await todoService.UnassignTodo(id)
        );

        app.MapPost("api/todo/set-priority", async (SetPriorityRequest request, TodoService todoService) =>
            await todoService.SetPriority(request.TodoId, request.PriorityLevel)
        );

        app.MapPost("api/todo/get-filtred", async (TodoFilter filter, TodoService todoService) =>
            await todoService.GetFiltered(filter)
        );

        app.MapPost("api/user/create", async (UserEditModel request, UserService userService) =>
            await userService.CreateOrUpdateUser(request)
        );

        app.MapGet("api/user/list", async (UserService userService) => 
            await userService.GetAllUsers()
        );
    }
}