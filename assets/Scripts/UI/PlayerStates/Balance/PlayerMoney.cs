using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField]
    private Text balance;

    public void UpdateBalance(int money)
    {
        if (balance != null)
        {
           balance.text = money.ToString();
        }
    }
}
