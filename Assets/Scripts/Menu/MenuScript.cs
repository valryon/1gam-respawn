using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{
  private bool gameLoading;

  void Start()
  {
    gameLoading = false;

    StartCoroutine(Fireworks());
  }

  IEnumerator Fireworks()
  {
    yield return new WaitForSeconds(5f);

    while (true)
    {
      yield return new WaitForSeconds(Random.Range(0.15f, 1.25f));

      for (int i = 0; i < Random.Range(0, 4); i++)
      {
        Vector3 position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
        SpecialEffects.Instance.JuiceExplosion(position);
      }
    }
  }

  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      SpecialEffects.Instance.JuiceExplosion(position);

      RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
      if (hit.collider != null && hit.collider.gameObject.name.ToLower().Contains("coconut")) // Haha, so ugly
      {
        if (gameLoading == false)
          StartCoroutine(LoadGame());
      }
    }
    else if (Input.anyKeyDown)
    {
      if (gameLoading == false)
        StartCoroutine(LoadGame()); LoadGame();
    }
  }

  IEnumerator LoadGame()
  {
    if (gameLoading == false)
    {
      gameLoading = true;

      Soundbank.Instance.PlaySound("ground", Vector3.zero);
      SpecialEffects.Instance.ShakeCamera(0.5f, 0.5f);

      yield return new WaitForSeconds(0.5f);

      // Play!
      Application.LoadLevel("Game");
    }
  }
}
