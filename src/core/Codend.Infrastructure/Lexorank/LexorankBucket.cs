namespace Codend.Infrastructure.Lexorank;

public class LexorankBucket : IEquatable<LexorankBucket>
{
    private static readonly LexorankBucket Bucket0 = new(0);
    private static readonly LexorankBucket Bucket1 = new(1);
    private static readonly LexorankBucket Bucket2 = new(2);

    private static readonly IReadOnlyCollection<LexorankBucket> BucketList = new List<LexorankBucket>()
    {
        Bucket0, Bucket1, Bucket2
    };

    /// <summary>
    /// Bucket id.
    /// </summary>
    public uint Id { get; }

    private LexorankBucket(uint id)
    {
        Id = id;
    }

    /// <summary>
    /// Returns first bucket.
    /// </summary>
    public static LexorankBucket First => Bucket0;

    /// <summary>
    /// Use to get bucket with given id.
    /// </summary>
    /// <param name="bucketId">Id of the bucket to be returned.</param>
    /// <returns><see cref="LexorankBucket"/> instance with given id or throws an exception.</returns>
    /// <exception cref="LexorankException">LexorankException when bucket id is not valid.</exception>
    public static LexorankBucket GetById(uint bucketId)
    {
        var bucket = BucketList.FirstOrDefault(x => x.Id == bucketId);
        return bucket ?? throw new LexorankException($"Wrong bucket id (bucketId = {bucketId}).");
    }

    public static LexorankBucket FromString(string bucketId)
    {
        if (uint.TryParse(bucketId, out var id))
        {
            return GetById(id);
        }

        throw new LexorankException($"Invalid bucketId string: {bucketId}. It should be parseable uint '0,1,2'.");
    }

    /// <summary>
    /// Use to get next bucket instance.
    /// </summary>
    /// <returns>Another <see cref="LexorankBucket"/> instance in queue.</returns>
    public LexorankBucket Next()
    {
        return Id switch
        {
            0 => Bucket1,
            1 => Bucket2,
            2 => Bucket0,
            _ => throw new LexorankException($"Invalid bucket id {Id}")
        };
    }

    /// <summary>
    /// Use to get previous bucket instance.
    /// </summary>
    /// <returns>Previous <see cref="LexorankBucket"/> instance.</returns>
    public LexorankBucket Previous()
    {
        return Id switch
        {
            0 => Bucket2,
            1 => Bucket0,
            2 => Bucket1,
            _ => throw new LexorankException($"Invalid bucket id {Id}")
        };
    }

    /// <inheritdoc />
    public bool Equals(LexorankBucket? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Id.ToString();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}