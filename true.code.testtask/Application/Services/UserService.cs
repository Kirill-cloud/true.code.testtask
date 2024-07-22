using Microsoft.EntityFrameworkCore;
using @true.code.testtask.Application.Contracts;
using @true.code.testtask.Domain.Models;
using @true.code.testtask.Infrastructure;

namespace @true.code.testtask.Application.Services;

public class UserService(ILogger<UserService> logger, TodoDbContext context)
{
    private const string InternalError = "An unexpected error occurred. Please try again later.";

    public async Task<DefaultResult<User[]>> GetAllUsers()
    {
        using var _ = logger.BeginScope("{action}", "get-all-users");
        try
        {
            var users = await context.Users.AsNoTracking().ToArrayAsync();
            return DefaultResult<User[]>.CreateSuccess(users);
        }
        catch (Exception e)
        {
            logger.LogError(e, "UserService.GetAllUsers handle exception");
            return DefaultResult<User[]>.CreateError(InternalError);
        }
    }

    public async Task<DefaultResult> CreateOrUpdateUser(UserEditModel userModel)
    {
        using var _ = logger.BeginScope("{action} {userId}", "create-or-update-user", userModel.Id);
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userModel.Id) ?? new User();

            user.Name = userModel.Name;
            context.Users.Attach(user);

            await context.SaveChangesAsync();
            return DefaultResult.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "UserService.CreateOrUpdateUser handle exception");
            return DefaultResult.CreateError(InternalError);
        }
    }
}