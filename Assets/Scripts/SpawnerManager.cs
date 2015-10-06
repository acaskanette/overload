using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour {


    [SerializeField]
    private GameObject spawnerObject;

    // Some spawners
    private GameObject[] spawnerList;
    //private Transform[] spawnerLocations;

    private int maxNumberColours;

    private int sizeOfGrid;

   // private GameObject player;
    private FloorScript playerFloorScript;

	// Use this for initialization
	void Awake () {

        playerFloorScript = GameObject.FindWithTag("Manager").GetComponent<FloorScript>();

        // Get the size of the spawner array
        maxNumberColours = playerFloorScript.GetColoursLength();

        // Initialize the spawners
        spawnerList = new GameObject[maxNumberColours];
        for (int i = 0; i < spawnerList.Length; i++)
        {
            spawnerList[i] = null;
        }

	}

    public void CreateSpawner(Transform _location, Color _colour, int _index)
    {
        // Create a spawner
        spawnerList[_index] = (GameObject)GameObject.Instantiate(spawnerObject, _location.position, Quaternion.identity);
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
    }
  

    // Check my list of spawner and see if there are any left of that colour with something to spawn
    public bool CheckAnyMoreToSpawn(Color _colour)
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



	
	// Update is called once per frame
	void Update () {
	
	}
}
