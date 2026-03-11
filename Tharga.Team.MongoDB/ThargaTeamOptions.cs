using Tharga.MongoDB;

namespace Tharga.Team.MongoDB;

public record ThargaTeamOptions
{
    internal Type _userEntity;
    internal Type _teamEntity;
    internal Type _teamMemberModel;

    public void RegisterUserRepository<TUserEntity>()
        where TUserEntity : EntityBase, IUser
    {
        _userEntity = typeof(TUserEntity);
    }

    public void RegisterTeamRepository<TTeamEntity, TTeamMemberModel>()
        where TTeamEntity : TeamEntityBase<TTeamMemberModel>
        where TTeamMemberModel : TeamMemberBase
    {
        _teamEntity = typeof(TTeamEntity);
        _teamMemberModel = typeof(TTeamMemberModel);
    }
}