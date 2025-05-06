using AutoMapper;
using Google.Protobuf.Collections;

namespace Streckenbuch.Server.Mappings;


public class RepeatedFieldConverter<T> : ITypeConverter<List<T>?, RepeatedField<T>>, ITypeConverter<RepeatedField<T>?, List<T>>
{
    public RepeatedField<T> Convert(List<T>? source, RepeatedField<T> destination, ResolutionContext context)
    {
        var repeatedField = new RepeatedField<T>();
        if (source is not null)
        {
            repeatedField.AddRange(source);
        }

        return repeatedField;
    }

    public List<T> Convert(RepeatedField<T>? source, List<T> destination, ResolutionContext context)
    {
        var list = new List<T>();
        if (source is not null)
        {
            list.AddRange(source);
        }

        return list;
    }
}