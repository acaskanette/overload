﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class TileScript : NetworkBehaviour {
       

    private bool isActive;      // When the block is lit up
    private bool isTouched;     // When the block has been stepped on and changed texture
    private Color colour;       // Glow colour, what set the block belongs to

    public Color defaultColour;
    public Texture touchedTexture;  // What block changes to when touch
    public Texture defaultTexture;  // No texture applied after block deactivated

    GameObject managerObject;       // Where my manager at?    
    [SerializeField]
    private GameObject activateParticle;       // Particle System to spawn when activated
    [SerializeField]
    private GameObject activeParticle;
    private GameObject activeParticleInstance;

    float tileGlow;
    float currentPitch;




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
        tileGlow = 0.0f;
        GetComponent<Renderer>().material.SetFloat("_MKGlowPower", tileGlow);
        managerObject = GameObject.FindGameObjectWithTag("Manager");        
    }    
  
    
    // Activates a tile
    public void ActivateTile(Color _colour)
    {
        isActive = true;        
        colour = _colour;        
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
        print("ActivateTile");
        activeParticleInstance = (GameObject)GameObject.Instantiate(activeParticle, transform.position, Quaternion.identity);
        activeParticleInstance.GetComponentInChildren<ParticleSystem>().startColor = colour;
        GameObject activateParticleInstance = (GameObject)GameObject.Instantiate(activateParticle, transform.position, Quaternion.identity);
        activateParticleInstance.GetComponent<ParticleSystem>().startColor = colour;


    }


    // Deactivates a Tile
    public void DeactivateTile()
    {
        isActive = false;
        isTouched = false;
        colour = defaultColour;
        tileGlow = 0.0f;
        GetComponent<Renderer>().material.SetFloat("_MKGlowPower", tileGlow);
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
        GetComponent<Renderer>().material.SetTexture("_MKGlowTex", defaultTexture);
        GameObject.Destroy(activeParticleInstance);
        activeParticleInstance = null;
    }

    // Touches a tile
    void OnTriggerEnter(Collider _other)     // OnTouched
    {        
        if (isActive && !isTouched && _other.tag.Contains("Player")) {  // If this tile's active and it hasn't been touched yet and the player entered
            Debug.Log("Mgr: " + managerObject);            
            // Add Score
            managerObject.GetComponent<ScoreManager>().SteppedOn(colour);
                        
            isTouched = true;            
            GetComponent<Renderer>().material.SetTexture("_MKGlowTex", touchedTexture);
            gameObject.transform.parent.GetComponent<FloorScript>().CheckDoneAndDeactivate(colour);

            // Set Pitch
            GetComponent<AudioSource>().pitch = currentPitch;
            GetComponent<AudioSource>().Play();

            
        }

        //if (other.tag == "Player")
        //{
        //    Debug.Log("OnTouched -- isActive: " + isActive + "  isTouched: " + isTouched);
        //}
    }

    // Change my current pitch
    public void ChangePitch(float _pitch) {
        currentPitch = _pitch;
    }
	
	// Update is called once per frame
	void Update () {

        if (isActive)
        {
            tileGlow = Mathf.Cos(Time.timeSinceLevelLoad)/2.0f+1.25f;
            //print("Glow Factor: " + tileGlow);
            GetComponent<Renderer>().material.SetFloat("_MKGlowPower", tileGlow);
        }

	}
}
