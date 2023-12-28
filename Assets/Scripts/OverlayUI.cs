using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] private Text SanityText;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void RefreshHealth()
    {
        int health = Backend.instance.GetHealth();
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void RefreshSanity()
    {
        SanityText.text = "Sanity: " + Backend.instance.GetSanity();
    }
}