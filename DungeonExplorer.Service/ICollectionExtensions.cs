namespace DungeonExplorer.Api.Service;

public static class CollectionSyncExtensions
{
    public static void SyncWith<T, TId>(
        this ICollection<T> existing,
        IEnumerable<T> incoming,
        Action<T, T> updateExisting)
        where T : class, IIdentifiable<TId>
        where TId : IComparable<TId>
    {
        // Split incoming into "new" and "identified"
        var incomingIdentified = incoming
            .Where(i => i.Id.CompareTo(default!) != 0)
            .ToList();

        var incomingNew = incoming
            .Where(i => i.Id.CompareTo(default!) == 0)
            .ToList();

        // Build dictionary only for identified items
        var existingById = existing
            .Where(e => e.Id.CompareTo(default!) != 0)
            .ToDictionary(e => e.Id);

        var incomingById = incomingIdentified
            .ToDictionary(e => e.Id);

        // DELETE missing
        var toDelete = existingById.Keys.Except(incomingById.Keys).ToList();
        foreach(var id in toDelete)
        {
            existing.Remove(existingById[id]);
        }

        // UPDATE existing
        var toUpdate = existingById.Keys.Intersect(incomingById.Keys).ToList();
        foreach(var id in toUpdate)
        {
            updateExisting(existingById[id], incomingById[id]);
        }

        // ADD new items (Id == default)
        foreach(var newItem in incomingNew)
        {
            existing.Add(newItem);
        }
    }
}