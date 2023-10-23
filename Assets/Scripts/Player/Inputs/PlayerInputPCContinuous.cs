using UnityEngine;


public class PlayerInputPCContinuous : IPlayerInput
{
  public const float INPUT_EPSILON = 0.005f;

  public void handle()
  {
    //empty
  }

  public float? getHorizontalAxisInput()
  {
    float value = Input.GetAxis( "Horizontal" );
    if ( Mathf.Abs( value ) > INPUT_EPSILON )
      return value;

    return null;
  }

  public float? getVerticalAxisInput()
  {
    float value = Input.GetAxis( "Vertical" );
    if ( Mathf.Abs( value ) > INPUT_EPSILON )
      return value;

    return null;
  }
}