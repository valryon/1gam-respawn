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

  // ------

  private bool isFalling;
  private bool hasReboundThisFrame;

  private int powerLevel;

  private GameScript gameScript;
  private Animator animator;
  private TrailRenderer trail;

  void Start()
  {
    gameScript = FindObjectOfType<GameScript>();
    if (gameScript == null)
    {
      Debug.LogError("Missing Game script!");
    }

    trail = GetComponentInChildren<TrailRenderer>();

    animator = GetComponent<Animator>();

    // Disable physics
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.gravityScale = 0f;
    collider2D.enabled = false;

    isFalling = false;

    // Clones fall directly
    if (IsClone)
    {
      Fall();
    }

    UpdatePower();
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
      bool isMoving = false;

      // ARROWS to move slightly
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        isMoving = true;
        rigidbody2D.AddForce(new Vector2(-1 * controlForce.x, controlForce.y / 10f));
      }
      if (Input.GetKey(KeyCode.RightArrow))
      {
        isMoving = true;
        rigidbody2D.AddForce(new Vector2(controlForce.x, (controlForce.y / 10f)));
      }
      if (Input.GetKey(KeyCode.UpArrow))
      {
        isMoving = true;
        rigidbody2D.AddForce(new Vector2(0, controlForce.y / 2f));
      }
      if (Input.GetKey(KeyCode.DownArrow))
      {
        isMoving = true;
        rigidbody2D.AddForce(new Vector2(0, -controlForce.y));
      }

      if (isMoving)
      {
        //Soundbank.Instance.PlaySound("control", transform.position);
      }
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
        if (collision.contacts[i].normal.y > 0.6f)
        {
          HitDude(dudeMaybe, collision.contacts[i].normal);
          break;
        }
      }
    }

    // Another coconut?
    //CoconutScript anotherCoconut = collision.gameObject.GetComponent<CoconutScript>();
    //if (anotherCoconut != null)
    //{
    //}

    // The ground?
    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      // Destroy the coconut
      DestroyOnGroundCollision();
    }
  }

  void OnTriggerEnter2D(Collider2D otherCollider)
  {
    // Pick bonus?
    BonusScript bonus = otherCollider.GetComponent<BonusScript>();
    if (bonus != null)
    {
      bonus.Pick(this);
    }
  }

  /// <summary>
  /// Make the terrible coconut fall on its target
  /// </summary>
  public void Fall()
  {
    isFalling = true;
    animator.SetTrigger("fall");

    rigidbody2D.gravityScale = 1f;
    collider2D.enabled = true;
  }

  /// <summary>
  /// Kill someone violently
  /// </summary>
  /// <param name="theDude"></param>
  private void HitDude(RandomGuyScript theDude, Vector2 normal)
  {
    animator.SetTrigger("kill");

    theDude.Kill(this);

    // Slowmotion bonus
    gameScript.AddSlowmotionBonus(gameScript.slowmotionBonusPerKill);

    // Rebound
    Vector2 baseForce = (normal * reboundForce);
    Vector2 force = Vector2.zero;

    if (hasReboundThisFrame == false)
    {
      hasReboundThisFrame = true;
      force = baseForce + ((baseForce * (gameScript.ComboCount - 1)) / 15f); // Bonus for each combo
    }
    else
    {
      force = baseForce / 4f;
    }
    rigidbody2D.velocity = Vector2.zero;
    rigidbody2D.AddForce(force);

    UpdatePower();

    // JUICE
    // Force should be very small (0.5f = waow). Duration is in seconds
    SpecialEffects.Instance.ShakeCamera(powerLevel / 16f, 0.1f);

  }

  /// <summary>
  /// Coconut breaks on ground
  /// </summary>
  private void DestroyOnGroundCollision()
  {
    animator.SetTrigger("ground");

    // PARTICLES
    SpecialEffects.Instance.GroundEffect(transform.position);
    Soundbank.Instance.PlaySound("ground", transform.position);

    // Notify game script
    gameScript.CoconutDestroyed();

    // Stop moving 
    Destroy(rigidbody2D);
    Destroy(collider2D);

    // Remove script
    Destroy(this);
  }

  public void UpdatePower() 
  {
    powerLevel = (gameScript.ComboCount / 5) + 1 ;

    trail.endWidth = powerLevel * 0.04f;
    trail.startWidth = powerLevel * 0.04f;
  }

  public bool IsClone
  {
    get;
    set;
  }

}
