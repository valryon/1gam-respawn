using UnityEngine;
using System.Collections;

public class CoconutScript : MonoBehaviour
{
  public float controlForce;
  public float reboundForce;
  public float reboundVelocityPurcent;

  public float slowmoMaxTime;

  private bool isFalling;

  private int reboundCountThisFrame;
  private float slowmoTime;
  private Vector2 currentVelocity;

  void Start()
  {
    // Disable physics
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.gravityScale = 0f;
    collider2D.enabled = false;

    controlForce = Mathf.Abs(controlForce);
    isFalling = false;

    slowmoTime = slowmoMaxTime;
  }

  void Update()
  {
    reboundCountThisFrame = 0;

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
      if (slowmoTime > 0f && Input.GetKey(KeyCode.Space))
      {
        slowmoTime -= Time.deltaTime + (Time.deltaTime * (1 - Time.timeScale));
        EnableSlowMotion();
      }
      else
      {
        // Regen slow motion
        slowmoTime += Time.deltaTime;
        DisableSlowMotion();
      }
      Debug.Log(slowmoTime);
      slowmoTime = Mathf.Clamp(slowmoTime, 0f, slowmoMaxTime);

      // ARROWS to move slightly
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        // Lot to left, a bit up
        rigidbody2D.AddForce(new Vector2(-1 * controlForce, (1 * controlForce) / 10f));
      }
      else if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        // Lot to right, a bit up
        rigidbody2D.AddForce(new Vector2(1 * controlForce, (1 * controlForce) / 10f));
      }
      //else if (Input.GetKeyDown(KeyCode.UpArrow))
      //{
      //  // Lot to right, a bit up
      //  rigidbody2D.AddForce(new Vector2(0, controlForce));
      //}

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
  }

  void FixedUpdate()
  {
    // Store velocity from FixedUpdate
    // So we can use it in collisions (where the velocity has been reseted)
    currentVelocity = rigidbody2D.velocity;
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

    // Rebound
    reboundCountThisFrame++;
    rigidbody2D.velocity = Vector2.zero;

    Vector2 force = (normal * reboundForce) / reboundCountThisFrame;

    // The rebound relies on the fall velocity
    //force += force * (Mathf.Abs(currentVelocity.y) * reboundVelocityPurcent);

    rigidbody2D.AddForce(force);
  }

  /// <summary>
  /// Coconut breaks on ground
  /// </summary>
  private void DestroyOnGroundCollision()
  {
    // Notify game script
    GameScript gameScript = FindObjectOfType<GameScript>();
    if (gameScript != null)
    {
      gameScript.CoconutDestroyed();
    }

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

  private void EnableSlowMotion()
  {
    Time.timeScale = 0.15f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;
  }

  private void DisableSlowMotion()
  {
    Time.timeScale = 1f;
  }
}
