using UnityEngine;

namespace MyGameProject
{
    public class PlayerRun : MonoBehaviour
    {
        private Animator animator;
        private CharacterController characterController;

        [SerializeField] private float speed = 4f;
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private Transform cameraTransform;

        [SerializeField] private float gravity = 9.8f;
        private float verticalVelocity;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        public void Run()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

            if (inputDirection != Vector3.zero)
            {
                MoveCharacter(inputDirection);
            }
            else
            {
                StopMovement();
            }
        }

        private void MoveCharacter(Vector3 inputDirection)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * inputDirection.z + right * inputDirection.x).normalized;

            RotateCharacter(moveDirection);

            float movementSpeed = inputDirection.magnitude * speed;

            UpdateAnimation(movementSpeed);

            ApplyMovement(moveDirection, movementSpeed);
        }

        private void RotateCharacter(Vector3 moveDirection)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(moveDirection),
                Time.deltaTime * rotationSpeed
            );
        }

        private void UpdateAnimation(float movementSpeed)
        {
            animator.SetBool("IsWalk", movementSpeed > 0.1f);
            SetSpeedToBlendTree(movementSpeed);
        }

        private void ApplyMovement(Vector3 moveDirection, float movementSpeed)
        {
            if (characterController.isGrounded)
            {
                verticalVelocity = -gravity * Time.deltaTime;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            Vector3 velocity = moveDirection * movementSpeed;
            velocity.y = verticalVelocity;

            characterController.Move(velocity * Time.deltaTime);
        }

        public void SetSpeedToBlendTree(float movementSpeed)
        {
            animator.SetFloat("speed", movementSpeed);
        }

        public void StopMovement()
        {
            animator.SetBool("IsWalk", false);
            SetSpeedToBlendTree(0f);
        }
    }
}
