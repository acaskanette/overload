using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {


    public float Speed = 10;         // How fast the enemy moves to chase the player
    Color colour;               // Type of enemy, what colour it glows
    
    private Color[] colours = { Color.red, Color.yellow, Color.cyan, Color.magenta };   // Colours enemy can be
    
    private GameObject player;  // Reference to the player in the scene
        
    // Use this for initialization
	void Start () {

        player = (GameObject)(GameObject.FindWithTag("Player"));
      	
	}

    public void Initialize(Color _colour)
    {

        colour = _colour;
        GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);

    }

	
	// Update is called once per frame
	void Update () {

        Chase();

	}


    /// <summary>
    /// Find the player and then Chase the player
    /// </summary>
    private void Chase()
    {
 
        transform.LookAt(player.transform);
 
        transform.position += transform.forward*Speed*Time.deltaTime;
        
    }


    public void Kill()
    {
        // play death animations/sounds
        // wait til they're done
        GameObject.Destroy(this.gameObject);
    }


}
