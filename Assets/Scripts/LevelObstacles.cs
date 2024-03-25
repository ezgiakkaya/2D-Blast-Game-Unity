using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacles : Level
{

    public int numMoves;
    public bool game_finished = false;
    //public Grid.PieceType[] obstacleTypes;

    private int movesUsed = 0;
    private int numObstaclesLeft;
    private int numBoxesLeft;
    private int numStonesLeft;
    private int numVasesLeft;


    // Use this for initialization
    void Start()
    {
        type = LevelType.OBSTACLE;
        game_finished = false;
        // Initialize counts based on the grid's current state
        numBoxesLeft = grid.CountPiecesOfType(Grid.PieceType.BOX);
        numStonesLeft = grid.CountPiecesOfType(Grid.PieceType.STONE);
        numVasesLeft = grid.CountPiecesOfType(Grid.PieceType.VASE);

        // Update the HUD for each obstacle type
        hud.SetBoxTarget(numBoxesLeft);
        hud.SetStoneTarget(numStonesLeft);
        hud.SetVaseTarget(numVasesLeft);
        numMoves = grid.current_level.move_count;
        hud.SetRemaining(numMoves);
    }


    // Update is called once per frame
    public void Update()
    {
        game_finished = false;

    }



    public override void OnMove()
    {
        movesUsed++;
        hud.SetRemaining(numMoves - movesUsed);
        if (numMoves - movesUsed == 0 && (numBoxesLeft > 0 || numStonesLeft > 0 || numVasesLeft > 0))
        {
            GameLose();
        }
    }

    public override void OnPieceCleared(GamePiece piece)
    {
        base.OnPieceCleared(piece);

        switch (piece.Type)
        {
            case Grid.PieceType.BOX:
                numBoxesLeft--;
                hud.SetBoxTarget(numBoxesLeft);
                break;
            case Grid.PieceType.STONE:
                numStonesLeft--;
                hud.SetStoneTarget(numStonesLeft);
                break;
            case Grid.PieceType.VASE:
                numVasesLeft--;
                hud.SetVaseTarget(numVasesLeft);
                break;
        }

        if (numBoxesLeft == 0 && numStonesLeft == 0 && numVasesLeft == 0 && game_finished != true)
        {
            game_finished = true;
            currentScore += 1000 * (numMoves - movesUsed);
            GameWin();
        }
    }
}
