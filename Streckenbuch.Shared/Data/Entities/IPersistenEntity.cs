namespace Streckenbuch.Shared.Data.Entities;
public interface IPersistenEntity : IEntity
{
    public bool IsDeleted { get; set; }
}
