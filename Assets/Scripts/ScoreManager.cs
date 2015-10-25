﻿using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    private Color lastColourSteppedOn;      // Last Colour of tile the player stepped on
    private int steppedOnInARow;            // How many tiles of the same colour I have stepped on in a row

    private Color lastColourEnemyKilled;    // Last colour of the enemy you just killed
    private int killedInARow;               // How many enemies you have killed in a row

    private float currentScore;               // Current score of the game
    private float previousScore;              // Score last frame    

    // Consts for tile hitting
    private const int BASE_SCORE_PER_TILE = 5;
    private const int SCORE_MULTIPLIER_PER_TILE_HIT = 10;
    // Consts for enemy killing
    private const int BASE_SCORE_PER_ENEMY = 2;
    private const float TIME_TO_REDUCE_KILLSTREAK = 3.0f;
    private float timeToReduceKillStreak = 0.0f;


	// Use this for initialization
	void Start () {
        currentScore = 0;
        previousScore = 0;
        steppedOnInARow = 0;
        lastColourSteppedOn = Color.black;	
	}


    // Called by the Tile whenever it is stepped on and activated
    public void SteppedOn(Color _colour) {

        //print(lastColourSteppedOn);
        if (lastColourSteppedOn == _colour)     // If I stepped on the same colour as last time
            steppedOnInARow++;                  // increment how many I have stepped on in a row
        else
            steppedOnInARow = 1;                // otherwise, stepped on a new colour, so set to 1

        currentScore += (BASE_SCORE_PER_TILE + SCORE_MULTIPLIER_PER_TILE_HIT * steppedOnInARow);        
                                                // Add to score based on whatever formula you like
        lastColourSteppedOn = _colour;          // Update what colour was last hit

        //print("Added: " + (BASE_SCORE_PER_TILE + SCORE_MULTIPLIER_PER_TILE_HIT * steppedOnInARow) + "  Total Score:" + currentScore);

    }

    // Called by an enemy as it dies
    public void EnemyKilled()
    {
        killedInARow++;                  // increment how many I have stepped on in a row
        print(killedInARow);
        currentScore += Mathf.Pow(BASE_SCORE_PER_ENEMY,killedInARow);

      // print("Added: " + Mathf.Pow(BASE_SCORE_PER_ENEMY, killedInARow) + "  Total Score:" + currentScore);

    }
	
	// Update is called once per frame
	void Update () {

        timeToReduceKillStreak += Time.deltaTime;
        //print(timeToReduceKillStreak);
        if (timeToReduceKillStreak >= TIME_TO_REDUCE_KILLSTREAK)
        {
            killedInARow=0;
            print(killedInARow);
            timeToReduceKillStreak = 0.0f;
        }
        
        
	}
}
