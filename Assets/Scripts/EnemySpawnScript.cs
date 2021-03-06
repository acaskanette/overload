﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawnScript : NetworkBehaviour {


  public const float TIME_BETWEEN_SPAWNS = 5.0f;  // in seconds
  public const int MAX_ENEMIES_AT_ONE_TIME = 2;   // how many enemies this spawner can control
  public const int MAX_NUMBER_OF_SPAWNS = 99;      // how many I'll spawn before I stop spawning

  public GameObject enemyGameObject;              // What enemy am I spawning

  private GameObject[] spawnedEnemies;            // References to the enemies I have spawned
  private int spawnIndex = 0;                     // Next empty spot in spawnedEnemies array
  private int numberSpawned = 0;                  // How many enemies spawned so far

  private Color spawnColour;                      // Colour of enemies spawned

  float timeBetweenSpawns = 2.0f;                 // Default seconds between enemies spawn

  [SerializeField]
  GameObject enemySnipedEffect;

  StateManager stateManager;

  // Setter for spawnColour;
  public void SetSpawnColour(Color _spawnColour) {
    spawnColour = _spawnColour;
  }

  // Getter for spawnColour;
  public Color GetSpawnColour() {
    return spawnColour;
  }

  // Use this for initialization
  void Start() {

    stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();

    spawnedEnemies = new GameObject[MAX_ENEMIES_AT_ONE_TIME];

    // print("Start");
    for (int i = 0; i < spawnedEnemies.Length; i++) {
      spawnedEnemies[i] = null;
    }

    spawnIndex = 0;

  }

  /// <summary>
  /// Spawn a single enemy of the type that my spawner is
  /// </summary>
  void Spawn() {

    //print("Spawning #" + numberSpawned);

    if (spawnIndex != -1 && numberSpawned < MAX_NUMBER_OF_SPAWNS)  // if the array is not full and I haven't spawned too much
    {
      // Increase how many enemies this spawner spawned so far
      // print("Spawn");
      numberSpawned++;
      // Spawn an enemy

      GameObject tSpawnedEnemy = (GameObject)(GameObject.Instantiate(enemyGameObject, transform.position, Quaternion.identity));
      EnemyScript enemy = tSpawnedEnemy.GetComponent<EnemyScript>();
      enemy.SetColour(spawnColour);
      if (spawnColour == Color.magenta || spawnColour == Color.green)
        enemy.SetAIState(EnemyScript.AIState.DivePinpoint);
      else if (spawnColour == Color.yellow || spawnColour == Color.cyan)
        enemy.SetAIState(EnemyScript.AIState.Orbit);

      // Saves it into my spawnedEnemy array
      spawnedEnemies[spawnIndex] = tSpawnedEnemy;
      // Find out where I can spawn another one
      spawnIndex = NextSpawnIndex();
    }
  }


  /// <summary>
  /// Finds the next empty spot in my spawnedEnemyArray
  /// </summary>
  /// <returns>-1 if the array is full ; the index of the empty one if it is not full</returns>
  private int NextSpawnIndex() {
    //print("NextSpawnIndex");
    int tIndex = -1;    // -1 mean "it's full"
    for (int i = 0; i < spawnedEnemies.Length; i++) {
      if (spawnedEnemies[i] == null)
        tIndex = i; // Otherwise an empty spot in the array is given
    }
    //print("Spawn Index Set to: " + tIndex);
    return tIndex;
  }


  /// <summary>
  /// Kill all enemies in the spawnedEnemiesArray
  /// </summary>
  public void KillActiveEnemies() {
    //print("KillActiveEnemies");       
    StartCoroutine(MakeDeathSounds());
  }

  IEnumerator MakeDeathSounds() {
    //print("DeathSoundStarted");
    AudioSource firework = GetComponent<AudioSource>();

    firework.Play();
    for (int i = 0; i < spawnedEnemies.Length; i++) {
      if (spawnedEnemies[i] != null)
        spawnedEnemies[i].GetComponent<EnemyScript>().SetAIState(EnemyScript.AIState.Dying);
    }

    yield return new WaitForSeconds(0.7f);

    //print("Miniwait");
    firework.mute = true;

    yield return new WaitForSeconds(1.7f);

    for (int i = 0; i < spawnedEnemies.Length; i++) {
      if (spawnedEnemies[i] != null) {
        GameObject.Instantiate(enemySnipedEffect, spawnedEnemies[i].gameObject.transform.position + Vector3.up * 6.0f, Quaternion.identity);
      }
    }


    yield return new WaitForSeconds(0.7f);

    firework.mute = false;

    for (int i = 0; i < spawnedEnemies.Length; i++) {
      if (spawnedEnemies[i] != null) {
        spawnedEnemies[i].GetComponent<EnemyScript>().Kill();
        spawnedEnemies[i] = null;
      }
    }

    spawnIndex = 0;

    // print("DeathSoundEnded");
  }


  /// <summary>
  /// Determines if there are any more enemies left in this spawner to spawn
  /// Called by: FloorScript
  /// </summary>
  /// <returns>bool true if I have more enemies to spawn</returns>
  public bool AnyMoreToSpawn() {
    return (numberSpawned < MAX_NUMBER_OF_SPAWNS);
  }


  /// <summary>
  /// Take all the active enemies and put them back into the spawner to be spawned again
  /// </summary>
  public void ResetSpawner() {
    for (int i = 0; i < spawnedEnemies.Length; i++) {
      if (spawnedEnemies[i] != null) {
        numberSpawned--;
        Destroy(spawnedEnemies[i]);
        spawnedEnemies[i] = null;
      }
    }
    spawnIndex = NextSpawnIndex();
  }

  // Update is called once per frame
  void Update() {

    if (stateManager.currentState == StateManager.GameState.PLAYING_STATE) {

      timeBetweenSpawns += Time.deltaTime;

      if (timeBetweenSpawns > TIME_BETWEEN_SPAWNS) {
        // print("CalledSpawnFromUpdate");
        Spawn();
        timeBetweenSpawns = 0.0f;
      }

    }

  }
}
