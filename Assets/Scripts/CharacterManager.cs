using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {


    [SerializeField]
    private const int MAX_NUMBER_OF_LIVES = 5;      // Maximum capacity for lives
    [SerializeField]
    private const int STARTING_LIVES = 3;           // How many lives you start the game with

    private int currentLives;                       // How many lives you have right now

    private StateManager stateManager;


	// Use this for initialization
	void Start () {
        currentLives = STARTING_LIVES;              // Initialize number of lives
        stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();
	}



    // Something hit the player, so let's process that
    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Enemy")      // Oh no! An enemy hit me!
        {
            currentLives--;
            // Check if permanently dead
            if (OutOfLives())
            {
                stateManager.SetState(StateManager.GameState.GAMEOVER_STATE);   // If I am, Game Over man!
            } 
            else {
                // If not, Respawn in the center, send the enemies back into their spawners
                Respawn();
                stateManager.SetState(StateManager.GameState.RESET_STATE);   // If I am, Game Over man!

            }
        }
    }

    // Respawn the player in the center of the level
    void Respawn()
    {
        gameObject.transform.position = new Vector3(0.0f, 1.5f, 0.0f);
    }

    // Tells me whether I'm out of lives
    bool OutOfLives()
    {
        return (currentLives <= 0);
    }

    


	// Update is called once per frame
	void Update () {
	
	}
}
