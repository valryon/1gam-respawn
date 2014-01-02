using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
  // --------------------------
  // x and y controls
  // --------------------------

  public float horizontalAcceleration = 10;
  public float maxHorizontalSpeed = 15.0f;
  public float jumpAcceleration = 120;
  public float continuousJumpAcceleration = 7f;
  public float continuousJumpDuration = 0.5f; // seconds

  public int Level = 1;

  /// <summary>
  /// Sprite flip
  /// </summary>
  private Vector2 flip = new Vector2(1, 1);

  private float continousJumpRemaingTime;
  private bool isGrounded;
  private Transform groundCheckerLeft, groundCheckerRight;
  private Vector2 startPosition;

  private WeaponScript[] weapons;
  private HealthScript health;

  void Awake()
  {
    groundCheckerLeft = transform.FindChild("FloorCheckLeft");
    if (groundCheckerLeft == null)
    {
      Debug.LogError("Missing FloorCheckLeft object");
    }

    groundCheckerRight = transform.FindChild("FloorCheckRight");
    if (groundCheckerRight == null)
    {
      Debug.LogError("Missing FloorCheckRight object");
    }

    startPosition = transform.position;

    health = GetComponent<HealthScript>();
    health.OnDeath += DieAndRespawn;

    weapons = GetComponentsInChildren<WeaponScript>();
  }

  #region Horizontal

  /// <summary>
  /// Handles the horizontal axis.
  /// </summary>
  private float HandleHorizontalAxis()
  {
    float x = Input.GetAxis("Horizontal");

    if (x == 0f)
    {
      // Do nothing if no input.
      return 0f;
    }
    else
    {
      // Movement speed
      float movement = x * horizontalAcceleration;

      return movement;
    }
  }

  #endregion

  #region Vertical

  /// <summary>
  /// Handles the player jump
  /// </summary>
  /// <param name="pressed">If set to <c>true</c> pressed.</param>
  /// <param name="maintened">If set to <c>true</c> maintened.</param>
  private float HandleJump()
  {
    float movement = 0f;

    // Check for jump input
    // Two types: first press or continuous
    if (Input.GetButton("Jump") || Input.GetButtonDown("Jump"))
    {
      if (continousJumpRemaingTime > 0f)
      {
        continousJumpRemaingTime -= Time.deltaTime;
      }

      if (Input.GetButtonDown("Jump"))
      {
        bool jumped = false;

        // Basic jump
        if (isGrounded)
        {
          jumped = true;

          movement = jumpAcceleration;
        }

        if (jumped)
        {
          continousJumpRemaingTime = continuousJumpDuration;
        }
      }
      else if (Input.GetButton("Jump"))
      {
        // Add a force when the jump button is maintened
        if (continousJumpRemaingTime > 0f)
        {
          rigidbody2D.AddForce(new Vector2(0, continuousJumpAcceleration));
        }
      }
    }
    else
    {
      continousJumpRemaingTime = 0f;
    }

    return movement * flip.y;
  }

  #endregion

  void Update()
  {
    // Ground?
    //---------------------------------------------
    //isGrounded = Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground"));
    isGrounded = false;
    TestGroundForChecker(groundCheckerLeft);
    TestGroundForChecker(groundCheckerRight);

    // Attack
    //---------------------------------------------
    if (Input.GetButtonDown("Fire1"))
    {
      foreach (var wpn in weapons)
      {
        if (wpn.CanAttack)
        {
          var projectile = wpn.Attack();

          SetProjectileToLevel(projectile);
        }
      }
    }

    // Movement
    //---------------------------------------------
    float x = 0f;
    float y = 0f;

    //  x
    // -----
    x = HandleHorizontalAxis();

    if (x != 0)
    {
      // Remove the previous velocity on x (exponential otherwise)
      x = x - (rigidbody2D.velocity.x);
    }

    //  y
    // -----
    y = HandleJump();

    // Apply movements
    // -----
    var force = new Vector2(x, y);

    if (force != Vector2.zero)
    {
      // Finally, add the force to the rigid body.
      rigidbody2D.AddForce(force);

      float currentMaxHorizontalSpeed = maxHorizontalSpeed;

      rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -currentMaxHorizontalSpeed, currentMaxHorizontalSpeed),
                                          rigidbody2D.velocity.y // Do not clamp in y, it useless and makes the jump very hard to tweak
      );
    }

    UpdateFlip();
  }

  private void TestGroundForChecker(Transform checker)
  {
    RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, checker.position);

    foreach (var hit in hits)
    {
      if (hit.collider.gameObject != this.gameObject)
      {
        isGrounded = true;
        break;
      }
    }
  }

  private void UpdateFlip()
  {
    // Flip 
    // -- x
    if (Input.GetAxis("Horizontal") != 0f)
    {
      flip.x = Mathf.Sign(Input.GetAxis("Horizontal"));
    }

    Vector3 currentScale = transform.localScale;
    if (Mathf.Sign(flip.x) != Mathf.Sign(currentScale.x))
    {
      if (flip.x < 0)
      {
        currentScale.x = Mathf.Abs(currentScale.x) * -1;
      }
      else
      {
        currentScale.x = Mathf.Abs(currentScale.x);
      }
    }
    // -- y
    // -- for reverse gravity, y flip should be the opposite sign of the gravity
    flip.y = -1 * Mathf.Sign(Physics2D.gravity.y);

    if (Mathf.Sign(flip.y) != Mathf.Sign(currentScale.y))
    {
      if (flip.y < 0)
      {
        currentScale.y = Mathf.Abs(currentScale.y) * -1;
      }
      else
      {
        currentScale.y = Mathf.Abs(currentScale.y);
      }
    }

    transform.localScale = currentScale;
  }

  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // Magic Numberz party below
  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  void DieAndRespawn()
  {
    // DIE


    // UPGRADE
    Level++;

    // Change stats
    this.transform.localScale += new Vector3(0.1f, 0.1f, 0);

    // Camera is a bit larger
    Camera.main.orthographicSize += 0.1f;

    // RESPAWN

    // Change location
    this.transform.position = startPosition;

    // Reset health
  }

  private void SetProjectileToLevel(Transform projectile)
  {

    // Apply level on projectile
    projectile.transform.localScale = projectile.transform.localScale * (Level * 0.25f);

    MoveScript projectileMove = projectile.GetComponent<MoveScript>();
    projectileMove.speed *= (Level * 0.15f);

    DamageScript damage = projectile.GetComponent<DamageScript>();
    damage.damage = (int)Mathf.Ceil(Level * 0.15f);
  }

  #region Properties

  /// <summary>
  /// Sprite orientation
  /// </summary>
  public Vector2 Flip
  {
    get
    {
      return flip;
    }
  }

  #endregion

}
