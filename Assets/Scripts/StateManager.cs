using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {


    public enum GameState { PLAYING_STATE, PAUSED_STATE, GAMEOVER_STATE, VICTORY_STATE, RESET_STATE, NUMBER_OF_STATES };

    private GameState currentState;

    SpawnerManager spawnerManager;


	// Use this for initialization
	void Start () {
        
        currentState = GameState.PLAYING_STATE;
        print("Now Playing...");
        spawnerManager = gameObject.GetComponent<SpawnerManager>();
	}

    public void SetState(GameState _newState)
    {
        currentState = _newState;

        switch (currentState)
        {
            case GameState.PLAYING_STATE:
                print("Now Playing...");
                break;
            case GameState.PAUSED_STATE:
                print("Now Paused...");
                break;
            case GameState.GAMEOVER_STATE:
                print("Game Over man!");
                break;
            case GameState.VICTORY_STATE:
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
		
}
