using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{

  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
      if (hit.collider != null)
      {
        LoadGame();
      }
    }
    else if (Input.anyKeyDown)
    {
      LoadGame();
    }
  }

  private void LoadGame()
  {
    // Play!
    Application.LoadLevel("Game");
  }
}
