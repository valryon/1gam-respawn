using UnityEngine;
using System.Collections;

public class CoconutScript : MonoBehaviour
{
  public Vector2 controlForce;

  void Start()
  {
    // Disable physics
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.gravityScale = 0f;
    collider2D.enabled = false;
  }

  void Update()
  {
    // SPACE to drop the coconut
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Fall();
    }
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
        if (collision.contacts[i].normal.y > 0.8f)
        {
          HitDude(dudeMaybe);
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
    rigidbody2D.gravityScale = 1f;
    collider2D.enabled = true;
  }

  /// <summary>
  /// Kill someone violently
  /// </summary>
  /// <param name="theDude"></param>
  private void HitDude(RandomGuyScript theDude)
  {
    theDude.Kill(this);
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
  }
}
