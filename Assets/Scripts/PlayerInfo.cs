using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{
    public static bool IsTutorialDone = false;
    public static bool CanTurnOnLight = false;
    // Start is called before the first frame update
    void Start()
    {
        SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLightTurningOff)
        {
            t += Time.deltaTime / lampTime;
            Vector3 newScale = Vector3.Lerp(lamp.transform.localScale, Vector3.zero, t);
            lamp.transform.localScale = newScale;

            if (lamp.transform.localScale.magnitude < 0.5)
            {
                isLightTurningOff = false;
                SetGhost(!isPlayerGhost);
            }
        }

        // Alternate Ghost form
        if (CanTurnOnLight && Input.GetKeyDown(KeyCode.E))
        {
            // Gradually reduce the lamp size
            isLightTurningOff = true;

            if (IsTutorialDone)
            {
                CanTurnOnLight = true;
            }

            else
            {
                CanTurnOnLight = false;
            }
            
        }

        // Make player vincible
        if (lastHitTime + invincibilityDuration <= Time.time)
        {
            isPlayerInvincible = false;
        }
    }

    // Lamp System
    private float t = 0;
    private bool isLightTurningOff = false;
    [SerializeField] private GameObject lamp;
    [SerializeField] private int lampScale;
    [SerializeField] private float lampTime;

    // Health System
    private int health;
    [Header("Health Properties")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] public UnityEvent HealthUpdated;
    [SerializeField] private GameObject transitionScreen;
    private float lastHitTime = 0f;
    private bool isPlayerInvincible = false;

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
        lastHitTime = Time.time;
        if (health == 0) {
            Backend.instance.ReloadLevel();
        }
    }

    // Ghost System
    private bool isPlayerGhost = false;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite ghostSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void SetGhost(bool newForm)
    {
        isPlayerGhost = newForm;
        int ghostLayer = LayerMask.NameToLayer("Ghost");
        int normalLayer = LayerMask.NameToLayer("Player");
        if (newForm)
        {
            lamp.SetActive(false);
            spriteRenderer.sprite = ghostSprite;
            gameObject.layer = ghostLayer;
        }
        else
        {
            lamp.SetActive(true);
            lamp.transform.localScale = Vector3.one * lampScale;
            spriteRenderer.sprite = normalSprite;
            gameObject.layer = normalLayer;
        }
    }

    public bool GetIsGhost()
    {
        return this.isPlayerGhost;
    }
}
