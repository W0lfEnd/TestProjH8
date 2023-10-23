using UnityEngine;


public class CameraController : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private Transform _target;
  [SerializeField] private float     _rotationSpeed;
  [SerializeField] private bool      isLockedAtTarget             = false;
  [SerializeField] private float     lookAtNMetersInfrontToTarget = 0.0f;
  #endregion

  #region Private Fields
  private Vector3    _offsetOnStart   = default;
  private Quaternion _rotationAtStart = default;
  #endregion


  #region Unity Methods
  void Awake()
  {
    _offsetOnStart = _target.position - transform.position;
    _rotationAtStart = _target.rotation;
  }

  void Update()
  {
    followByX();

    if ( isLockedAtTarget )
      spectate();
  }
  #endregion

  #region Private Methods
  private void spectate()
  {
    Vector3    direction  = _target.position + (_target.forward * lookAtNMetersInfrontToTarget) - transform.position;
    Quaternion toRotation = Quaternion.LookRotation( direction, Vector3.up );
    transform.rotation = Quaternion.Slerp( transform.rotation, toRotation, _rotationSpeed * Time.time );
  }

  private void followByX()
  {
    Vector3 curPos = transform.position;
    curPos = _target.position - _offsetOnStart;

    transform.position = curPos;
  }
  #endregion
}