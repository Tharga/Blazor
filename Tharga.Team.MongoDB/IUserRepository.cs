using Tharga.MongoDB;

namespace Tharga.Team.MongoDB;

public interface IUserRepository<TUserEntity> : IRepository
    where TUserEntity : EntityBase, IUser
{
    IAsyncEnumerable<TUserEntity> GetAsync();
    Task<TUserEntity> GetAsync(string identity);
    Task AddAsync(TUserEntity user);
}