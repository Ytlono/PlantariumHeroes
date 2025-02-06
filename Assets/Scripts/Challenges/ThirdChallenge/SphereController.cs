using MyGameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private ChallengeThird challenge;

    public ChallengeThird Challenge
    {
        get => challenge;
        set => challenge = value;
    }

    private void Start()
    {
        Challenge = FindObjectOfType<ChallengeThird>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                Challenge.RemoveSphere(this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
