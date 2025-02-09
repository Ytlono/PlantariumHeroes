using UnityEngine;
using UnityEngine.EventSystems;

namespace MyGameProject
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerRun playerRun;
        private PlayerDie playerDie;
        private PlayerAttack playerAttack;
        private PlayerUltimate playerUltimate;
        private Player player;
        private bool isAttacking;
        private bool isHoldingAttack;

        void Start()
        {
            playerRun = GetComponent<PlayerRun>();
            playerDie = GetComponent<PlayerDie>();
            playerAttack = GetComponent<PlayerAttack>();
            player = GetComponent<Player>();
            playerUltimate = GetComponent<PlayerUltimate>();
        }

        void Update()
        {
           
            if (!isAttacking && player.IsAlive())
            {
                HandleRunInput();
            }
            
            HandleDeathAndRecovery();
            AttackCombinations();
            Ultimate();
           
        }

        private void HandleRunInput()
        {
            if (!isAttacking && player.IsAlive())
            {
                playerRun.Run();
            }
            else
            {
                playerRun.SetSpeedToBlendTree(0f);
            }
        }

        private void HandleDeathAndRecovery()
        {
            if (!player.IsAlive())
            {
                playerRun.StopMovement();
                playerDie.ActivateDieAnimation();
                isAttacking = false;
            }

            if (!player.IsAlive() && Input.GetKeyDown(KeyCode.C))
            {
                player.Heal(player.MaxHealth);
                playerDie.ActivateDieRecoveryAnimation();
            }
        }

        private void AttackCombinations()
        {
            if (Input.GetMouseButtonDown(0) && player.IsAlive() && !IsPointerOverUI())
            {
                isAttacking = true;
                isHoldingAttack = true;
                playerRun.StopMovement();
                playerAttack.Attack1(isAttacking);
            }

            if (Input.GetMouseButton(0) && isHoldingAttack && player.IsAlive())
            {
                playerAttack.Attack1(true);
            }

            if (playerAttack.CheckEndOfAnimation() && !Input.GetMouseButton(0))
            {
                isAttacking = false;
                isHoldingAttack = false;
                playerAttack.Attack1(false);
            }
        }

        private void Ultimate()
        {
            if (player.IsManaFull() && Input.GetKey(KeyCode.Q))
            {
                player.IncreaseManna(-player.CurrentManna);
                playerUltimate.ActivateUltimate(player,playerRun);
            }
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}
