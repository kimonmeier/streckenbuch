namespace Streckenbuch.Shared.Models;

[Flags]
public enum HalteortTyp : int
{
    None = 0,
    Point = 1 << 0,
    Half = Point << 1,
    One = Half << 1,
    OneHalf = One << 1,
    Two = OneHalf << 1,
    TwoHalf = Two << 1,
    Three = TwoHalf << 1,
    ThreeHalf = Three << 1,
    Four = ThreeHalf << 1,
}