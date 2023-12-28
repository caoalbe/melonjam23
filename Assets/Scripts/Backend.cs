using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Backend : MonoBehaviour
{
    private float currTime = 0f;
    // [SerializeField] private GameObject playercharacter;

    void Start()
    {
        InitSingleton();
        SetSanity(100);
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

    // Sanity System
    private int sanity; // can takes values from [0,..100]
    [Header("Sanity Properties")]
    [SerializeField] public int sanityDecayAmount;
    [SerializeField] public float sanityDecayCooldown;
    public UnityEvent SanityUpdated;
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

    // Singleton Boilerplate
    public static Backend instance;

    private void InitSingleton()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }
}
