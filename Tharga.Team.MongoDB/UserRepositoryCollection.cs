using Microsoft.Extensions.Logging;
using Tharga.MongoDB;
using Tharga.MongoDB.Disk;

namespace Tharga.Team.MongoDB;

internal class UserRepositoryCollection<TUserEntity> : DiskRepositoryCollectionBase<TUserEntity>, IUserRepositoryCollection<TUserEntity>
    where TUserEntity : EntityBase, IUser
{
    public UserRepositoryCollection(IMongoDbServiceFactory mongoDbServiceFactory, ILogger<UserRepositoryCollection<TUserEntity>> logger)
        : base(mongoDbServiceFactory, logger)
    {
    }

    public override string CollectionName => "User";
}