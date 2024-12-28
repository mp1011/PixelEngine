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

    public static int Clamp(this int number, int min, int max)
    {
        if (number < min)
            return min;
        else if (number > max)
            return max;
        else
            return number;
    }

    public static double Clamp(this double number, double min, double max)
    {
        if (number < min)
            return min;
        else if (number > max)
            return max;
        else
            return number;
    }
}
