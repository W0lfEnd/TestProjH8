using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private PlayerInputType _inputType             = default;
  [SerializeField] private float           _horizontalSensitivity = 1f;
  [SerializeField] private float           _verticalSensitivity   = 1f;
  #endregion

  #region Public Properties
  public float? CurrentHorizontalInput { get; private set; }
  public float? CurrentVerticalInput   { get; private set; }
  #endregion

  #region Private Fields
  private IPlayerInput playerInput => playerInputs.safeGet( _inputType );

  private Dictionary<PlayerInputType, IPlayerInput> playerInputs = new Dictionary<PlayerInputType, IPlayerInput>()
  {
    [PlayerInputType.PC_DISCRETE]   = new PlayerInputPCDiscrete()
  , [PlayerInputType.PC_CONTINUOUS] = new PlayerInputPCContinuous()
  , [PlayerInputType.MOBILE]        = new PlayerInputMobile(),
  };
  #endregion


  #region Unity Methods
  private void Awake()
  {
    _inputType = getDefaultInputType();
  }

  void Update()
  {
    playerInput.handle();
    setInputValues();
  }
  #endregion

  #region Private Methods
  private void setInputValues()
  {
    CurrentHorizontalInput = playerInput.getHorizontalAxisInput() * _horizontalSensitivity;
    CurrentVerticalInput = playerInput.getVerticalAxisInput() * _verticalSensitivity;
    //CurrentVerticalInput = _verticalSensitivity;
  }

  private PlayerInputType getDefaultInputType()
    => Application.platform switch
    {
      RuntimePlatform.IPhonePlayer
        or RuntimePlatform.Android
        => PlayerInputType.MOBILE

    , _ => PlayerInputType.PC_DISCRETE
    };
  #endregion
}