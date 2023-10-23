using UnityEngine;


public class PlayerInputPCDiscrete : IPlayerInput
{
  public void handle()
  {
    //empty
  }

  public float? getHorizontalAxisInput()
  {
    if ( Input.GetKey( KeyCode.D ) )
      return 1.0f;

    if ( Input.GetKey( KeyCode.A ) )
      return -1.0f;

    return null;
  }

  public float? getVerticalAxisInput()
  {
    if ( Input.GetKey( KeyCode.W ) )
      return 1.0f;

    if ( Input.GetKey( KeyCode.S ) )
      return -1.0f;

    return null;
  }
}