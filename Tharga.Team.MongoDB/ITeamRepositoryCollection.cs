using Tharga.MongoDB;

namespace Tharga.Team.MongoDB;

public interface ITeamRepositoryCollection<TTeamEntity, TTeamMemberModel> : IDiskRepositoryCollection<TTeamEntity>
    where TTeamEntity : TeamEntityBase<TTeamMemberModel>
    where TTeamMemberModel : TeamMemberBase;