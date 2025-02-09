using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public class HealItem : MonoBehaviour
    {
        private float healAmount = 100f;
        private Player player;

        public void Start()
        {
            Player = FindAnyObjectByType<Player>(); 
        }
        public float HealAmount
        {
            get => healAmount;
            set => healAmount = value;
        }

        public Player Player
        {
            get => player;
            set => player = value;
        }

        public void Destroy()
        {
            Destroy(transform.gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Player.Heal(HealAmount);
                Destroy();
            }
        }
    }
}
