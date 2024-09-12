namespace Rewrite.RewriteJava.Tree;
public enum Flag : long
{
    Public = 1,
    Private = 1 << 1,
    Protected = 1 << 2,
    Static = 1 << 3,
    Final = 1 << 4,
    Synchronized = 1 << 5,
    Volatile = 1 << 6,
    Transient = 1 << 7,
    Native = 1 << 8,
    Interface = 1 << 9,
    Abstract = 1 << 10,
    Strictfp = 1 << 11,
    HasInit = 1 << 18,
    Varargs = 1L << 34,
    Union = 1L << 39,
    Default = 1L << 43,
    SignaturePolymorphic = 1L << 46,
    PotentiallyAmbiguous = 1L << 48,
    Sealed = 1L << 62,
    NonSealed = 1L << 63
}

internal static class FlagExtensions
{

    public static ISet<Flag> BitMapToFlags(long flagsBitMap)
    {
        var flags = new HashSet<Flag>();
        var values = Enum.GetValues(typeof(Flag));
        foreach (Flag flag in values)
        {
            if ((flagsBitMap & (long)flag) != 0) flags.Add(flag);
        }

        return flags;
    }

    public static long FlagsToBitMap(HashSet<Flag> flags)
    {
        long mask = 0;
        foreach (var flag in flags)
            mask |= (long)flag;
        return mask;
    }

    public static bool HasFlags(long flagsBitMap, params Flag[] flags)
    {
        foreach (var flag in flags)
            if ((flagsBitMap & (long)flag) == 0)
                return false;
        return true;
    }
}
