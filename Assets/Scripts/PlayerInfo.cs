using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    private float currTime = 0f;
    private bool isPlayerInvincible = false;
    private bool isPlayerGhost = false;
    [SerializeField] private GameObject lamp;

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(maxHealth);
        SetSanity(maxSanity);
    }

    // Update is called once per frame
    void Update()
    {
        // Alternate Ghost form
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetGhost(!isPlayerGhost);
        }

        // Decay the Sanity
        if (isPlayerGhost && lastSanityDecayTime + sanityDecayCooldown <= currTime)
        {
            DecaySanity();
        }

        // Make player vincible
        if (lastHitTime + invincibilityDuration <= currTime)
        {
            isPlayerInvincible = false;
        }

        currTime += Time.deltaTime;
    }

    // Health System
    private int health;
    [Header("Health Properties")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] public UnityEvent HealthUpdated;
    private float lastHitTime = 0f;

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int amount)
    {
        this.health = amount;
    }

    public void TakeDamage(int damage)
    {
        if (isPlayerInvincible) { return; }

        health -= damage;
        isPlayerInvincible = true;
        HealthUpdated.Invoke();
        lastHitTime = currTime;
        if (health == 0) { Backend.instance.ReloadLevel(); }
    }

    // Sanity System
    private int sanity; // takes values from [0,..100]
    [Header("Sanity Properties")]
    [SerializeField] private int maxSanity;
    [SerializeField] public int sanityDecayAmount;
    [SerializeField] public float sanityDecayCooldown;
    public UnityEvent SanityUpdated;
    private float lastSanityDecayTime = 0f;

    public int GetSanity()
    {
        return this.sanity;
    }

    public void SetSanity(int amount)
    {
        this.sanity = amount;
        SanityUpdated.Invoke();
    }

    private void DecaySanity()
    {
        this.sanity -= sanityDecayAmount;
        SanityUpdated.Invoke();
        this.lastSanityDecayTime = currTime;
    }

    // Ghost System
    private void SetGhost(bool newForm)
    {
        isPlayerGhost = newForm;
        lastSanityDecayTime = currTime;
        int ghostLayer = LayerMask.NameToLayer("Ghost");
        int normalLayer = LayerMask.NameToLayer("Player");
        // TODO: implement this with changing sprites
        if (newForm)
        {
            lamp.SetActive(false);
            gameObject.layer = ghostLayer;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            lamp.SetActive(true);
            gameObject.layer = normalLayer;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
