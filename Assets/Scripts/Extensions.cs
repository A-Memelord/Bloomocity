public static class Extensions
{
    public static float NextFloat(this System.Random random, float minValue, float maxValue)
    {
        double val = (random.NextDouble() * (maxValue - minValue)) + minValue;
        return (float)val;
    }
}