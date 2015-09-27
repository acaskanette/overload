using UnityEngine;
using System.Collections;
using System;


public class FloorScript : MonoBehaviour {

    public GameObject floortile;  

    public int sizeOfGrid = 9; // should be odd
    public int sizeOfTile = 1;

    public const int TILES_PER_SET = 3;

    private System.Random rand;

    private Color[] colours = { Color.red, Color.yellow, Color.cyan, Color.magenta };
   
    private GameObject[,] floor;    // 2D array of floortiles
    


    /// <summary>
    /// Startup: Instantiate all variables
    /// </summary>
    void Start () {

        rand = new System.Random();

        BuildFloor();                       // Builds the main floor of the level
        BuildWalls();                       // Builds walls around the level            
        ActivateTileSet(rand.Next(0,colours.Length));                 // Activate one block set

    }


    /// <summary>
    /// Builds the floor
    /// </summary>
    void BuildFloor()
    {
        // Instantiate floor struct
        floor = new GameObject[sizeOfGrid, sizeOfGrid];

        int startPoint = -(sizeOfGrid / 2); // upper left bound of the grid
        
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {

                floor[i, j] = (GameObject)(GameObject.Instantiate(floortile, new Vector3((startPoint + i) * sizeOfTile, 0, (startPoint + j) * sizeOfTile), Quaternion.identity));                

            }
        }
    }

    void BuildWalls()
    {

        int startPoint = -(sizeOfGrid / 2 + 1);     // diagonal from outer corner of floor

        // Make walls
        for (int i = 0; i < sizeOfGrid + 2; i++)
        {
            GameObject.Instantiate(floortile, new Vector3((startPoint + i) * sizeOfTile, 0.5f, sizeOfGrid + 1), Quaternion.identity);
            GameObject.Instantiate(floortile, new Vector3((startPoint + i) * sizeOfTile, 0.5f, -(sizeOfGrid + 1)), Quaternion.identity);
            
            if (i != 0 || i != (sizeOfGrid + 1))
            {
                GameObject.Instantiate(floortile, new Vector3(-(sizeOfGrid + 1), 0.5f, (startPoint + i) * sizeOfTile), Quaternion.identity);
                GameObject.Instantiate(floortile, new Vector3((sizeOfGrid + 1), 0.5f, (startPoint + i) * sizeOfTile), Quaternion.identity);
            }

        }
    }


    void ActivateTileSet(int randomColour)
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
                ActivateTile(activeTile, colours[randomColour]);
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

        BlockScript tileScript = floor[x, y].GetComponent<BlockScript>();
        
        if ( tileScript.IsActive() )       // If it's Active
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
                BlockScript tileScript = floor[i, j].GetComponent<BlockScript>();
                allActive = allActive && tileScript.IsActive();
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
        print("ActivateTile");
        BlockScript tileScript = _floorTile.GetComponent<BlockScript>();
        tileScript.ActivateTile(_colour);
                       
    }


      
    /// <summary>
    /// Resets a tile to default.
    /// </summary>
    /// <param name="_floorTile"></param>
    /// <param name="_color"></param>
    private void DeActivateTile(GameObject _floorTile)
    {
        _floorTile.GetComponent<BlockScript>().DeactivateTile();         
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
        
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                BlockScript tileScript = floor[i, j].GetComponent<BlockScript>();
                if (tileScript.GetColour() == _colour)      // If the tile is the colour I'm checking
                {
                    allTouched = allTouched && tileScript.IsTouched();    // Then see if it's all touched
                    tTilesOfColourSet[tColoursIndex] = floor[i, j];
                    tColoursIndex++;
                }
            }
        }

        if (allTouched)
        {
            for (int t=0; t < TILES_PER_SET; t++)
            {
               DeActivateTile(tTilesOfColourSet[t]);
               
            }
        }

    }

	
	void Update () {

        if ( Input.GetKeyDown(KeyCode.R))
        {
            int randomColour = rand.Next(0, colours.Length);
            ActivateTileSet(randomColour);
        }
    }
}
