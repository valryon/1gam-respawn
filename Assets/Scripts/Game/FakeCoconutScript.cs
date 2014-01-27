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
        "Bonus1",
        "Bonus2",
        "Bonus3"
      }},
    { MessageType.Fail, new string[]{
        "Fail1",
        "Fail2",
        "Fail3"
      }},
    { MessageType.Kill, new string[]{
        "Kill1",
        "Kill2",
        "Kill3"
      }},
    { MessageType.Power, new string[]{
        "Power1",
        "Power2",
        "Power3"
      }},
    { MessageType.Start, new string[]{
        "Start1",
        "Start2",
        "Start3"
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
