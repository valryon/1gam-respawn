using UnityEngine;
using System.Collections;

public class TitleFireworksScript : MonoBehaviour
{
  void Start()
  {
    BoxCollider2D box = GetComponent<BoxCollider2D>();

    // Juice on all the collider
    for (int i = 0; i < 30; i++)
    {
      Vector3 position = new Vector3(
          Random.Range(transform.position.x - box.size.x, transform.position.x + box.size.x),
          Random.Range(transform.position.y - box.size.y, transform.position.y + box.size.y),
          0
        );

      SpecialEffects.Instance.JuiceExplosion(position);
    }

    // Auto destruct
    Destroy(gameObject);
  }
}
