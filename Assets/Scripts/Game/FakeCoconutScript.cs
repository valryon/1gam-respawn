using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MessageType
{
  Bonus,
  Kill,
  Start,
  Fail,
  Power
}

public class FakeCoconutScript : MonoBehaviour
{

  private TextMesh textMesh;
  private Animator animator;

  private static Dictionary<MessageType, string[]> messages = new Dictionary<MessageType, string[]>() {
    { MessageType.Bonus, new string[]{
        "Yeepee,a super bonus!",
        "What did you just take?",
        "Double coconut!",
        "Bonus!!!!",
        "Look at that!"

      }},
    { MessageType.Fail, new string[]{
        "Haha, fail!",
        "Loser!",
        "What a shame",
        "OMG",
        "WTF",
        "Sucker!"
      }},
    { MessageType.Kill, new string[]{
        "Bang",
        "Nice",
        "Take that",
        "You rocks",
        "<3"
      }},
    { MessageType.Power, new string[]{
        "Nice combo!",
        "Wow, coooooombo",
        "Growing combo!",
        "Marry me!"
      }},
    { MessageType.Start, new string[]{
        "SPACEBAR to start!",
        "SPACEBAR is magic!"
      }}

  };

  void Start()
  {
    GetComponentIfNotStarted();

    IsBusy = false;
    this.textMesh.text = "";
    animator.SetBool("talk", false);
  }

  public void Message(MessageType messageType)
  {
    GetComponentIfNotStarted();

    IsBusy = true;
    animator.SetBool("talk", true);

    var messagesForType = messages[messageType];
    string m = messagesForType[Random.Range(0, messagesForType.Length)];
    this.textMesh.text = m;

    StartCoroutine(Timeout(((m.Length / 10) + 1) * 2f));
  }

  IEnumerator Timeout(float duration)
  {
    yield return new WaitForSeconds(duration);

    this.textMesh.text = "";
    IsBusy = false;
    animator.SetBool("talk", false);

    yield return null;
  }

  private void GetComponentIfNotStarted()
  {
    if (this.textMesh == null)
    {
      textMesh = GetComponentInChildren<TextMesh>();
    }
    if (this.animator == null)
    {
      this.animator = GetComponent<Animator>();
    }
  }

  public bool IsBusy
  {
    get;
    private set;
  }
}
