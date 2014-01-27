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

  public void UpdateGUI(float time, int score, int combo)
  {
    timeText.text = time.ToString("00");
    scoreText.text = score.ToString("000000000");
    comboText.text = "x" + combo;
  }

  public void UpdateSlowmotion(float purcent)
  {

  }

}
