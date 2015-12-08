using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DeactivateTileScript : NetworkBehaviour
{   

	// Use this for initialization
	void Start () {
        
        Destroy(gameObject, 1.7f);
	}   
		
}
