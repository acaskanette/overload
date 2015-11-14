using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private GameObject deathEffect;         // Particle system on death

    public float Speed = 0.5f;         // How fast the enemy moves to chase the player
    Color colour;               // Type of enemy, what colour it glows
            
    private GameObject player;  // Reference to the player in the scene
    private Rigidbody rigidBody;

        
    // Use this for initialization
	void Start () {

        player = (GameObject)(GameObject.FindWithTag("Player"));
        rigidBody = GetComponent<Rigidbody>();
      	
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


    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Tile")
        {
            print("Enemy Hit a tile");
            rigidBody.AddForce(Vector3.up * 0.10f, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Find the player and then Chase the player
    /// </summary>
    private void Chase()
    {
 
       transform.LookAt(player.transform);
       rigidBody.AddForce(transform.forward * Speed, ForceMode.Acceleration);
        if (transform.position.y > 4.0f)
            transform.position -= Vector3.down * Time.deltaTime*2;
        
    }


    public void Kill()
    {
        // play death animations/sounds
        GameObject.FindWithTag("Manager").GetComponent<ScoreManager>().EnemyKilled();
        GameObject deathEffectInstance = (GameObject)GameObject.Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
        deathEffectInstance.GetComponent<EnemyExplosionScript>().SetColour(colour);
        Destroy();
    }

    public void Destroy()
    {               
        GameObject.Destroy(this.gameObject);
    }


}
