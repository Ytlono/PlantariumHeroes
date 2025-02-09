using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField]
    private Image manaBar;
    [SerializeField]
    private Text currentMannaText;

    public Image ManaBarImage
    {
        get { return manaBar; }
        set { manaBar = value; }
    }

    public Text CurrentMannaText
    {
        get { return currentMannaText; }
        set { currentMannaText = value; }
    }

    public void UpdateManaBar(float currentMana, float maxMana)
    {
        if (ManaBarImage != null)
        {
            CurrentMannaText.text = currentMana.ToString("F0");
            ManaBarImage.fillAmount = currentMana / maxMana;
        }
    }
}
