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
        "Hu?!",
        "What?!",
        "Bonus time!",
        "Enjoy!",
      }},
    { MessageType.Fail, new string[]{
        "Fail",
        "Ha ha",
        "Loser",
        "What a shame",
        "OMG",
        "Arg",
        "LOL",
        "xD",
        "WTF",
        "Sucker!"
      }},
    { MessageType.Kill, new string[]{
        "Bang",
        "Nice",
        "Take that",
        "You rocks",
        "<3",
        "Wow"
      }},
    { MessageType.Power, new string[]{
        "My hero!",
        "My coconut!",
        "Marry me!",
        "Dat combo!"
      }},
    { MessageType.Start, new string[]{
        "Press SPACEBAR!",
        "Smash SPACEBAR!",
        "Hit SPACEBAR!"
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
