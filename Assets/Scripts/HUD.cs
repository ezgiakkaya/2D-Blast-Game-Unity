using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

    public Level level;
    public GameOver gameOver;

    public Text remaining_text;

    public Text remaining_box_subtext;
    public Text remaining_stone_subtext;
    public Text remaining_vase_subtext;
    public Image[] obstacles;



    private int obstacleIdx;


    private bool isGameOver = false;


    void Start()
    {
        UpdateObstacleImages();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void SetRemaining(int remaining)
    {
        remaining_text.text = remaining.ToString();
    }


    public void SetBoxTarget(int remaining)
    {
        remaining_box_subtext.text = remaining.ToString();

    }
    public void SetVaseTarget(int remaining)
    {
        remaining_vase_subtext.text = remaining.ToString();

    }
    public void SetStoneTarget(int remaining)
    {
        remaining_stone_subtext.text = remaining.ToString();

    }


    public void OnGameWin()
    {

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        Debug.Log("currentLevel : " + currentLevel);
        if (currentLevel < 10)
        {
            // Not the final level, increment and save
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        }
        gameOver.ShowWin();
        isGameOver = true;
    }

    public void OnGameLose()
    {
        gameOver.ShowLose();
        isGameOver = true;
    }




    // In HUD.cs
    public void UpdateObstacleImages()
    {

        int boxCount = level.grid.CountPiecesOfType(Grid.PieceType.BOX);
        obstacles[0].gameObject.SetActive(boxCount > 0);
        remaining_box_subtext.gameObject.SetActive(boxCount > 0);
        remaining_box_subtext.text = boxCount.ToString();

        int stoneCount = level.grid.CountPiecesOfType(Grid.PieceType.STONE);
        obstacles[1].gameObject.SetActive(stoneCount > 0);
        remaining_stone_subtext.gameObject.SetActive(stoneCount > 0);
        remaining_stone_subtext.text = stoneCount.ToString();

        int vaseCount = level.grid.CountPiecesOfType(Grid.PieceType.VASE);
        obstacles[2].gameObject.SetActive(vaseCount > 0);
        remaining_vase_subtext.gameObject.SetActive(vaseCount > 0);
        remaining_vase_subtext.text = vaseCount.ToString();
    }




}
