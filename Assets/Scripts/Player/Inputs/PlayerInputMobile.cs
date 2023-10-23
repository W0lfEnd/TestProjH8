using UnityEngine;


public class PlayerInputMobile : IPlayerInput
{
  public Vector2 TouchStartedPos { get; private set; } = default;
  public Vector2? TouchCurPos
  {
    get
    {
      if ( !IsNowTouching )
        return null;

      return Input.GetTouch( 0 ).position;
    }
  }

  public bool IsNowTouching => Input.touchCount > 0;


  public void handle()
  {
    if ( !Input.touchSupported )
      return;

    if ( !IsNowTouching )
    {
      TouchStartedPos = default;
      return;
    }

    Touch touch = Input.GetTouch( 0 );
    switch ( touch.phase )
    {
    case TouchPhase.Began:
      TouchStartedPos = touch.position;
      break;

    case TouchPhase.Moved:
    case TouchPhase.Stationary:
    case TouchPhase.Ended:
    case TouchPhase.Canceled:
      //skip
      break;
    }
  }

  public float? getHorizontalAxisInput()
  {
    if ( !IsNowTouching )
      return null;

    float horizontalInput = ( (Vector2)TouchCurPos! - TouchStartedPos ).normalized.x;
    return horizontalInput;
  }

  public float? getVerticalAxisInput()
  {
    if ( !IsNowTouching )
      return null;

    float verticalInput = ( (Vector2)TouchCurPos! - TouchStartedPos ).normalized.y;
    return verticalInput;
  }
}