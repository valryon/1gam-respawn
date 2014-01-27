using UnityEngine;
using System.Collections;

/// <summary>
/// Core gameplay script
/// </summary>
public class GameScript : MonoBehaviour
{
  public Transform randomGuyPrefab;
  public Transform[] randomGuySpawns;

  public Transform coconutSpawn;
  public Transform coconutPrefab;

  public float minSpawnCooldownInSeconds = 0.45f;
  public float maxSpawnCooldownInSeconds = 1.25f;

  private Transform randomGuysParent;
  private float cooldown;

  void Start()
  {
    // Check parameters
    if (randomGuySpawns == null || randomGuySpawns.Length == 0) Debug.LogError("Missing randomGuySpawns!");
    if (randomGuyPrefab == null) Debug.LogError("Missing RandomGuy prefab!");

    if (coconutSpawn == null) Debug.LogError("Missing coconutSpawn!");

    // Initialize
    cooldown = Random.Range(minSpawnCooldownInSeconds, maxSpawnCooldownInSeconds);

    // Create a parent for enemies hierarchy
    randomGuysParent = new GameObject("Random Guys").transform;

    // Instantiate coconut
    SpawnCoconut();
  }

  void Update()
  {
    // Not a coroutine so we can modify the min/max and use a random more simply
    cooldown -= Time.deltaTime;
    if (cooldown <= 0f)
    {
      cooldown = Random.Range(minSpawnCooldownInSeconds, maxSpawnCooldownInSeconds);
      SpawnGuy();
    }
  }

  /// <summary>
  /// Create a random coconut
  /// </summary>
  private void SpawnCoconut()
  {
    Transform coconut = Instantiate(coconutPrefab, coconutPrefab.position, Quaternion.identity) as Transform;
    coconut.Rotate(new Vector3(0, 0, Random.Range(0.75f, 1.25f)));
  }

  /// <summary>
  /// Create a new moving target
  /// </summary>
  private void SpawnGuy()
  {
    // Randomize betwwen spawns
    Transform spawn = randomGuySpawns[Random.Range(0, randomGuySpawns.Length)];

    // Instantiate
    Transform randomGuy = Instantiate(randomGuyPrefab, spawn.position, Quaternion.identity) as Transform;
    randomGuy.parent = randomGuysParent;

    // Set properties
    RandomGuyScript randomGuyScript = randomGuy.GetComponent<RandomGuyScript>();
    if (randomGuyScript != null)
    {
      randomGuyScript.direction = Mathf.Sign(spawn.localScale.x);
    }
  }
}
