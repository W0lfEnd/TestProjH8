using System;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class RangedFloat
{
  #region Public Fields
  public float min = 0.0f;
  public float max = 0.0f;
  #endregion


  #region Public Methods
  public float getRandomValue()
  {
    return Random.Range( min, max );
  }

  public float getClampedValue( float value )
  {
    return Mathf.Clamp( value, min, max );
  }

  public bool valuesEqualZero()
  {
    return min == 0.0f  && max == 0.0f;
  }

  public void setRange( float min_value, float max_value )
  {
    this.min = min_value;
    this.max = max_value;
  }
  #endregion

  #region Public Constructor
  public RangedFloat()
  {
    this.min = 0.0f;
    this.max = 0.0f;
  }

  public RangedFloat( float min, float max )
  {
    this.min = min;
    this.max = max;
  }
  #endregion
}
