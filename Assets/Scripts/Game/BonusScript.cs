using UnityEngine;
using System.Collections;
using System.Linq;

public enum BonusType
{
  Clone
  //Fly,
  //Slowmotion
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
    text.renderer.enabled = false;
    animator = GetComponent<Animator>();
  }

  internal void SetRandomType()
  {
    var types = System.Enum.GetValues(typeof(BonusType)); ;
    bonus = (BonusType)types.GetValue(Random.Range(0, types.Length));
  }

  public void Pick(CoconutScript coconut)
  {
    // Sound
    Soundbank.Instance.PlaySound("bonus", transform.position);

    // Auto destruction
    collider2D.enabled = false;
    Destroy(gameObject, 5f);

    GameScript game = FindObjectOfType<GameScript>();

    // Visual feedback
    text.renderer.enabled = true;
    text.text = bonus.ToString();
    animator.SetTrigger("pick");

    // Effect
    switch (bonus)
    {
      case BonusType.Clone:
        GameObject coconutClone = Instantiate(coconut.gameObject) as GameObject;

        // Tell the script it's a clone
        var coconutCloneScript = coconutClone.GetComponent<CoconutScript>();
        coconutCloneScript.IsClone = true;
      
        // Disable the collider for few sec
        coconutClone.collider2D.enabled = false;
        StartCoroutine(EnableColliderAfterCooldown(coconutClone, 0.5f));
      
        // Eject
        coconutClone.rigidbody2D.AddForce(new Vector2(Random.Range(-50f, 150f), Random.Range(1500f, 2500f)));

        break;
      //case BonusType.Fly:
      //  break;
      //case BonusType.Slowmotion:
      //  game.AddSlowmotionBonus(game.slowmotionTotalTimeInSeconds);
      //  break;
      default:
        break;
    }
  }

	private IEnumerator EnableColliderAfterCooldown(GameObject coconut, float cooldown) {
    yield return new WaitForSeconds(cooldown);

    coconut.collider2D.enabled = true;
	}
}
