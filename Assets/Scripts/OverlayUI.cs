using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] private Text SanityText;
    [SerializeField] private GameObject Player;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void RefreshHealth()
    {
        int health = Player.GetComponent<PlayerInfo>().GetHealth();
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
}
