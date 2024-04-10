using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClickScript : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CycleSetting()
    {
        // WIP
    }

    public void StartLevelAfterCountdown()
    {
        FindObjectOfType<StageManager>().HandleLevelStart();
        gameObject.SetActive(false);
    }

    public void TriggerMap()
    {
        GameObject.Find("Map/LevelSelect").gameObject.GetComponent<Animator>().SetTrigger("map");
    }
}
