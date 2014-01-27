using UnityEngine;
using System.Collections;
using System.Linq;

public enum BonusType
{
  Clone,
  //Fly,
  Slowmotion
}

public class BonusScript : MonoBehaviour
{

  public float duration;
  public BonusType bonus;

  private TextMesh text;
  private Animator animator;

  void Start()
  {
    text = GetComponentInChildren<TextMesh>();
    animator = GetComponent<Animator>();
  }

  internal void SetRandomType()
  {
    var types = System.Enum.GetValues(typeof(BonusType)); ;
    bonus = (BonusType)types.GetValue(Random.Range(0, types.Length));
  }

  public void Pick(CoconutScript coconut)
  {
    // Auto destruction
    collider2D.enabled = false;
    Destroy(gameObject, 5f);

    GameScript game = FindObjectOfType<GameScript>();

    // Visual feedback
    text.text = bonus.ToString();
    animator.SetTrigger("pick");

    // Effect
    switch (bonus)
    {
      case BonusType.Clone:
        GameObject coconutClone = Instantiate(coconut.gameObject) as GameObject;
        coconutClone.rigidbody2D.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(50f, 150f)));

        //var coconutCloneScript = coconutClone.GetComponent<CoconutScript>();

        break;
      //case BonusType.Fly:
      //  break;
      case BonusType.Slowmotion:
        game.AddSlowmotionBonus(game.slowmotionTotalTimeInSeconds);
        break;
      default:
        break;
    }
  }
}
