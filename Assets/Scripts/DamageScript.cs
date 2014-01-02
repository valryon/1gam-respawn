using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour
{
  public int damage = 1;

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
    //if (otherCollider2D.tag != this.tag)
    //{
      HealthScript healthScript = otherCollider2D.GetComponent<HealthScript>();
      if (healthScript != null)
      {
        Debug.Log(gameObject + " inflicts " + damage + " damages to " + otherCollider2D.gameObject);

        healthScript.HealthPoints -= damage;
      }
    }
  //}
}
