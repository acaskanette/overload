using UnityEngine;
using System.Collections;
using System;


public class FloorScript : MonoBehaviour {

    [SerializeField]
    private GameObject floortile;                // What's spawned as my floor
    [SerializeField]
    private GameObject wallTile;                 // What's spawned as my walls
    [SerializeField]
    private GameObject obstacleTile;              // What obstacles are made of
    [SerializeField]
    private int sizeOfGrid = 9;                  // size of the square grid, should be odd
    [SerializeField]
    private int sizeOfTile = 1;                  // physical game world size of the tile

    [SerializeField]
    private const int TILES_PER_SET = 3;         // How many tiles are in a set when they are lit up
    private const int NUMBER_OF_OBSTACLES = 4;

    private System.Random rand;

    private Color[] colours = { Color.green, Color.yellow, Color.cyan, Color.magenta };       
                                                // Colours my tiles may randomize to
   
    private GameObject[,] floor;                // 2D array of floortiles
    private GameObject[] obstacles;             // Array of obstacles (2nd floor)
    private TileScript[,] floorScripts;         // 2D array of the matching floortile's TileScripts

    private SpawnerManager spawnerManager;      // Spawner Manager reference



    // Give the GameObject of the tile at those indices
    public GameObject GetTileByIndex(int _i, int _j) {
        return floor[_i, _j];
    }


    // Find the Tile by GameObject reference
    public Vector2 GetTileIndicesByGameObject(GameObject _tile)
    {

        Vector2 tTileLocation = new Vector2(-999,-999);

        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                if (_tile == floor[i, j])
                {
                    tTileLocation = new Vector2(i, j);
                }
            }
        }
        return tTileLocation;

    }


    // How big my grid of floor is
    public int GetSizeOfGrid()
    {
        return sizeOfGrid;
    }
    
    // How many colours I have in my array 
    public int GetColoursLength()
    {
        return colours.Length;
    }



    /// <summary>
    /// Startup: Instantiate all variables
    /// </summary>
    void Awake () {

        rand = new System.Random();
        spawnerManager = GameObject.FindWithTag("Manager").GetComponent<SpawnerManager>();

        BuildFloor();                           // Builds the main floor of the level and the obstacles (2nd floor)
        BuildWalls();                           // Builds walls around the level   
       
        

    }

    void Start()
    {
        BuildSpawners();                        // Build spawners in the level
    }


    /// <summary>
    /// Builds the floor
    /// </summary>
    void BuildFloor()
    {
        // Instantiate floor struct
        floor = new GameObject[sizeOfGrid, sizeOfGrid];
        obstacles = new GameObject[NUMBER_OF_OBSTACLES];
        floorScripts = new TileScript[sizeOfGrid, sizeOfGrid];

        int startPoint = -(sizeOfGrid / 2); // upper left bound of the grid
        
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {

                floor[i,j] = (GameObject)(GameObject.Instantiate(floortile, new Vector3((startPoint + i) * sizeOfTile, 0, (startPoint + j) * sizeOfTile), Quaternion.identity));                
                floorScripts[i,j] = floor[i,j].GetComponent<TileScript>();

            }
        }

        for (int i = 0; i < 4; i++)
        {
            int x = rand.Next(startPoint+1, -startPoint);
            int z = rand.Next(startPoint+1, -startPoint);
            if ( !( x == 0 && z == 0) )     // Unless it's the center tile
                obstacles[i] = (GameObject)(GameObject.Instantiate(obstacleTile, new Vector3(x * sizeOfTile, 1.5f, z * sizeOfTile), Quaternion.identity));                
        }
    }


    /// <summary>
    /// Builds the Walls
    /// </summary>
    void BuildWalls()
    {

        int startPoint = -(sizeOfGrid / 2 + 1);     // diagonal from outer corner of floor

        // Make walls
        for (int i = 0; i < sizeOfGrid + 2; i++)
        {
            GameObject.Instantiate(wallTile, new Vector3((startPoint + i) * sizeOfTile, 2.0f, sizeOfGrid + 1), Quaternion.identity);    // top wall
            GameObject.Instantiate(wallTile, new Vector3((startPoint + i) * sizeOfTile, 2.0f, -(sizeOfGrid + 1)), Quaternion.identity); // bottom wall
            
            if (i != 0 || i != (sizeOfGrid + 1))
            {
                GameObject.Instantiate(wallTile, new Vector3(-(sizeOfGrid + 1), 2.0f, (startPoint + i) * sizeOfTile), Quaternion.identity); // side wall
                GameObject.Instantiate(wallTile, new Vector3((sizeOfGrid + 1), 2.0f, (startPoint + i) * sizeOfTile), Quaternion.identity);  // other side wall
            }

        }
    }


    /// <summary>
    /// Builds the Spawners
    /// </summary>
    void BuildSpawners()
    {        
        spawnerManager.CreateSpawner(floor[0,0].transform, colours[0], 0);  // Make Red Spawner
        ActivateTileSet(colours[0]);                                        // Activate red block set
        spawnerManager.CreateSpawner(floor[0,8].transform, colours[1], 1);  // Make Yellow Spawner
        ActivateTileSet(colours[1]);                                        // Activate yellow block set
        spawnerManager.CreateSpawner(floor[8,0].transform, colours[2], 2);  // Cyan Spawner
        ActivateTileSet(colours[2]);                                        // Activate cyan block set
        spawnerManager.CreateSpawner(floor[8,8].transform, colours[3], 3);  // Magenta Spawner
        ActivateTileSet(colours[3]);                                        // Activate magenta block set
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
    private GameObject GetInactiveTile () {
      
        // Randomize a tile
        int x, y;
        x = rand.Next(0, sizeOfGrid);
        y = rand.Next(0, sizeOfGrid);
                
        if ( floorScripts[x,y].IsActive() || (x==sizeOfGrid/2 && y==sizeOfGrid/2) || IsUnderAnObstacle(x,y))       // If it's Active or the center tile or under an obstacle
            return GetInactiveTile();      // Get a new one                   
        else
            return floor[x, y];            // Otherwise, return it

    }



    /// <summary>
    /// Returns true if the tile at location x, y is directly below an obstacle
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsUnderAnObstacle(int x, int y)
    {
        bool isUnder = false;

        for (int i = 0 ; i < NUMBER_OF_OBSTACLES ; i++) {
            if (floor[x, y].transform.position.x == obstacles[i].transform.position.x && floor[x, y].transform.position.z == obstacles[i].transform.position.z)
                isUnder = true;
        }
        
        return isUnder;
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
                allActive = allActive && floorScripts[i,j].IsActive();
            }
        }
        return allActive;
    }


    /// <summary>
    /// Activates a tile, causing it to glow that colour.
    /// </summary>
    /// <param name="_floorTile">Which tile to activate.</param>    
    /// <param name="_color">Colour the tile will glow.</param>
    private void ActivateTile(GameObject _floorTile, Color _colour) {
       
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
                if (floorScripts[i,j].GetColour() == _colour)      // If the tile is the colour I'm checking
                {
                    allTouched = allTouched && floorScripts[i,j].IsTouched();    // Then see if it's all touched
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
        if (spawnerManager.CheckAnyMoreToSpawn(_colour))
        // Create a new set of tiles of this colour (if true)
        {
            print("Activating Tileset after a kill");
            ActivateTileSet(_colour);
        }
        
    }
   


	/// <summary>
	/// Update the floor
	/// </summary>
	void Update () {

        if ( Input.GetKeyDown(KeyCode.R))
        {
            int randomColour = rand.Next(0, colours.Length);
            ActivateTileSet(colours[randomColour]);
        }
    }
}
