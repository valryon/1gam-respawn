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
    currentHP = baseHealthPoints;
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
