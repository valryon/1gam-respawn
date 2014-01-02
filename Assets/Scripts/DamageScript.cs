using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour
{
  public int damage = 1;
  public bool dieOnCollision = false;

  void OnCollisionEnter2D(Collision2D collision)
  {
    InflictDamages(collision.collider);
  }

  void OnTriggerEnter2D(Collider2D otherCollider2D)
  {
    InflictDamages(otherCollider2D);
  }

  private void InflictDamages(Collider2D otherCollider2D)
  {
    // Quickly compare tags
    if (otherCollider2D.tag != this.tag)
    {
      HealthScript healthScript = otherCollider2D.GetComponent<HealthScript>();
      if (healthScript != null)
      {
        healthScript.HealthPoints -= damage;

        if (dieOnCollision)
        {
          Destroy(this.gameObject);
        }
      }
    }
  }
}
