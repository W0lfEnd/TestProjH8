using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuoyancyAlwaysLookUp : MonoBehaviour
{
  #region Serialize Fields
  [SerializeField] private float _rotationSpeed = 10f;
  [Range( 0, 90f )]
  [SerializeField] private float _maxAngle = 90f;

  [SerializeField] private Rigidbody _rigidbody;
  #endregion


  #region Unity Methods
  private void FixedUpdate()
  {
    float dotProd = Vector3.Dot( _rigidbody.transform.up, Vector3.up );

    Vector3 rightWorld = _rigidbody.transform.right.setY( 0 ).normalized;
    if ( dotProd < _maxAngle / 90f )
    {
      float rotationPower = ( _rotationSpeed * ( 1 - dotProd * dotProd ) );
      //rotationPower *= rotationPower;
      _rigidbody.AddTorque( Vector3.Cross(_rigidbody.transform.up, Vector3.up).normalized * rotationPower, ForceMode.Force );
    }
  }
  #endregion
}