using UnityEngine;
using System.Collections;

public class CoconutScript : MonoBehaviour
{
  /// <summary>
  /// Force applied when using arrows to move the coconut
  /// </summary>
  public Vector2 controlForce;

  /// <summary>
  /// Rebound when hitting an enemy
  /// </summary>
  public float reboundForce;

  /// <summary>
  /// Available slowmotion time
  /// </summary>
  public float slowmotionTotalTimeInSeconds;

  /// <summary>
  /// Slowmotion bonus per kill
  /// </summary>
  public float slowmotionBonusPerKill;

  private bool isFalling, isSlowmotion;
  private float slowmotionRemainingTime;
  private float previousRealtimeDelta;
  private bool hasReboundThisFrame;

  private GameScript gameScript;


  void Start()
  {
    gameScript = FindObjectOfType<GameScript>();
    if (gameScript == null)
    {
      Debug.LogError("Missing Game script!");
    }

    // Disable physics
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.gravityScale = 0f;
    collider2D.enabled = false;

    isFalling = false;

    slowmotionRemainingTime = slowmotionTotalTimeInSeconds;
  }

  void Update()
  {
    hasReboundThisFrame = false;

    if (isFalling == false)
    {
      // SPACE to drop the coconut
      if (Input.GetKeyDown(KeyCode.Space))
      {
        Fall();
      }
    }
    else
    {
      // SLOW MOTION
      if (Input.GetKey(KeyCode.Space) && slowmotionRemainingTime > 0)
      {
        if (isSlowmotion || (slowmotionRemainingTime > (slowmotionTotalTimeInSeconds / 8f)))
        {
          isSlowmotion = true;

          // delta time is affected by slow motion so it can't be used for a cooldown here
          ApplySlowMotion();
          slowmotionRemainingTime -= (Time.realtimeSinceStartup - previousRealtimeDelta);
        }
        else
        {
          isSlowmotion = false;
        }
      }
      else
      {
        isSlowmotion = false;
      }

      if (isSlowmotion == false)
      {
        // Regen slow motion
        DisableSlowMotion();
        slowmotionRemainingTime += Time.deltaTime;
      }

      slowmotionRemainingTime = Mathf.Clamp(slowmotionRemainingTime, 0f, slowmotionTotalTimeInSeconds);
      gameScript.GUI.UpdateSlowmotion(slowmotionRemainingTime / slowmotionTotalTimeInSeconds);

      // ARROWS to move slightly
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        rigidbody2D.AddForce(new Vector2(-1 * controlForce.x, controlForce.y / 10f));
      }
      if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        rigidbody2D.AddForce(new Vector2(controlForce.x, (controlForce.y / 10f)));
      }
      if (Input.GetKeyDown(KeyCode.UpArrow))
      {
        rigidbody2D.AddForce(new Vector2(0, controlForce.y / 2f));
      }
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        rigidbody2D.AddForce(new Vector2(0, -controlForce.y));
      }

      // Keep in camera bounds
      var dist = (transform.position - Camera.main.transform.position).z;
      var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
      var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

      transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
                transform.position.y,
                transform.position.z
                );
    }
    previousRealtimeDelta = Time.realtimeSinceStartup;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    RandomGuyScript dudeMaybe = collision.gameObject.GetComponent<RandomGuyScript>();

    // Hit something?
    if (dudeMaybe)
    {
      // On the head?
      for (int i = 0; i < collision.contacts.Length; i++)
      {
        if (collision.contacts[i].normal.y > 0.6f)
        {
          HitDude(dudeMaybe, collision.contacts[i].normal);
          break;
        }
      }
    }

    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      // Destroy the coconut
      DestroyOnGroundCollision();
    }
  }

  /// <summary>
  /// Make the terrible coconut fall on its target
  /// </summary>
  private void Fall()
  {
    isFalling = true;

    rigidbody2D.gravityScale = 1f;
    collider2D.enabled = true;
  }

  /// <summary>
  /// Kill someone violently
  /// </summary>
  /// <param name="theDude"></param>
  private void HitDude(RandomGuyScript theDude, Vector2 normal)
  {
    theDude.Kill(this);

    // Slowmotion bonus
    slowmotionRemainingTime += slowmotionBonusPerKill;

    // Rebound
    Vector2 baseForce = (normal * reboundForce);
    Vector2 force = Vector2.zero;

    if (hasReboundThisFrame == false)
    {
      hasReboundThisFrame = true;
      force = baseForce + ((baseForce * (gameScript.ComboCount - 1)) / 10f); // Bonus for each combo
    }
    else
    {
      force = baseForce / 4f;
    }
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.AddForce(force);
  }

  /// <summary>
  /// Coconut breaks on ground
  /// </summary>
  private void DestroyOnGroundCollision()
  {
    // Notify game script
    gameScript.CoconutDestroyed();

    // Change sprite?
    SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    spriteRenderer.color = Color.grey;

    // Stop moving 
    Destroy(rigidbody2D);
    Destroy(collider2D);

    // Remove script
    Destroy(this);

    // Make sure we're not in slowmo
    DisableSlowMotion();
  }

  private void ApplySlowMotion()
  {
    Time.timeScale = 0.15f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale; // Smooth physics
  }

  private void DisableSlowMotion()
  {
    Time.timeScale = 1f;
  }
}
