using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SoundbankEntry
{
  public string key;
  public AudioClip sound;
}

public class Soundbank : MonoBehaviour
{
  public static Soundbank Instance;

  public List<SoundbankEntry> sounds;

  void Awake()
  {
    Instance = this;
  }

  /// <summary>
  /// Play a sound from bank
  /// </summary>
  /// <param name="name"></param>
  /// <param name="position"></param>
  public void PlaySound(string name, Vector3 position)
  {
    foreach (var s in sounds)
    {
      if (s.key.ToLower() == name.ToLower())
      {
        Play(s.sound, position);
        break;
      }
    }
  }

  private void Play(AudioClip audioClip, Vector3 position)
  {
    AudioSource.PlayClipAtPoint(audioClip, position);
  }
}
