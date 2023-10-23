using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu( fileName = "JetSkiParticleSettings", menuName = "JetSkiParticleSettings", order = 0 )]
public class JetSkiParticleSettings : ScriptableObject
{
  public float       Power         = 0.001f;
  public float       EnableAtSpeed = 200f;
  public RangedFloat StartSpeed    = new(2.0f, 6.0f);
  public RangedFloat StartLifetime = new(0.0f, 0.7f);
  public RangedFloat ScaleLimits   = new(0.0f, 5.0f);
}