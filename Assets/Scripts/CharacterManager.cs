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
    private Transform startPosition;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioClip shutdownSound;
    [SerializeField]
    private Text livesText;

    [SerializeField]
    private GameObject UICanvas;



  // Use this for initialization
  void Start() {

    currentLives = STARTING_LIVES;              // Initialize number of lives
    stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();
    animator.SetBool("hasDied", false);
    startPosition = gameObject.transform;
    print(startPosition.position);

  }



  // Something hit the player, so let's process that
  void OnTriggerEnter(Collider _other) {
    if (_other.tag == "Enemy" && !animator.GetBool("hasDied"))      // Oh no! An enemy hit me!
    {
      LoseLife();
      // Check if permanently dead
      if (OutOfLives()) {
        stateManager.SetState(StateManager.GameState.GAMEOVER_STATE);   // If I am, Game Over man!
        Respawn();
      } else {
        // If not, Respawn in the center, send the enemies back into their spawners                
        animator.SetBool("hasDied", true);
        _other.gameObject.GetComponentInChildren<Animator>().SetBool("killedPlayer", true);
        StartCoroutine(OnDeath());
      }
    }
  }

  IEnumerator OnDeath() {
    yield return new WaitForSeconds(3.0f);

    Respawn();
    stateManager.SetState(StateManager.GameState.RESET_STATE);
  }

  public void ResetLives() {
    currentLives = STARTING_LIVES;
    livesText.text = currentLives.ToString();
  }

  void LoseLife() {
    if (currentLives > 0) {
      currentLives--;
      livesText.text = currentLives.ToString();
      AudioSource.PlayClipAtPoint(shutdownSound, transform.position);
    }

  }

  // Respawn the player in the center of the level
  void Respawn() {
        print(startPosition.position);
    gameObject.transform.position = startPosition.position;
    animator.SetBool("hasDied", false);
  }

  // Tells me whether I'm out of lives
  bool OutOfLives() {
    return (currentLives <= 0);
  }
  
  // Update is called once per frame
  void Update() {

  }
}
