using UnityEngine;


public class PlayerController : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private PlayerInput      _playerInput;
  [SerializeField] private PlayerMovement   _playerMovement;
  [SerializeField] private PlayerVFX        _playerVFX;
  [SerializeField] private PlayerAnimator   _playerAnimator;
  [SerializeField] private PlayerAppearance _playerAppearance;
  [Space]
  [SerializeField] private BuoyancyObject   _buoyancyObject;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    _buoyancyObject.OnTouchingWaterStateChanged         += onTouchingWaterStateChanged;
    _buoyancyObject.OnTouchingWaterFloatersCountChanged += onTouchingWaterFloatersCountChanged;

    _playerAppearance.initRandom();
  }

  void Update()
  {
    _playerVFX.ForwardSpeed = _playerMovement.ForwardSpeed;
    _playerVFX.EnableParticles = _playerInput.CurrentVerticalInput != null;

    _playerMovement.RawInputRotationAngle = _playerInput.CurrentHorizontalInput.GetValueOrDefault( 0.0f );
    _playerMovement.RawInputSpeedForce = _playerInput.CurrentVerticalInput.GetValueOrDefault( 0.0f );
  }
  #endregion

  #region Private Methods
  private void onTouchingWaterStateChanged( bool state )
  {
    _playerMovement.CanMove = state;
    _playerVFX.UseWaterParticles = state;
  }

  private void onTouchingWaterFloatersCountChanged( int oldCount, int newCount )
  {
    if ( newCount > oldCount )
      _playerVFX.spawnLandingWaterSplash();
  }
  #endregion
}