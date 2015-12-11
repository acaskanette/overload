using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnerManager : NetworkBehaviour {


  [SerializeField]
  private GameObject spawnerObject;

  // Some spawners
  private GameObject[] spawnerListA;
  private GameObject[] spawnerListB;

  private int maxNumberColours;
  private int numberOfFloors;

  private int sizeOfGrid;

  // private GameObject player;
  private FloorManager floorManager;

  private StateManager stateManager;

  // Use this for initialization
  void Awake() {

    floorManager = GameObject.FindWithTag("Manager").GetComponent<FloorManager>();

    // Get the size of the spawner array
    maxNumberColours = floorManager.GetColoursLength();
    numberOfFloors = 2;

    // Initialize the spawners
    spawnerListA = new GameObject[maxNumberColours];
    spawnerListB = new GameObject[maxNumberColours];
    for (int i = 0; i < maxNumberColours; i++) {
      spawnerListA[i] = null;
      spawnerListB[i] = null;
    }

    stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();

  }

  public void CreateSpawner(Transform _location, Color _colour, int _index, int spawner) {
    if (spawner == 1) {
      // Create a spawner
      spawnerListA[_index] = (GameObject)GameObject.Instantiate(spawnerObject, _location.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity);
      // Set its colour
      spawnerListA[_index].GetComponent<EnemySpawnScript>().SetSpawnColour(_colour);
    } else if (spawner == 2) {
      // Create a spawner
      spawnerListB[_index] = (GameObject)GameObject.Instantiate(spawnerObject, _location.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity);
      // Set its colour
      spawnerListB[_index].GetComponent<EnemySpawnScript>().SetSpawnColour(_colour);
    }
  }


  public void KillSpawnsOfColour(Color _colour, int spawner) {
    // Cycle through the spawners
    for (int i = 0; i < maxNumberColours; i++) {
      if (spawner == 1 && spawnerListB[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListB[i].GetComponent<EnemySpawnScript>();
        // If spawner is that colour, then call kill on the spawner
        if (tSpawnScript.GetSpawnColour() == _colour) {
          tSpawnScript.KillActiveEnemies();
        }
      } else if (spawner == 2 && spawnerListA[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListA[i].GetComponent<EnemySpawnScript>();
        // If spawner is that colour, then call kill on the spawner
        if (tSpawnScript.GetSpawnColour() == _colour) {
          tSpawnScript.KillActiveEnemies();
        }
      }
    }
  }


  // Check my list of spawner and see if there are any left of that colour with something to spawn
  public bool AnyMoreToSpawn(Color _colour, int spawner) {
    // Cycle through the spawners
    for (int i = 0; i < maxNumberColours; i++) {
      if (spawner == 1 && spawnerListA[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListA[i].GetComponent<EnemySpawnScript>();
        // If spawner is that colour, then call kill on the spawner
        if (tSpawnScript.GetSpawnColour() == _colour) {
          if (tSpawnScript.AnyMoreToSpawn())
            return true;
        }
      }

      if (spawner == 2 && spawnerListB[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListB[i].GetComponent<EnemySpawnScript>();
        // If spawner is that colour, then call kill on the spawner
        if (tSpawnScript.GetSpawnColour() == _colour) {
          if (tSpawnScript.AnyMoreToSpawn())
            return true;
        }
      }
    }

    return false;

  }

  public bool AllSpawnsEmpty(int spawner) {
    //print("Checking all spawns empty");
    bool allEmpty = true;
    FloorManager floor = GameObject.FindGameObjectWithTag("Manager").GetComponent<FloorManager>();
    foreach (Color color in floor.colours) {
      allEmpty = allEmpty && !AnyMoreToSpawn(color, spawner);
      //print(color + " " + !AnyMoreToSpawn(color, spawner) );
    }
    return allEmpty;
  }

  public bool AllEnemiesDead() {
    //print("Checking if all enemies dead");
    // bool allDead = false;
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    return enemies.Length == 0;

  }


  // Take all enemies on the screen and put them back into their Spawner
  public void ResetSpawners() {
    for (int i = 0; i < maxNumberColours; i++) {
      if (spawnerListA[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListA[i].GetComponent<EnemySpawnScript>();
        tSpawnScript.ResetSpawner();
      }
      if (spawnerListB[i] != null) {
        EnemySpawnScript tSpawnScript = spawnerListB[i].GetComponent<EnemySpawnScript>();
        tSpawnScript.ResetSpawner();
      }
    }
  }



  // Update is called once per frame
  void Update() {

  }
}
