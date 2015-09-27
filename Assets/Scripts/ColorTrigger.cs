using UnityEngine;
using System.Collections;


public class ColorTrigger : MonoBehaviour {
       

    public bool isActive;
    private bool isTouched;
    private Color colour;

    public Color defaultColour;
    public Texture touchedTexture;
    public Texture defaultTexture;

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
        if (isActive && !isTouched && other.tag == "Player") {
            // other.GetComponent<ConstructFloor>().TouchedTile(gameObject);              
            isTouched = true;            
            GetComponent<Renderer>().material.SetTexture("_MKGlowTex", touchedTexture);
            other.GetComponent<ConstructFloor>().CheckDoneAndDeactivate(colour);
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
