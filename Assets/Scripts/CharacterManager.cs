using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {


    [SerializeField]
    private const int MAX_NUMBER_OF_LIVES = 5;      // Maximum capacity for lives
    [SerializeField]
    private const int STARTING_LIVES = 3;           // How many lives you start the game with

    private int currentLives;                       // How many lives you have right now

    private StateManager stateManager;

    [SerializeField]
    private Animator animator;

    bool hasDied;
    

    [SerializeField]
    private AudioClip shutdownSound;
    [SerializeField]
    private GameObject livesIcon;

    [SerializeField]
    private GameObject UICanvas;

    private GameObject[] livesIconArray;



	// Use this for initialization
	void Start () {
        currentLives = STARTING_LIVES;              // Initialize number of lives
        stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();
        hasDied = false;
        
        // Set up UI for lives
        livesIconArray = new GameObject[MAX_NUMBER_OF_LIVES];
        for (int i = 0; i < MAX_NUMBER_OF_LIVES; i++)
        {
            if (i < STARTING_LIVES)
            {
                livesIconArray[i] = (GameObject)GameObject.Instantiate(livesIcon, Vector3.zero, Quaternion.identity);
                livesIconArray[i].transform.parent = UICanvas.transform;
                livesIconArray[i].GetComponent<RectTransform>().localPosition = new Vector3(-555.0f + i * 32.0f, -140.0f);
            }
            else
                livesIconArray[i] = null;
        }
            
	}



    // Something hit the player, so let's process that
    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Enemy" && !hasDied)      // Oh no! An enemy hit me!
        {
            LoseLife();            
            // Check if permanently dead
            if (OutOfLives())
            {
                stateManager.SetState(StateManager.GameState.GAMEOVER_STATE);   // If I am, Game Over man!
            } 
            else {
                // If not, Respawn in the center, send the enemies back into their spawners
                hasDied = true;
                animator.SetBool("hasDied", hasDied);
                _other.gameObject.GetComponentInChildren<Animator>().SetBool("killedPlayer", true);
                StartCoroutine(OnDeath());
                
            }
        }
    }

    IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(3.0f);
       
        Respawn();
        stateManager.SetState(StateManager.GameState.RESET_STATE);   // If I am, Game Over man!
        
    }



    void LoseLife() {

        if (currentLives > 0 && livesIconArray[currentLives - 1] != null)
        {
            GameObject.Destroy(livesIconArray[currentLives-1]);
            livesIconArray[currentLives - 1] = null;
            currentLives--;
            AudioSource.PlayClipAtPoint(shutdownSound, transform.position);
        }            
    
    }

    // Respawn the player in the center of the level
    void Respawn()
    {
        gameObject.transform.position = new Vector3(0.0f, 1.5f, 0.0f);
        hasDied = false;
        animator.SetBool("hasDied", hasDied);
    }

    // Tells me whether I'm out of lives
    bool OutOfLives()
    {
        return (currentLives <= 0);
    }

    


	// Update is called once per frame
	void Update () {
	    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            hasDied = false;
            animator.SetBool("hasDied", hasDied);
        }
	}
}
