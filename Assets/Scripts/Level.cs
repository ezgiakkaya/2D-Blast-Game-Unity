using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Level : MonoBehaviour
{
    public enum LevelType
    {

        OBSTACLE,

    };
    protected LevelType type;

    public LevelType Type
    {
        get { return type; }
    }

    //each level needs a reference to the grid object so we can assign it in the inspector 
    // we need a variable to determine how good the played played 
    public Grid grid;
    public int score1Star;
    public int score2Star;
    public int score3Star;

    public HUD hud;

    protected int currentScore;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("new level started");

    }

    // Update is called once per frame
    void Update()
    {

    }

    //FUNCTIONS THAT EVERY LEVEL NEED
    // declared as virtual so that levels can override it
    public virtual void GameWin()
    {
        Debug.Log("You win.");
        grid.GameOver();
        hud.OnGameWin();
    }
    public virtual void GameLose()
    {
        Debug.Log("You lose.");
        grid.GameOver();
        hud.OnGameLose();
    }

    public virtual void OnMove()
    {

    }
    //takes the piece cleared as a parameter
    public virtual void OnPieceCleared(GamePiece piece)
    {
        //update score
        currentScore += piece.score;
        Debug.Log("current score is:" + currentScore);

    }
}
