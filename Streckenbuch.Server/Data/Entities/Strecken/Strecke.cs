#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using Streckenbuch.Shared.Data.Entities;

namespace Streckenbuch.Server.Data.Entities.Strecken;

public class Strecke : IEntity
{
    public Guid Id { get; set; }

    public int StreckenNummer { get; set; }
}
