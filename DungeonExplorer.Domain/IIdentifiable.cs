namespace DungeonExplorer.Api.Domain;

public interface IIdentifiable<TId>: IComparable<IIdentifiable<TId>>
{
    TId Id { get; set; }
}
