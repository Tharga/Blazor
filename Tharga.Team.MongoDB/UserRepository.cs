using Tharga.MongoDB;

namespace Tharga.Team.MongoDB;

internal class UserRepository<TUserEntity> : IUserRepository<TUserEntity>
    where TUserEntity : EntityBase, IUser
{
    private readonly IUserRepositoryCollection<TUserEntity> _collection;

    public UserRepository(IUserRepositoryCollection<TUserEntity> collection)
    {
        _collection = collection;
    }

    public virtual IAsyncEnumerable<TUserEntity> GetAsync()
    {
        return _collection.GetAsync();
    }

    public virtual Task<TUserEntity> GetAsync(string identity)
    {
        return _collection.GetOneAsync(x => x.Identity == identity);
    }

    public virtual Task AddAsync(TUserEntity user)
    {
        return _collection.AddAsync(user);
    }
}