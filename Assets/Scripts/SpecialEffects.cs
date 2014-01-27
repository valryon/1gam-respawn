using UnityEngine;
using System.Collections;

/// <summary>
/// Particles! Screen shaking! Fun! Juice!
/// </summary>
public class SpecialEffects : MonoBehaviour
{
  public static SpecialEffects Instance;

  private Vector3 originPosition;
  private float shakeDecay;
  private float shakeIntensity;

  void Awake()
  {
    Instance = this;
  }

  void Update()
  {
    if (shakeIntensity > 0)
    {
      Camera.main.transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;
      shakeIntensity -= shakeDecay;
    }
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
