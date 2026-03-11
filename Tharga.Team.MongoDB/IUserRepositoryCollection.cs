using Tharga.MongoDB;

namespace Tharga.Team.MongoDB;

public interface IUserRepositoryCollection<TUserEntity> : IDiskRepositoryCollection<TUserEntity>
    where TUserEntity : EntityBase, IUser;