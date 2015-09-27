using UnityEngine;
using System.Collections;


public class BlockScript : MonoBehaviour {
       

    private bool isActive;       // When the block is lit up
    private bool isTouched;     // When the block has been stepped on and changed texture
    private Color colour;       // Glow colour, what set the block belongs to

    public Color defaultColour;
    public Texture touchedTexture;  // What block changes to when touch
    public Texture defaultTexture;  // No texture applied after block deactivated

    // Getter for IsActive
    public bool IsActive()
    {
        return isActive;
        
    }

    // Getter for IsTouched
    public bool IsTouched()
    {
        return isTouched;
    }

    // Getter for colour
    public Color GetColour()
    {
        return colour;
    }


    // Initialization
    void Awake () {
        isActive = false;
        isTouched = false;
        colour = defaultColour;
    }
    
    
    // Activates a tile
    public void ActivateTile(Color _colour)
    {
        isActive = true;
        Debug.Log("Set Active -- isActive: " + isActive + "  isTouched: " + isTouched);
        colour = _colour;
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
    }

   
    // Deactivates a Tile
    public void DeactivateTile()
    {
        isActive = false;
        isTouched = false;
        colour = defaultColour;
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
        GetComponent<Renderer>().material.SetTexture("_MKGlowTex", defaultTexture);
    }

    // Touches a tile
    void OnTriggerEnter(Collider other)     // OnTouched
    {
        print("OnTriggerEnter");
        if (isActive && !isTouched && other.tag == "Player") {  // If this tile's active and it hasn't been touched yet and the player entered
            // other.GetComponent<ConstructFloor>().TouchedTile(gameObject);              
            isTouched = true;            
            GetComponent<Renderer>().material.SetTexture("_MKGlowTex", touchedTexture);
            other.GetComponent<FloorScript>().CheckDoneAndDeactivate(colour);
            GetComponent<AudioSource>().Play();
        }

        if (other.tag == "Player")
        {
            Debug.Log("OnTouched -- isActive: " + isActive + "  isTouched: " + isTouched);
        }
    }
	
	// Update is called once per frame
	void Update () {
        


	}
}
