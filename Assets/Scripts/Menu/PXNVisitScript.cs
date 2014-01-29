using UnityEngine;
using System.Collections;

public class PXNVisitScript : MonoBehaviour
{
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      if (collider2D.OverlapPoint(position))
      {
        Application.OpenURL("http://www.pixelnest.io");
      }
    }
  }
}
