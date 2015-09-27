using UnityEngine;
using System.Collections;
using System;


//struct FloorTile {

//    public GameObject tile;         // The GameObject that stores the Tile itself
//    public bool isActive;           // Whether or not this Tile is lit up
//    public bool isTouched;          // Whether or not this Tile is "on", when all of a colour are on it kills all enemies of that colour
//    public Color color;             // Color of the tile currently

//}


public class ConstructFloor : MonoBehaviour {

    public GameObject floortile;
  

    public int sizeOfGrid = 9; // should be odd
    public int sizeOfTile = 1;

    public const int BLOCKS_PER_COLOUR = 3;

    private System.Random rand;
    private Color[] colours = { Color.red, Color.yellow, Color.cyan, Color.magenta };
   
    private GameObject[,] floor;
    


    /// <summary>
    /// Startup: Instantiate all variables
    /// </summary>
    void Start () {

        rand = new System.Random();

        BuildFloor();                       // Builds the main floor of the level
        BuildWalls();                       // Builds walls around the level            
        ActivateBlockSet();                 // Activate one block set

    }


    /// <summary>
    /// Builds the floor
    /// </summary>
    void BuildFloor()
    {
        // Instantiate floor struct
        floor = new GameObject[sizeOfGrid, sizeOfGrid];
        int startPoint = -(sizeOfGrid / 2);
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {

                floor[i, j] = (GameObject)(GameObject.Instantiate(floortile, new Vector3((startPoint + i) * sizeOfTile, 0, (startPoint + j) * sizeOfTile), Quaternion.identity));
                //// floor[i,j].tile.isActive = false;
                //floor[i, j].isTouched = false;
                //floor[i, j].color = defaultColour;

            }
        }
    }

    void BuildWalls()
    {

        int startPoint = -(sizeOfGrid / 2 + 1);

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


    void ActivateBlockSet()
    {

        GameObject activeTile;                                  // FloorTile to light up

        int randomColour = rand.Next(0, colours.Length);       // Colour to assign, update later when enemies are finished


        // For as many blocks as there are in a set....
        for (int b = 0; b < BLOCKS_PER_COLOUR; b++)
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

        ColorTrigger tileScript = floor[x, y].GetComponent<ColorTrigger>();
        
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
                ColorTrigger tileScript = floor[i,j].GetComponent<ColorTrigger>();
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
        print("activate-construct00");
        ColorTrigger tileScript = _floorTile.GetComponent<ColorTrigger>();
        tileScript.ActivateTile(_colour);
                       
    }



    /// <summary>
    /// Touches a tile.
    /// </summary>
    /// <param name="tileTouched"></param>
    //public void TouchedTile(GameObject tileTouched) {

    //    for (int i = 0; i < sizeOfGrid; i++) {
    //        for (int j = 0; j < sizeOfGrid; j++) {
    //            //Debug.Log("" + i + j + " " + floor[i,j].isActive);
    //            if (tileTouched == floor[i,j].tile) {

    //                // Debug.Log("Tile: " + i + " " + j + " ..." + floor[i, j].isActive);

    //                ColorTrigger tileScript = floor[i, j].tile.GetComponent<ColorTrigger>();
    //                if (floor[i,j].tile.GetComponent<ColorTrigger>().isActive && !floor[i,j].isTouched)
    //                {                        
    //                    floor[i,j].tile.GetComponent<AudioSource>().Play();
    //                    floor[i,j].tile.GetComponent<Renderer>().material.SetTexture("_MKGlowTex", pressedTexture);
    //                    floor[i,j].isTouched = true;
    //                }
    //            }
    //        }
    //    }

    //}

    
        
    /// <summary>
    /// Resets a tile to default.
    /// </summary>
    /// <param name="_floorTile"></param>
    /// <param name="_color"></param>
    private void DeActivateTile(GameObject _floorTile)
    {

        _floorTile.GetComponent<ColorTrigger>().DeactivateTile(); 
        //= false;
        //_floorTile.isTouched = false;
        //_floorTile.color = defaultColour;
        //_floorTile.tile.GetComponent<Renderer>().material.SetColor("_MKGlowColor", defaultColour);

    }

    public void CheckDoneAndDeactivate(Color _colour)
    {
        bool allDone = true;
        GameObject[] tColours = new GameObject[BLOCKS_PER_COLOUR];
        int tColoursIndex = 0;
        for (int i = 0; i < sizeOfGrid; i++)
        {
            for (int j = 0; j < sizeOfGrid; j++)
            {
                ColorTrigger tileScript = floor[i, j].GetComponent<ColorTrigger>();
                if (tileScript.GetColour() == _colour)
                {
                    allDone = allDone && tileScript.IsTouched();
                    tColours[tColoursIndex] = floor[i, j];
                    tColoursIndex++;
                }
            }
        }

        if (allDone)
        {
            for (int t=0; t < BLOCKS_PER_COLOUR; t++)
            {
               DeActivateTile(tColours[t]);
            }
        }

    }

	
	void Update () {

        if ( Input.GetKeyDown(KeyCode.R))
        {
            int randomColour = rand.Next(0, colours.Length);
            ActivateBlockSet();
        }
    }
}
