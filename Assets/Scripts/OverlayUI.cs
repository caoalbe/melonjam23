using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] private Text HealthText;
    [SerializeField] private Text SanityText;

    public void SetHealth(int value)
    {
        HealthText.text = "Health: " + value;
    }

    public void RefreshSanity()
    {
        SanityText.text = "Sanity: " + Backend.instance.GetSanity();
    }
}
