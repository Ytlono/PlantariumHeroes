using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueController : Character
{
    private StatueHP statueHealthBar;
    private void Awake()
    {
        statueHealthBar = GetComponent<StatueHP>();
        MaxHealth = 1000f;
        CurrentHealth = MaxHealth;
    }
    private void Update()
    {
        statueHealthBar.UpdateHealthBar(CurrentHealth, MaxHealth);
    }
    public override void Heal(float healingAmount)
    {
        CurrentHealth += healingAmount;
    }

    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public override void UpdateState()
    {
        if (CurrentHealth == 0)
        {
            StateSetGet = State.DEAD;
        }
        else if (CurrentHealth > 0)
        {
            StateSetGet = State.INGAME;
        }
    }
}
