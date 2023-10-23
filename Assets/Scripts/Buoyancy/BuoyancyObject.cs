using System;
using Bitgem.VFX.StylisedWater;
using UnityEngine;


[RequireComponent( typeof( Rigidbody ) )]
public class BuoyancyObject : MonoBehaviour
{
  #region Events
  public event Action<bool> OnTouchingWaterStateChanged = delegate {};
  public event Action<int, int> OnTouchingWaterFloatersCountChanged = delegate {};
  #endregion

  #region Serialize Fields
  [SerializeField] public Transform[] Floaters;
  [SerializeField] public float       FloatingPower = 15f;

  [Header("Underwater Settings")]
  [SerializeField] private float _underWaterDrag = 1.0f;
  [SerializeField] private float _underWaterAngularDrag = 4.5f;

  [Header( "Air Settings")]
  [SerializeField] private float _airDrag = 0.1f;
  [SerializeField] private float _airAngularDrag = 1.0f;
  #endregion

  #region Public Properties
  public bool IsTouchingWater => _floatersUnderwaterCount > 0;

  public int FloatersUnderwaterCount
  {
    get => _floatersUnderwaterCount;
    set
    {
      if ( _floatersUnderwaterCount == value )
        return;

      int oldValue = _floatersUnderwaterCount;

      bool oldIsTouchingWater = IsTouchingWater;
      _floatersUnderwaterCount = value;

      _rigidbody.drag        = IsTouchingWater ? _underWaterDrag        : _airDrag;
      _rigidbody.angularDrag = IsTouchingWater ? _underWaterAngularDrag : _airAngularDrag;

      if ( oldIsTouchingWater != IsTouchingWater )
        OnTouchingWaterStateChanged( IsTouchingWater );

      OnTouchingWaterFloatersCountChanged( oldValue, value );
    }
  }
  #endregion

  #region Private Fields
  private Rigidbody _rigidbody;
  private int       _floatersUnderwaterCount = 0;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    _rigidbody = this.GetComponent<Rigidbody>();
  }

  private void FixedUpdate()
  {
    int  floatersUnderwaterCountNow = 0;
    for ( int i = 0; i < Floaters.Length; i++ )
    {
      float heightDiff = Floaters[i].position.y - getWaterHightAtPosition( Floaters[i].position );
      if ( heightDiff < 0 )
      {
        floatersUnderwaterCountNow++;
        float powerForOneFloater = FloatingPower / Floaters.Length;
        _rigidbody.AddForceAtPosition( powerForOneFloater * Mathf.Abs( heightDiff ) * Vector3.up, Floaters[i].position, ForceMode.Force );
      }
    }

    FloatersUnderwaterCount = floatersUnderwaterCountNow;
  }
  #endregion

  #region Private Methods
  private float getWaterHightAtPosition( Vector3 pos )
  {
    if ( !WaterVolumeHelper.Instance )
      return 0.0f;

    return WaterVolumeHelper.Instance.GetHeight( pos ) ?? 0.0f;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    for ( int i = 0; i < Floaters.Length; i++ )
    {
      Gizmos.DrawWireSphere( Floaters[i].position, 0.1f );
    }
  }
  #endregion
}