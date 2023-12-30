using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject menu;

    // Update is called once per frame
    void Update()
    {
        // TODO: Access dialogue component in a safer way
        if (Input.GetKeyDown(KeyCode.Escape) && !GameObject.Find("Canvas").GetComponent<DialogueUI>().IsOpen)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        menu.SetActive(isPaused);
        Time.timeScale = 1 - Time.timeScale;
    }

    public void LoadCheckpoint()
    {
        Backend.instance.ReloadLevel();
    }

    public void RestartGame()
    {
        Backend.instance.RestartGame();
    }
}
