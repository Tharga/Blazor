using System.Security.Claims;

namespace Tharga.Team;

public interface IUserService
{
    Task<IUser> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal = default);
    IAsyncEnumerable<IUser> GetAsync();
}

//public abstract class UserServiceBase : IUserService
//{
//    private readonly IUserRep _userRep;

//    protected UserServiceBase(IUserRep userRep)
//    {
//        _userRep = userRep;
//    }

//    public Task<IUser> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal = default)
//    {
//        throw new NotImplementedException();
//    }

//    public IAsyncEnumerable<IUser> GetAsync()
//    {
//        throw new NotImplementedException();
//    }
//}

//public interface IUserRep
//{

//}