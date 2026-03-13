using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Tharga.MongoDB;
using Tharga.MongoDB.Disk;

namespace Tharga.Team.MongoDB;

internal class TeamRepositoryCollection<TTeamEntity, TMember> : DiskRepositoryCollectionBase<TTeamEntity>, ITeamRepositoryCollection<TTeamEntity, TMember>
    where TTeamEntity : TeamEntityBase<TMember>
    where TMember : TeamMemberBase
{
    public TeamRepositoryCollection(IMongoDbServiceFactory mongoDbServiceFactory, ILogger<RepositoryCollectionBase<TTeamEntity, ObjectId>> logger)
        : base(mongoDbServiceFactory, logger)
    {
    }

    public override string CollectionName => "Team";

    public override IEnumerable<CreateIndexModel<TTeamEntity>> Indices =>
    [
        new(Builders<TTeamEntity>.IndexKeys.Ascending(x => x.Key), new CreateIndexOptions { Unique = true, Name = "Key" }),
        new(Builders<TTeamEntity>.IndexKeys.Combine(
            Builders<TTeamEntity>.IndexKeys.Ascending(x => x.Id),
            Builders<TTeamEntity>.IndexKeys.Ascending("Members.Key")
        ), new CreateIndexOptions { Unique = true, Name = "UniqueTeamMemberKey" })
    ];
}