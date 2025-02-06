using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameProject {
    public class HealRock : MonoBehaviour
    {
        [SerializeField]
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {

                Player player = other.gameObject.GetComponent<Player>();
                player.Heal(1500);
            }
        }
    }
}