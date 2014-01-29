using UnityEngine;
using System.Collections;

/// <summary>
/// Particles! Screen shaking! Fun! Juice!
/// </summary>
public class SpecialEffects : MonoBehaviour
{
  public static SpecialEffects Instance;

  public ParticleSystem killEffect;
  public ParticleSystem groundEffect;
  public ParticleSystem juiceExplosionEffect;

  private Vector3 originPosition;
  private float shakeDecay;
  private float shakeIntensity;

  private Transform specialEffectParent;

  void Awake()
  {
    Instance = this;

    specialEffectParent = new GameObject("Special effects").transform;
  }

  void Update()
  {
    if (shakeIntensity > 0)
    {
      Camera.main.transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;
      shakeIntensity -= shakeDecay;
    }
  }

  void Destroy()
  {
    Instance = null;
  }

  public void JuiceExplosion(Vector3 position)
  {
    ParticleSystem ps = Instantiate(juiceExplosionEffect) as ParticleSystem;
    ps.transform.position = position;
    ps.transform.parent = specialEffectParent;
    Destroy(ps.gameObject, ps.duration);
  }

  public void KillEffect(Vector3 position)
  {
    ParticleSystem ps = Instantiate(killEffect) as ParticleSystem;
    ps.transform.position = position;
    ps.transform.parent = specialEffectParent;
    Destroy(ps.gameObject, ps.duration);
  }

  public void GroundEffect(Vector3 position)
  {
    ParticleSystem ps = Instantiate(groundEffect) as ParticleSystem;
    ps.transform.position = position;
    ps.transform.parent = specialEffectParent;
    Destroy(ps.gameObject, ps.duration);
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="force">[0, 0.5]</param>
  /// <param name="duration">seconds</param>
  public void ShakeCamera(float force, float duration)
  {
    originPosition = Camera.main.transform.position;

    shakeIntensity = force;

    float frames = duration / Time.deltaTime;
    shakeDecay = shakeIntensity / frames;
  }
}
