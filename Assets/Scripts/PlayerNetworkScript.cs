using UnityEngine;
using System.Collections;

public class PlayerNetworkScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        tag = (gameObject.transform.position.x < 0) ? "PlayerA" : "PlayerB";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
