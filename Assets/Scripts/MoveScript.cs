using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{
  public Vector2 direction;
  public Vector2 speed;

  private Vector3 movement;

  void Update()
  {
    movement = new Vector3(
       speed.x * direction.x,
       speed.y * direction.y,
       0);

    if (rigidbody2D == null)
    {
      transform.Translate(movement * Time.deltaTime);
    }
  }

  void FixedUpdate()
  {
    if (rigidbody2D != null)
    {
      rigidbody2D.velocity = movement;
    }
  }
}
