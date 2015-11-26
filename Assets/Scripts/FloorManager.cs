using UnityEngine;
using System.Collections;
using System;


public class FloorManager : MonoBehaviour
{

    [SerializeField]
    private const int TILES_PER_SET = 3;         // How many tiles are in a set when they are lit up

    private System.Random rand;

    // Colours my tiles may randomize to
    public Color[] colours = { Color.green, Color.yellow, Color.cyan, Color.magenta };

    private FloorScript floorA;                // 2D array of floortiles
    private FloorScript floorB;                // 2D array of floortiles

    private TileScript[,] floorScriptsA;         // 2D array of the matching floortile's TileScripts
    private TileScript[,] floorScriptsB;         // 2D array of the matching floortile's TileScripts

    private SpawnerManager spawnerManager;      // Spawner Manager reference

    [SerializeField]
    private GameObject deactivateParticleEffect;


    // How many colours I have in my array 
    public int GetColoursLength()
    {
        return colours.Length;
    }

    /// <summary>
    /// Startup: Instantiate all variables
    /// </summary>
    void Awake()
    {

        rand = new System.Random();
        spawnerManager = GameObject.FindWithTag("Manager").GetComponent<SpawnerManager>();

    }

    void Start()
    {
        
    }


    /// <summary>
    /// Builds the floor
    /// </summary>
    void BuildFloors()
    {
        floorA = new FloorScript();
        floorB = new FloorScript();        

    }       

    /// <summary>
    /// Activates a tile set of the given colour
    /// </summary>
    /// <param name="_colour">index of the colour array</param>
    void ActivateTileSet(Color _colour)
    {

        GameObject activeTile;                                  // FloorTile to light up

        //int randomColour = rand.Next(0, colours.Length);       // Colour to assign, update later when enemies are finished


        // For as many blocks as there are in a set....
        for (int b = 0; b < TILES_PER_SET; b++)
        {

            if (!AllActive())  // As long as there is an active block out there
            {
                // Grab a random tile
                activeTile = GetInactiveTile();
                // Activate it
                ActivateTile(activeTile, _colour);
            }
        }


    }

    /// <summary>
    /// Gets a random inactive tile from the floor
    /// </summary>
    /// <returns>A random inactive tile, recursively if the randomizer found an active one</returns>
    private GameObject GetInactiveTile()
    {

        // Randomize a tile
        int x, y;
        x = rand.Next(0, sizeOfGrid);
        y = rand.Next(0, sizeOfGrid);

        if (floorScripts[x, y].IsActive() || (x == sizeOfGrid / 2 && y == sizeOfGrid / 2))       // If it's Active or the center tile or under an obstacle
            return GetInactiveTile();      // Get a new one                   
        else
            return floor[x, y];            // Otherwise, return it

    }





    /// <summary>
    /// Returns if all Floor tiles are active (should almost never happen, but may if you stand still for a while)
    /// </summary>
    /// <returns>True if all tiles are Active or Touched.</returns>
    private bool AllActive()
    {
        bool allActive = true;
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                allActive = allActive && floorScripts[i, j].IsActive();
            }
        }
        return allActive;
    }


    /// <summary>
    /// Activates a tile, causing it to glow that colour.
    /// </summary>
    /// <param name="_floorTile">Which tile to activate.</param>    
    /// <param name="_color">Colour the tile will glow.</param>
    private void ActivateTile(GameObject _floorTile, Color _colour)
    {

        //print("ActivateTile");
        _floorTile.GetComponent<TileScript>().ActivateTile(_colour);

    }



    /// <summary>
    /// Resets a tile to default.
    /// </summary>
    /// <param name="_floorTile"></param>
    /// <param name="_color"></param>
    private void DeActivateTile(GameObject _floorTile)
    {
        _floorTile.GetComponent<TileScript>().DeactivateTile();
        GameObject deactivateParticle = (GameObject)GameObject.Instantiate(deactivateParticleEffect, _floorTile.transform.position + Vector3.up, Quaternion.identity);
        //deactivateParticle.GetComponent<ParticleSystem>().startColor = _floorTile.GetComponent<TileScript>().GetColour();
        deactivateParticle.GetComponentInChildren<ParticleSystem>().startColor = _floorTile.GetComponent<TileScript>().GetColour();
    }


    /// <summary>
    /// Check if that set has all been touched, and if so deactivate them.
    /// </summary>
    /// <param name="_colour">Set I am checking</param>
    public void CheckDoneAndDeactivate(Color _colour)
    {
        bool allTouched = true;
        GameObject[] tTilesOfColourSet = new GameObject[TILES_PER_SET];

        int tColoursIndex = 0;
        int numberTouched = 0;

        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                if (floorScripts[i, j].GetColour() == _colour)      // If the tile is the colour I'm checking
                {
                    allTouched = allTouched && floorScripts[i, j].IsTouched();    // Then see if it's all touched
                    tTilesOfColourSet[tColoursIndex] = floor[i, j];
                    tColoursIndex++;
                    if (floorScripts[i, j].IsTouched())
                        numberTouched++;
                }
            }
        }

        for (int p = 0; p < tTilesOfColourSet.Length; p++)
        {
            float pitch = 0.8f + 0.2f * numberTouched;
            tTilesOfColourSet[p].GetComponent<TileScript>().ChangePitch(pitch);
        }

        // If all of the blocks in the set have been touched
        if (allTouched)
        {

            // Kills all the enemies of that colour
            spawnerManager.KillSpawnsOfColour(_colour);

            // Deactivate all the Tiles of this colour
            for (int t = 0; t < TILES_PER_SET; t++)
            {
                //print("deactivating tile: " + t);
                DeActivateTile(tTilesOfColourSet[t]);       // Deactivate them all
            }

            StartCoroutine(CheckRespawn(_colour));

        }

    }




    private IEnumerator CheckRespawn(Color _colour)
    {

        yield return new WaitForSeconds(3.1f);

        // Check if there are any more enemies of this colour waiting to be spawned
        if (spawnerManager.AnyMoreToSpawn(_colour))
        // Create a new set of tiles of this colour (if true)
        {
            print("Activating Tileset after a kill");
            ActivateTileSet(_colour);
        }

    }


    public void ResetLevel()
    {
        ClearFloor();
        ClearWalls();
        ClearSpawners();
        Start();
    }

    void ClearFloor()
    {
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                GameObject.Destroy(floor[i, j]);
                floor[i, j] = null;
            }
        }
    }

    void ClearWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            GameObject.Destroy(wall);
        }
    }

    void ClearSpawners()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in spawners)
        {
            GameObject.Destroy(spawner);
        }
    }


    /// <summary>
    /// Update the floor
    /// </summary>
    void Update()
    {


    }
}

