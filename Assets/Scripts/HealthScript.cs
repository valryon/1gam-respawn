using UnityEngine;
using System.Collections;

/// <summary>
/// Handle damages
/// </summary>
public class HealthScript : MonoBehaviour
{
  public int baseHealthPoints = 1;

  public event System.Action OnDeath;

  private int currentHP;

  void Start()
  {
    Reset();
  }

  public void Reset()
  {
    currentHP = baseHealthPoints;
  }

  void OnDrawGizmos()
  {
    // health bar

    float purcent = ((float)currentHP / (float)baseHealthPoints);

    if (purcent > 0.7f)
    {
      Gizmos.color = Color.green;
    }
    else if (purcent > 0.35f)
    {
      Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
    }
    else
    {
      Gizmos.color = Color.red;
    }

    Gizmos.DrawCube(transform.position, new Vector3((1 * purcent), 0.25f, 1));
  }

  /// <summary>
  /// Current health value
  /// </summary>
  public int HealthPoints
  {
    get
    {
      return currentHP;
    }
    set
    {
      currentHP = value;
      if (currentHP <= 0)
      {
        if (OnDeath != null) OnDeath();
        else
        {
          Destroy(this.gameObject);
        }
      }
    }
  }

}
