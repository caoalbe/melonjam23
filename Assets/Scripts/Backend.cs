using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Backend : MonoBehaviour
{
    void Awake()
    {
        InitSingleton();
    }

    // Scene and Checkpoint Manager
    private Vector3 currCheckpoint;

    public void ReloadLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void SetCheckPoint(Vector3 newSpawn)
    {
        currCheckpoint = newSpawn;
    }

    public Vector3 GetCheckPoint()
    {
        if (currCheckpoint == null)
        {
            return GameObject.Find("Checkpoints").transform.GetChild(0).position;
        }
        return currCheckpoint;
    }

    // Singleton Boilerplate
    public static Backend instance;

    private void InitSingleton()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }
}
