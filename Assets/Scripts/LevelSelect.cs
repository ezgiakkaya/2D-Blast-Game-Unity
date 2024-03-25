using UnityEngine;
using UnityEngine.UI; // For working with UI text
using UnityEngine.SceneManagement; // For loading scenes
//using TMPro; // Uncomment this if using TextMeshPro

public class LevelSelect : MonoBehaviour
{
    public Text levelText;

    void Start()
    {
        UpdateLevelButton();
    }

    void UpdateLevelButton()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        Debug.Log("currentLevel UpdateLevelButton : " + currentLevel);
        // Check if the player has completed all levels
        if (currentLevel > 10)
        {
            levelText.text = "Finished";
        }
        else
        {
            levelText.text = "Level " + currentLevel;
        }
    }

    public void OnButtonPress()
    {

        //Debug.Log("Select button is pressed.");
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        // Debug.Log("PlayerPrefs.GetInt(, 1); : " + currentLevel);
        // Prevent loading a new level if all levels are completed
        if (currentLevel <= 10)
        {
            string sceneName = "Level" + currentLevel;
            //Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene("Level1");

        }
        else
        {
            //Debug.Log("All levels completed.");
            // Optionally, do something else here, like showing a completion screen
        }
    }
}


