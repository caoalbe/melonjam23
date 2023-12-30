using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// All this script does is just highlight the text on a button when its hovered
public class ButtonHighlight : MonoBehaviour
{
    private Color highlightedColor;
    private Color defaultColour;
    private Text label;

    void Start()
    {
        defaultColour = gameObject.GetComponent<Button>().colors.normalColor;
        highlightedColor = gameObject.GetComponent<Button>().colors.highlightedColor;
        label = gameObject.GetComponentInChildren<Text>();
    }

    void OnDisable()
    {
        Reset();
    }

    public void Highlight()
    {
        label.color = highlightedColor;
    }

    public void Reset()
    {
        label.color = defaultColour;
    }
}
