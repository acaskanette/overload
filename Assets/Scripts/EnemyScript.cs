using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private GameObject deathEffect;         // Particle system on death

    public float Speed = 1.0f;         // How fast the enemy moves to chase the player
    Color colour;               // Type of enemy, what colour it glows
            
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
        GameObject.FindWithTag("Manager").GetComponent<ScoreManager>().EnemyKilled();
        GameObject.Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
        Destroy();
    }

    public void Destroy()
    {               
        GameObject.Destroy(this.gameObject);
    }


}
