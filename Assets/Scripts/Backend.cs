using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backend : MonoBehaviour
{
    private float currTime = 0f;
    // [SerializeField] private GameObject playercharacter;

    void Start()
    {
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
    [SerializeField] public int sanityDecayAmount;
    [SerializeField] public float sanityDecayCooldown;
    private float lastSanityDecayTime = 0f;

    public void SetSanity(int amount)
    {
        this.sanity = amount;
    }

    private void DecaySanity()
    {
        this.sanity -= sanityDecayAmount;
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
