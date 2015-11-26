using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour {


    [SerializeField]
    private GameObject spawnerObject;

    // Some spawners
    private GameObject[] spawnerList;    

    private int maxNumberColours;

    private int sizeOfGrid;

   // private GameObject player;
    private FloorManager floorManager;

    private StateManager stateManager;

	// Use this for initialization
	void Awake () {

        floorManager = GameObject.FindWithTag("Manager").GetComponent<FloorManager>();

        // Get the size of the spawner array
        maxNumberColours = floorManager.GetColoursLength();

        // Initialize the spawners
        spawnerList = new GameObject[maxNumberColours];
        for (int i = 0; i < spawnerList.Length; i++)
        {
            spawnerList[i] = null;
        }

        stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();

	}

    public void CreateSpawner(Transform _location, Color _colour, int _index)
    {
        // Create a spawner
        spawnerList[_index] = (GameObject)GameObject.Instantiate(spawnerObject, _location.position + new Vector3(0.0f,1.5f,0.0f), Quaternion.identity);
        // Set its colour
        spawnerList[_index].GetComponent<EnemySpawnScript>().SetSpawnColour(_colour);
    }


    public void KillSpawnsOfColour(Color _colour)
    {
        // Cycle through the spawners
        for (int i = 0; i < spawnerList.Length; i++)
        {
            if (spawnerList[i] != null)
            {
                EnemySpawnScript tSpawnScript = spawnerList[i].GetComponent<EnemySpawnScript>();
                // If spawner is that colour, then call kill on the spawner
                if (tSpawnScript.GetSpawnColour() == _colour)
                {                    
                    tSpawnScript.KillActiveEnemies();
                    
                }
            }
        }

        if (AllSpawnsEmpty() && AllEnemiesDead())
        {
            stateManager.currentState = StateManager.GameState.VICTORY_STATE;
            print("All Spawns Empty and All Enemies Dead");
        }     
    }
  

    // Check my list of spawner and see if there are any left of that colour with something to spawn
    public bool AnyMoreToSpawn(Color _colour)
    {        
        // Cycle through the spawners
        for (int i = 0; i < spawnerList.Length; i++)
        {
            if (spawnerList[i] != null)
            {
                EnemySpawnScript tSpawnScript = spawnerList[i].GetComponent<EnemySpawnScript>();
                // If spawner is that colour, then call kill on the spawner
                if (tSpawnScript.GetSpawnColour() == _colour)
                {
                    if (tSpawnScript.AnyMoreToSpawn())
                        return true;
                }
            }
        }

        return false;

    }

    public bool AllSpawnsEmpty()
    {
        print("Checking all spawns empty");
        bool allEmpty = true;
        FloorManager floor = GameObject.FindGameObjectWithTag("Manager").GetComponent<FloorManager>();
        foreach (Color color in floor.colours)
        {
            allEmpty = allEmpty && !AnyMoreToSpawn(color);
            print(color + " " + !AnyMoreToSpawn(color) );
        }
        return allEmpty;
    }

    public bool AllEnemiesDead()
    {
        print("Checking if all enemies dead");
        // bool allDead = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;

    }


    // Take all enemies on the screen and put them back into their Spawner
    public void ResetSpawners()
    {
        for (int i = 0; i < spawnerList.Length; i++)
        {
            if (spawnerList[i] != null)
            {
                EnemySpawnScript tSpawnScript = spawnerList[i].GetComponent<EnemySpawnScript>();
                tSpawnScript.ResetSpawner();
            }
        }
    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
