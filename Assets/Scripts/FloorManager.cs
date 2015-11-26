using UnityEngine;
using System.Collections;
using System;


public class FloorManager : MonoBehaviour
{


    [SerializeField]    private GameObject floorObject;

    private GameObject floorA;                // 2D array of floortiles
    private GameObject floorB;                // 2D array of floortiles

    public Color[] colours = { Color.green, Color.yellow, Color.cyan, Color.magenta };
    // Colours my tiles and enemies may randomize to


    public void Awake()
    {
        BuildFloors();
    }


    public void Start()
    {
        BuildSpawners();
    }

    /// <summary>
    /// Builds the floors
    /// </summary>
    void BuildFloors()
    {
        print("build floor A and B");
        floorA = (GameObject)GameObject.Instantiate(floorObject, new Vector3(-12.0f, 0.0f, 0.0f),Quaternion.identity);
        floorA.name = "Floor A";

        floorB = (GameObject)GameObject.Instantiate(floorObject, new Vector3(12.0f, 0.0f, 0.0f), Quaternion.identity);
        floorB.name = "Floor B";
        
    }

    public int GetColoursLength()
    {
        return colours.Length;
    }



    /// <summary>
    /// Builds the Spawners
    /// </summary>
    public void BuildSpawners()
    {
        FloorScript AScript = floorA.GetComponent<FloorScript>();

        AScript.BuildSpawner(0, 0, colours[0], 0);
        AScript.BuildSpawner(0, 8, colours[1], 1);
        AScript.BuildSpawner(8, 0, colours[2], 2);
        AScript.BuildSpawner(8, 8, colours[3], 3);

        FloorScript BScript = floorB.GetComponent<FloorScript>();

        BScript.BuildSpawner(0, 0, colours[0], 0);
        BScript.BuildSpawner(0, 8, colours[1], 1);
        BScript.BuildSpawner(8, 0, colours[2], 2);
        BScript.BuildSpawner(8, 8, colours[3], 3);

        //spawnerManager.CreateSpawner(floorA[0, 0].transform, colours[0], 0);  // Make Red Spawner
        //ActivateTileSet(colours[0]);                                        // Activate red block set
        //spawnerManager.CreateSpawner(floor[0, 8].transform, colours[1], 1);  // Make Yellow Spawner
        //ActivateTileSet(colours[1]);                                        // Activate yellow block set
        //spawnerManager.CreateSpawner(floor[8, 0].transform, colours[2], 2);  // Cyan Spawner
        //ActivateTileSet(colours[2]);                                        // Activate cyan block set
        //spawnerManager.CreateSpawner(floor[8, 8].transform, colours[3], 3);  // Magenta Spawner
        //ActivateTileSet(colours[3]);                                        // Activate magenta block set
    }                                       // Activate magenta block set

}

