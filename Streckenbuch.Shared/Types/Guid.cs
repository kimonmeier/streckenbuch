namespace Streckenbuch.Shared.Types;

public partial class GuidProto
{
    public GuidProto(Guid guid)
    {
        Value = guid.ToString();
    }

    public static implicit operator GuidProto(Guid guid)
    {
        return new GuidProto()
        {
            Value = guid.ToString()
        };
    }

    public static implicit operator Guid(GuidProto proto)
    {
        return Guid.Parse(proto.Value);
    }
}
