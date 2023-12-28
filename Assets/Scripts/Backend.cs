using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Backend : MonoBehaviour
{
    private float currTime = 0f;
    // [SerializeField] private GameObject playercharacter;

    void Awake()
    {
        InitSingleton();
        ResetState();
    }

    void Update()
    {
        currTime += Time.deltaTime;

        // Decay the Sanity
        if (lastSanityDecayTime + sanityDecayCooldown <= currTime && true)
        {
            // TODO: check if light is off instead of "&& true"
            DecaySanity();
        }
    }

    private void ResetState()
    {
        SetSanity(100);
        SetHealth(3);
    }

    // Health System
    private int health;
    // [Header("Health Properties")]
    [HideInInspector] public UnityEvent HealthUpdated;

    public void SetHealth(int amount)
    {
        this.health = amount;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        HealthUpdated.Invoke();
        if (health == 0) { ReloadLevel(); }
    }

    public int GetHealth()
    {
        return health;
    }

    // Sanity System
    private int sanity; // takes values from [0,..100]
    [Header("Sanity Properties")]
    [SerializeField] public int sanityDecayAmount;
    [SerializeField] public float sanityDecayCooldown;
    [HideInInspector] public UnityEvent SanityUpdated;
    private float lastSanityDecayTime = 0f;

    public void SetSanity(int amount)
    {
        this.sanity = amount;
        SanityUpdated.Invoke();
    }

    public int GetSanity()
    {
        return this.sanity;
    }

    private void DecaySanity()
    {
        this.sanity -= sanityDecayAmount;
        SanityUpdated.Invoke();
        this.lastSanityDecayTime = currTime;
    }

    // Scene and Checkpoint Manager
    private Vector3 currCheckpoint;

    private void ReloadLevel()
    {
        ResetState();
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
