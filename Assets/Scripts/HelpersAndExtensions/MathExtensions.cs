public static class MathExtensions
{
  public static float withMin( this float value, float min )
  {
    return value < min ? min : value;
  }

  public static float withMax( this float value, float max )
  {
    return value > max ? max : value;
  }

  public static float toRange( this float data, float min, float max )
  {
    return data.withMin( min ).withMax( max );
  }

  public static float toRange( this float data, RangedFloat rangedFloat )
  {
    return data.toRange( rangedFloat.min, rangedFloat.max );
  }

  public static bool isInRange(this float value, float inclusiveMin, float inclusiveMax)
  {
    return value >= inclusiveMin && value <= inclusiveMax;
  }
}