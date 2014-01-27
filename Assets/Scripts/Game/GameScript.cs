using UnityEngine;
using System.Collections;

/// <summary>
/// Core gameplay script
/// </summary>
public class GameScript : MonoBehaviour
{
  //------------------------------------------------
  // Gameplay
  //------------------------------------------------

  /// <summary>
  /// Total time 
  /// </summary>
  public float time = 60f;

  /// <summary>
  /// Given time between two combos
  /// </summary>
  public float comboBaseCooldown = 5f;

  /// <summary>
  /// Points per kill
  /// </summary>
  public int points = 100;

  //------------------------------------------------
  // Coconut
  //------------------------------------------------

  /// <summary>
  /// Model for coconuts
  /// </summary>
  public Transform coconutPrefab;

  /// <summary>
  /// Where to spawn coconuts
  /// </summary>
  public Transform coconutSpawn;

  /// <summary>
  /// Coconut respawn
  /// </summary>
  public float respawnTimeInSeconds = 1.5f;

  //------------------------------------------------
  // Random guy
  //------------------------------------------------

  /// <summary>
  /// Model for random guys
  /// </summary>
  public Transform randomGuyPrefab;

  /// <summary>
  /// Where to spawn 
  /// </summary>
  public Transform[] randomGuySpawns;

  /// <summary>
  /// Random guy spawn frequency (min)
  /// </summary>
  public float minSpawnCooldownInSeconds = 0.45f;

  /// <summary>
  /// Random guy spawn frequency (max)
  /// </summary>
  public float maxSpawnCooldownInSeconds = 1.25f;

  //------------------------------------------------
  // Bonuses  
  //------------------------------------------------
  public BonusScript bonusPrefab;
  public float minBonusSpawnFrequency = 5f;
  public float maxBonusSpawnFrequency = 8f;

  private bool isEnded;

  private float timeleft;
  private int score;
  private int combo;
  private Transform randomGuysParent, bonusParent;
  private float enemySpawnCooldown, comboCooldown, bonusCooldown;

  private GameGUI gui;

  void Start()
  {
    isEnded = false;

    // Check parameters
    if (randomGuySpawns == null || randomGuySpawns.Length == 0) Debug.LogError("Missing randomGuySpawns!");
    if (randomGuyPrefab == null) Debug.LogError("Missing RandomGuy prefab!");
    if (bonusPrefab == null) Debug.LogError("Missing Bonus prefab!");
    if (coconutSpawn == null) Debug.LogError("Missing coconutSpawn!");

    gui = FindObjectOfType<GameGUI>();
    if (gui == null) Debug.LogError("Missing GUI!");

    // Initialize
    enemySpawnCooldown = Random.Range(minSpawnCooldownInSeconds, maxSpawnCooldownInSeconds);
    bonusCooldown = Random.Range(minBonusSpawnFrequency, maxBonusSpawnFrequency);

    timeleft = time;
    score = 0;
    combo = 1;

    // Create a parent for a proper hierarchy
    randomGuysParent = new GameObject("Random Guys").transform;
    bonusParent = new GameObject("Bonuses").transform;

    // Instantiate coconut
    StartCoroutine(RespawnCoconut(1f));

    gui.SetVisible(true);
  }

  void Update()
  {
    //------------------------------------
    // Ingame
    //------------------------------------
    if (isEnded == false)
    {
      // Not a coroutine so we can modify the min/max and use a random more simply
      enemySpawnCooldown -= Time.deltaTime;
      if (enemySpawnCooldown <= 0f)
      {
        enemySpawnCooldown = Random.Range(minSpawnCooldownInSeconds, maxSpawnCooldownInSeconds);
        SpawnGuy();
      }

      // Same for bonus
      bonusCooldown -= Time.deltaTime;
      if (bonusCooldown <= 0f)
      {
        bonusCooldown = Random.Range(minBonusSpawnFrequency, maxBonusSpawnFrequency);
        SpawnBonus();
      }

      // Reset combo is cooldown drops to 0
      if (comboCooldown > 0)
      {
        comboCooldown -= Time.deltaTime;
        if (comboCooldown <= 0f)
          combo = 1;
      }

      timeleft -= Time.deltaTime;

      gui.UpdateGUI(timeleft, score, combo);

      // Time is over?
      if (timeleft < 0)
      {
        isEnded = true;
        gui.SetVisible(false);
      }
    }
    //------------------------------------
    // End
    //------------------------------------
    else
    {
      // Display score, whatever
      // DEBUG: RELOAD
      Application.LoadLevel("Game");
    }

  }

  /// <summary>
  /// 
  /// </summary>
  public void CoconutDestroyed()
  {
    // Regenerate a coconut in few seconds
    StartCoroutine(RespawnCoconut(respawnTimeInSeconds));
  }

  IEnumerator RespawnCoconut(float cooldown)
  {
    yield return new WaitForSeconds(cooldown);

    SpawnCoconut();

    yield return null;
  }

  /// <summary>
  /// Create a random coconut
  /// </summary>
  private void SpawnCoconut()
  {
    Transform coconut = Instantiate(coconutPrefab, coconutSpawn.position, Quaternion.identity) as Transform;
    coconut.Rotate(new Vector3(0, 0, Random.Range(0.75f, 1.25f)));
    //coconut.parent = coconutSpawn;
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

  /// <summary>
  /// New bonus
  /// </summary>
  private void SpawnBonus()
  {
    BonusScript bonus = Instantiate(bonusPrefab) as BonusScript;
    bonus.SetRandomType();

    // Location on screen
    bonus.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.15f, 0.8f), Random.Range(0.15f, 0.8f), 0));
    bonus.transform.position = new Vector3(bonus.transform.position.x, bonus.transform.position.y, 0); // Reset 0
    bonus.transform.parent = bonusParent;
  }

  public void GuyDestroyed()
  {
    // Score
    score += (points * combo);

    // Combo
    comboCooldown = comboBaseCooldown;
    combo++;
  }

  /// <summary>
  /// Current combo count
  /// </summary>
  public int ComboCount
  {
    get
    {
      return combo;
    }
  }

  public GameGUI GameGUI
  {
    get
    {
      return gui;
    }
  }

}
