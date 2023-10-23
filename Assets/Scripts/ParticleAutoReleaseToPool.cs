using UnityEngine;
using UnityEngine.Pool;


[RequireComponent( typeof( ParticleSystem ) )]
public class ParticleAutoReleaseToPool : MonoBehaviour
{
  #region Private Fields
  private IObjectPool<ParticleSystem> _pool           = null;
  private ParticleSystem              _particleSystem = null;
  #endregion


  #region Unity Methods
  private void Awake()
  {
    _particleSystem = gameObject.GetComponent<ParticleSystem>();

    ParticleSystem.MainModule psMain = _particleSystem.main;
    psMain.stopAction = ParticleSystemStopAction.Callback;
  }
  
  private void OnParticleSystemStopped()
  {
    release();
  }
  #endregion

  #region Public Methods
  public void init( IObjectPool<ParticleSystem> pool )
  {
    this._pool = pool;
  }

  public void release()
  {
    if ( _pool != null )
    {
      _pool.Release( _particleSystem );
      return;
    }
    
    Destroy( gameObject );
  }
  #endregion
}