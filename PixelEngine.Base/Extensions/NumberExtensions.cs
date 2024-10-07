public static class NumberExtensions
{
    public static int NMod(this int number, int mod)
    {
        if (number >= 0)
            return number % mod;

        while (number < 0)
            number += mod;

        return number % mod;
    }
}
