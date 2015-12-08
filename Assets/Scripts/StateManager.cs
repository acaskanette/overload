using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StateManager : NetworkBehaviour
{
  public enum GameState { START_SCREEN, PLAYING_STATE, PAUSED_STATE, GAMEOVER_STATE, VICTORY_STATE, RESET_STATE, NUMBER_OF_STATES };

  public GameState currentState;

  SpawnerManager spawnerManager;
    CharacterManager characterManagerA;
    CharacterManager characterManagerB;
    ScoreManager scoreManager;
  FloorScript floorManager;

    GameObject playerA;
    GameObject playerB;

    [SerializeField]
  Canvas hud;
  [SerializeField]
  Canvas title;

  [SerializeField]
  Text gameOverText;

  [SerializeField]
  GameObject mainCamera;


  // Use this for initialization
  void Start() {
    currentState = GameState.START_SCREEN;
    print("Now Playing...");
    spawnerManager = gameObject.GetComponent<SpawnerManager>();
    scoreManager = gameObject.GetComponent<ScoreManager>();
    floorManager = gameObject.GetComponent<FloorScript>();
    playerA = GameObject.FindGameObjectWithTag("PlayerA");
    characterManagerA = playerA.GetComponent<CharacterManager>();

    playerB = GameObject.FindGameObjectWithTag("PlayerB");
    characterManagerB = playerB.GetComponent<CharacterManager>();
    hud.enabled = false;
  }

  public void SetState(GameState _newState) {
    currentState = _newState;

    // Will eventually move into EnterState method:
    switch (currentState) {
      case GameState.START_SCREEN:
        hud.enabled = false;
        title.enabled = true;
        mainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        print("Start Screen");
        break;
      case GameState.PLAYING_STATE:
        title.enabled = false;
        hud.enabled = true;
        mainCamera.transform.rotation = Quaternion.Euler(60.0f, 0.0f, 0.0f);
        print("Now Playing...");
        break;
      case GameState.PAUSED_STATE:
        print("Now Paused...");
        break;
      case GameState.GAMEOVER_STATE:
        gameOverText.enabled = true;
        gameOverText.color = Color.red;
        print("Game Over man!");
        break;
      case GameState.VICTORY_STATE:
        playerA.GetComponentInChildren<Animator>().SetBool("victory", true);
        playerB.GetComponentInChildren<Animator>().SetBool("victory", true);
        gameOverText.enabled = true;
        gameOverText.color = Color.green;
        print("Victory!");
        break;
      case GameState.RESET_STATE:
        print("Resetting Level...");
        spawnerManager.ResetSpawners();
        currentState = GameState.PLAYING_STATE;
        break;
      default:
        print("That state is bullshit man!");
        break;
    }
  }

  void Update() {
    if (currentState == GameState.START_SCREEN && Input.GetButtonDown("Enter")) {
      SetState(GameState.PLAYING_STATE);
    }

    if (currentState == GameState.GAMEOVER_STATE && Input.GetButtonDown("Enter")) {
      spawnerManager.ResetSpawners();
      scoreManager.ResetScore();
      floorManager.ResetLevel();
      characterManagerA.ResetLives();
      characterManagerB.ResetLives();
      gameOverText.enabled = false;
      SetState(GameState.START_SCREEN);
    }

  }
}
