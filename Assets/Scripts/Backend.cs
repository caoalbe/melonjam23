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

    // Sanity System
    private int sanity; // can takes values from [0,..100]
    private int health;
    [Header("Sanity Properties")]
    [SerializeField] public int sanityDecayAmount;
    [SerializeField] public float sanityDecayCooldown;
    public UnityEvent SanityUpdated;
    public UnityEvent HealthUpdated;
    private float lastSanityDecayTime = 0f;


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

    // Scene Manager
    private void ReloadLevel()
    {
        ResetState();
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
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
