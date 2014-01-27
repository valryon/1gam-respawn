using UnityEngine;
using System.Collections;

/// <summary>
/// Simply cross the screen
/// </summary>
public class RandomGuyScript : MonoBehaviour
{
  public float minSpeed;
  public float maxSpeed;
  public float direction;
  public float timeToLive = 15f;

  private float speed;

  void Start()
  {
    speed = Random.Range(minSpeed, maxSpeed) * direction;

    // Flip
    transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(direction), transform.localScale.y, transform.localScale.z);

    // Auto destroy
    StartCoroutine(ProgrammableKill(timeToLive));
  }

  void Update()
  {
  }

  void FixedUpdate()
  {
    rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
  }

  IEnumerator ProgrammableKill(float timeToLive)
  {
    // Wait
    yield return new WaitForSeconds(timeToLive);

    // Kill if not already dead
    Kill(null);

    // End coroutine
    yield return null;
  }

  /// <summary>
  /// Falling Coconuts Kill More People Than Shark Attacks
  /// </summary>
  /// <param name="killingCoconut"></param>
  public void Kill(CoconutScript killingCoconut)
  {
    if (killingCoconut != null)
    {
      // Juice!
      SpecialEffects.Instance.KillEffect(killingCoconut.transform.position);
      Soundbank.Instance.PlaySound("kill", transform.position);

      GameScript gameScript = FindObjectOfType<GameScript>();
      if (gameScript != null)
      {
        gameScript.GuyDestroyed();
      }
    }

    Destroy(gameObject);
  }
}
