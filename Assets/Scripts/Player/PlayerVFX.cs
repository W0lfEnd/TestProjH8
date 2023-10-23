using System;
using UnityEngine;
using UnityEngine.Pool;


public class PlayerVFX : MonoBehaviour
{
  #region Serialize Fields
  [Header("Prefabs")]
  [SerializeField] private ParticleSystem _prefabWaterSplashParticle;

  [Header( "References" )]
  [SerializeField] private ParticleSystem _mainThruster;
  [SerializeField] private ParticleSystem _leftWave;
  [SerializeField] private ParticleSystem _rightWave;
  [Space]
  [SerializeField] private TrailRenderer  _rightTrail;
  [SerializeField] private TrailRenderer  _leftTrail;
  [Space]
  [SerializeField] private Transform spawnPosLandingWaterSplash;

  [Header( "Particles Settings" )]
  [SerializeField] private JetSkiParticleSettings _mainThrusterSettings = null;
  [SerializeField] private JetSkiParticleSettings _sideWavesSettings = null;

  [Header( "Trails Settings" )]
  [SerializeField] private float Trails_EnableAtSpeed = 2f;
  #endregion

  #region Private Fields
  private ObjectPool<ParticleSystem> _poolSplashesParticles = null;
  private bool                       _enableParticles       = true;
  private bool                       _useWaterParticles     = false;

  [SerializeField, ReadOnly] private float _forwardSpeed = 0f;
  #endregion

  #region Public Fields
  public bool UseWaterParticles
  {
    get => _useWaterParticles;
    set
    {
      _useWaterParticles = value;
      EnableParticles = value;
    }
  }
  #endregion

  #region Public Properties
  public bool EnableParticles
  {
    get => _enableParticles;
    set
    {
      _enableParticles = value;

      setParticlesSettings();
    }
  }

  public float ForwardSpeed
  {
    get => _forwardSpeed;
    set
    {
      _forwardSpeed = value;

      OnSpeedChanged( _forwardSpeed );
    }
  }
  #endregion


  #region Unity Methods
  private void Awake()
  {
    initParticlesPool();
    ForwardSpeed = 0;
  }
  #endregion

  #region Public Methods
  public void spawnLandingWaterSplash()
  {
    ParticleSystem newParticle = _poolSplashesParticles.Get();

    newParticle.gameObject.ensureGetComponent<ParticleAutoReleaseToPool>().init( _poolSplashesParticles );
    newParticle.transform.SetPositionAndRotation( spawnPosLandingWaterSplash.position, spawnPosLandingWaterSplash.rotation );
  }
  #endregion

  #region Private Methods
  private void OnSpeedChanged( float speed )
  {
    setParticlesSettings( speed );
    setTrailsEnabled( speed > Trails_EnableAtSpeed );
  }

  private void setParticlesSettings() => setParticlesSettings( ForwardSpeed );

  private void setParticlesSettings( float speed )
  {
    setParticleSettings( _mainThruster, _mainThrusterSettings, speed );
    setParticleSettings( _leftWave,     _sideWavesSettings,    speed );
    setParticleSettings( _rightWave,    _sideWavesSettings,    speed );
  }

  private void setParticleSettings( ParticleSystem particle, JetSkiParticleSettings settings, float curSpeed )
  {
    bool isActive = curSpeed >= settings.EnableAtSpeed;
    setParticleEnabled( particle, isActive );

    if ( !isActive )
      return;

    setParticleScale( particle, ( curSpeed * settings.Power ).toRange( settings.ScaleLimits ) );

    float newStartLifetime = ( curSpeed * settings.Power ).toRange( settings.StartLifetime );
    setStartLifetime( particle, newStartLifetime );

    float normalizedNewStartLifetime = ( newStartLifetime - settings.StartLifetime.min ) / ( settings.StartLifetime.max - settings.StartLifetime.min );
    float newStartSpeed              = normalizedNewStartLifetime * ( settings.StartSpeed.max - settings.StartSpeed.min ) + settings.StartSpeed.min;
    setStartSpeed( particle, newStartSpeed );
  }

  private void setParticleScale( ParticleSystem particle, float size )
  {
    Vector3 newScale = Vector3.one * size;

    particle.transform.localScale = newScale;
  }

  private void setTrailsEnabled( bool state )
  {
    _leftTrail.enabled = state;
    _rightTrail.enabled = state;
  }

  private void setParticleEnabled( ParticleSystem particle, bool enabled )
  {
    if ( enabled && EnableParticles )
    {
      particle.Play();
      return;
    }

    particle.Stop();
  }

  private void setStartLifetime( ParticleSystem particle, float startLifetime )
  {
    ParticleSystem.MainModule main = particle.main;
    main.startLifetime = startLifetime;
  }

  private void setStartSpeed( ParticleSystem particle, float startSpeed )
  {
    ParticleSystem.MainModule main = particle.main;
    main.startSpeed = startSpeed;
  }

  private void initParticlesPool()
  {
    _poolSplashesParticles = new ObjectPool<ParticleSystem>( onCreate, onGet, onRelease, onDestroy, true, 2, 8 );
    return;

    ParticleSystem onCreate()
      => Instantiate( _prefabWaterSplashParticle, spawnPosLandingWaterSplash.position, spawnPosLandingWaterSplash.rotation, spawnPosLandingWaterSplash );

    void onGet( ParticleSystem particle )
    {
      particle.gameObject.SetActive( true );
      particle.Play();
    }

    static void onRelease( ParticleSystem particle )
    {
      particle.Stop();
      particle.gameObject.SetActive( false );
    }

    static void onDestroy( ParticleSystem particle )
    {
      Destroy(particle.gameObject);
    }
  }
  #endregion
}