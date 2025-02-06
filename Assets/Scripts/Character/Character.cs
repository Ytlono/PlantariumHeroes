using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private float maxHealth;
        private float currentHealth;
        private State state = State.INGAME;

        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public float CurrentHealth
        {
            get { return currentHealth; }
            protected set
            {
                if (value <= 0f)
                {
                    currentHealth = 0f;
                }
                else if (value > MaxHealth)
                {
                    currentHealth = MaxHealth;
                }
                else
                {
                    currentHealth = value;
                }
            }
        }

        public State StateSetGet
        {
            get { return state; }
            set { state = value; }
        }

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public abstract void TakeDamage(float damage);

        public void CheckHealth()
        {
            UpdateState();
        }

        public float HPPercentage()
        {
            return CurrentHealth / MaxHealth;
        }

        public bool IsAlive()
        {
            return StateSetGet == State.INGAME;
        }

        public abstract void Heal(float healingAmount);

        public abstract void UpdateState();
    }
}
