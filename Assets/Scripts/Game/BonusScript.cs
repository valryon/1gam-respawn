using UnityEngine;
using System.Collections;
using System.Linq;

public enum BonusType
{
  Clone,
  Fly,
  Slowmotion
}

public class BonusScript : MonoBehaviour {

  public float duration;
  public BonusType bonus;

  private TextMesh text;
  private Animator animator;

	void Start () 
  {
    text = GetComponentInChildren<TextMesh>();
    text.renderer.enabled = false;

    animator = GetComponent<Animator>();
	}

  internal void SetRandomType()
  {
    var types = System.Enum.GetValues(typeof(BonusType));;
    bonus = (BonusType)types.GetValue(Random.Range(0, types.Length));
  }

  public void Pick()
  {
    // Visual feedback
    //animator.SetTrigger("pick");
    text.text = bonus.ToString();
    text.renderer.enabled = true;
  }
}
