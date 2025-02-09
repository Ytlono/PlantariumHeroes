using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueHP : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    public void UpdateHealthBar(float CurrentHealth, float MaxHealth)
    {
        healthBar.fillAmount = CurrentHealth / MaxHealth;
    }
}
