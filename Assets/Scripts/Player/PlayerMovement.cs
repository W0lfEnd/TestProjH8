using Bitgem.VFX.StylisedWater;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
  #region Serialize Fields
  [Header( "References" )]
  [SerializeField] public Rigidbody _rigidbody;
  [SerializeField] private Transform _forwardForcePoint;
  [SerializeField] private Transform _torqueForcePoint;

  [FormerlySerializedAs( "RawInputSpeed" )]
  [Header( "Input values" )]
  [SerializeField] public float RawInputSpeedForce    = 0.0f;
  [SerializeField] public float RawInputRotationAngle = 0.0f;

  [Header( "Rotation" )]
  [SerializeField] private float _rotationForceMultiplier = 8.0f;
  [SerializeField] private float _rotationAngleMultiplier = 40.0f;
  [SerializeField] private float _maxRotationAngle        = 180f;

  [Header( "Move Forward" )]
  [SerializeField] private float _speedForceMultiplier = 12.0f;
  [SerializeField] private float _maxVelocity = 1000f;
  private float InputSpeedForce => RawInputSpeedForce * _speedForceMultiplier;
  #endregion

  #region Public Properties
  public Rigidbody Rigidbody => _rigidbody;

  public float ForwardSpeed
  {
    get
    {
      Vector3 forward  = transform.forward;
      Vector3 velocity = Rigidbody.velocity;
      return Vector3.Dot( forward, velocity ) * velocity.magnitude;
    }
  }
  #endregion


  #region Unity Methods
  private void FixedUpdate()
  {
    tryRotate();

    //If our engine is out of water
    if ( _forwardForcePoint.position.y >= WaterVolumeHelper.Instance.GetHeight( _forwardForcePoint.position ) )
      return;

    tryMove();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.black;
    Gizmos.DrawWireSphere( _forwardForcePoint.position, 0.1f );

    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere( _torqueForcePoint.position, 0.1f );

    Gizmos.DrawLine( _torqueForcePoint.position, _torqueForcePoint.position - getRotationDirection() );
  }
  #endregion

  #region Private Methods
  private void tryMove()
  {
    if ( _maxVelocity > _rigidbody.velocity.magnitude && InputSpeedForce != 0.0f )
      _rigidbody.AddForceAtPosition( InputSpeedForce * _rigidbody.transform.forward, _forwardForcePoint.position );
  }

  private void tryRotate()
  {
    if ( RawInputRotationAngle != 0.0f )
      _rigidbody.AddForceAtPosition( getRotationDirection() * _rotationForceMultiplier, _torqueForcePoint.position );
  }

  private Vector3 getRotationDirection()
  {
    float moveVelocitySign = Mathf.Sign( InputSpeedForce );
    float applyRotationForce = -Mathf.Clamp( moveVelocitySign * RawInputRotationAngle * _rotationAngleMultiplier, -_maxRotationAngle, _maxRotationAngle );

    return Quaternion.AngleAxis( applyRotationForce, _rigidbody.transform.up ) * _rigidbody.transform.forward;
  }
  #endregion
}