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

public class FakeCoconutScript : MonoBehaviour {

  private TextMesh text;

  private static Dictionary<MessageType, string[]> messages = new Dictionary<MessageType, string[]>() {
    { MessageType.Bonus, new string[]{
        "Yeepee,a super bonus!",
        "What did you just take?",
        "Double coconut!"
      }},
    { MessageType.Fail, new string[]{
        "Haha, fail!",
        "Loser!",
        "What a shame"
      }},
    { MessageType.Kill, new string[]{
        "Bang!",
        "Nice!",
        "Take that!"
      }},
    { MessageType.Power, new string[]{
        "Nice combo!",
        "Wow, coooooombo",
        "Growing combo!"
      }},
    { MessageType.Start, new string[]{
        "SPACEBAR to start!",
        "Hit SPACEBAR to start killing!",
        "SPACEBAR is magic!"
      }}

  };

	void Start () 
  {
    text = GetComponentInChildren<TextMesh>();
    text.color = GetComponent<SpriteRenderer>().color;
    text.renderer.enabled = false;

    IsBusy = false;
	}

  public void Message(MessageType messageType) {
    IsBusy = true;
    text.renderer.enabled = true;

    var messagesForType = messages[messageType];
    string m = messagesForType[Random.Range(0, messagesForType.Length)];
    text.text = m;

    StartCoroutine(Timeout(((m.Length/10)+1) * 2f));
  }

  IEnumerator Timeout(float duration) {
    yield return new WaitForSeconds(duration);

    text.renderer.enabled = false;
    IsBusy = false;

    yield return null;
  }

  public bool IsBusy
  {
    get; private set;
  }
}
