using UnityEngine;
using System.Collections;

/// <summary>
/// Manage text information
/// </summary>
public class GameGUI : MonoBehaviour
{
  public GUIText timeText;
  public GUIText scoreText;
  public GUIText comboText;

  private float slowmoPurcent = 1f;

  public void UpdateGUI(float time, int score, int combo)
  {
    timeText.text = time.ToString("00");
    scoreText.text = score.ToString("000000000");
    comboText.text = "x" + combo;
  }

  public void UpdateSlowmotion(float purcent)
  {
    slowmoPurcent = purcent;
  }

  void OnGUI()
  {
    GUI.color = Color.green;
    GUI.HorizontalScrollbar(new Rect(40, 30, 200, 20), 1, slowmoPurcent * 100, 10, 110);
  }

}
