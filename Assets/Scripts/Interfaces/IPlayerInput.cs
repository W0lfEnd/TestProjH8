public enum PlayerInputType
{
  PC_DISCRETE = 0,
  PC_CONTINUOUS   = 1,
  MOBILE      = 2
}


public interface IPlayerInput
{
  void handle();

  float? getHorizontalAxisInput();

  float? getVerticalAxisInput();
}