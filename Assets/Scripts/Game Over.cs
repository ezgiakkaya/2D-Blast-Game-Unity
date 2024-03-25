using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ScreenParent;
    public GameObject WinScreenParent;
    public Text loseText;
    public void Start()
    {

        //At the beginning of the game, we don't want to see game over pop-up
        ScreenParent.SetActive(false);
        WinScreenParent.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {

    }

    //will be called when we lose the game
    public void ShowLose()
    {
        ScreenParent.SetActive(true);
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOverShow");
        }

    }

    public void ShowWin()
    {
        WinScreenParent.SetActive(true);
        loseText.enabled = false;
        Animator animator = GetComponent<Animator>();

        //win animationu oluştur, winte winshow oynasın
        if (animator)
        {
            animator.Play("CelebrationShow");
        }


        // If it's the final level, no incrementing needed
        // PlayerPrefs.Save();
        StartCoroutine(WaitAndLoadMainScene(5));
    }

    public void OnReplayClicked()
    {


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnCloseClicked()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    // SceneManager.LoadScene("Main Scene");
    //sonrasında main scene e dönmek için coroutine oluştyur


    IEnumerator WaitAndLoadMainScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("LevelSelect");
    }

}
