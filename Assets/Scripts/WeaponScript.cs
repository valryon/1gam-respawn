using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
  /// <summary>
  /// Projectile prefab for shooting
  /// </summary>
  public Transform projectilePrefab;

  /// <summary>
  /// Cooldown in seconds between two shots
  /// </summary>
  public float shootingRate = 0.25f;

  private float shootCooldown;

  void Start()
  {
    shootCooldown = 0f;
  }

  void Update()
  {
    if (shootCooldown > 0)
    {
      shootCooldown -= Time.deltaTime;
    }
  }

  /// <summary>
  /// Create a new projectile if possible
  /// </summary>
  public Transform Attack()
  {
    if (CanAttack)
    {
      shootCooldown = shootingRate;

      // Create a new shot
      var projectileTransform = Instantiate(projectilePrefab) as Transform;
      // Position
      projectileTransform.position = transform.position;

      // Make the weapon shot always towards it
      MoveScript projectileMove = projectileTransform.GetComponent<MoveScript>();
      projectileMove.direction = this.transform.right; // towards in 2D space is the right of the sprite

      return projectileTransform;
    }

    return null;
  }

  /// <summary>
  /// Is the weapon ready to create a new projectile?
  /// </summary>
  public bool CanAttack
  {
    get
    {
      return shootCooldown <= 0f;
    }
  }
}


