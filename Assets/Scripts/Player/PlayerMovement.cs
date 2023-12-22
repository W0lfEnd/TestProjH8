using System;
using Bitgem.VFX.StylisedWater;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
  #region Serialize Fields
  [Header( "References" )]
  [SerializeField] public Rigidbody _rigidbody;
  [SerializeField] private Transform _forwardForcePoint;
  [SerializeField] private Transform _modelTransform;

  [FormerlySerializedAs( "RawInputSpeed" )]
  [Header( "Input values" )]
  [SerializeField] public float RawInputSpeedForce    = 0.0f;
  [SerializeField] public float RawInputRotationAngle = 0.0f;

  [Header( "Rotation" )]
  [SerializeField] private float _rotationAngleSpeed = 40.0f;
  [SerializeField] private float _rotationAngleMax            = 40.0f;
  [SerializeField] private float _maxDeepForForwardForcePoint = 0.4f;

  [Header( "Move Forward" )]
  [SerializeField] private float _speedForceMultiplier = 12.0f;
  [SerializeField] private float _maxVelocity = 1000f;
  #endregion

  #region Public Properties
  public bool      CanMove   { get; set; }
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

  #region Private Fields
  private float   InputSpeedForce => RawInputSpeedForce * _speedForceMultiplier;
  private Vector3 _forwardForcePointStartPos = default;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    _forwardForcePointStartPos = _forwardForcePoint.localPosition;
  }

  private void FixedUpdate()
  {
    tryRotate();

    //If our engine is out of water
    if ( !CanMove )
      return;

    tryMove();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.black;
    Gizmos.DrawWireSphere( _forwardForcePoint.position, 0.1f );
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
    float   targetRotationAngle = RawInputRotationAngle * _rotationAngleMax;
    Vector3 curEulers     = _modelTransform.rotation.eulerAngles;

    Vector3 targetEulers = curEulers.setZ( -targetRotationAngle );
    _modelTransform.rotation = Quaternion.Slerp(_modelTransform.rotation, Quaternion.Euler( targetEulers ), _rotationAngleSpeed * Time.fixedDeltaTime);

    float normalizedRotationDiff = Vector3.Angle( Vector3.up, _modelTransform.up ) / Vector3.Angle( Vector3.up, Quaternion.AngleAxis( _rotationAngleMax, _modelTransform.forward ) * Vector3.up );
    _forwardForcePoint.localPosition = Vector3.Lerp( _forwardForcePointStartPos, _forwardForcePointStartPos.setY( -_maxDeepForForwardForcePoint ), normalizedRotationDiff );
  }
  #endregion
}