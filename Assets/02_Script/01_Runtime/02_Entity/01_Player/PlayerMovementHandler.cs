using System.Collections;
using UnityEngine;
using TheLastHeir.Runtime.Combat;

namespace TheLastHeir.Runtime.Entity
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementHandler : EntityOwnedHandler<Player>
    {
        private Player player;
        private LockOnHandler _lockOnHandler;
        
        public Transform cameraTransform;
        public float CurrentSpeed { get; private set; }
        public bool IsGrounded { get; private set; }

        private Vector3 velocity;
        public bool isRolling = false;
        private float turnSmoothVelocity;
        private float rollTimer;
        private Vector3 rollDirection;
        private const float GroundedYVelocity = -2f;
        
        private float _lastRollTime;
        public bool CanRoll => Time.time >= _lastRollTime + player.playerStats.RollCooldown;

        public void Initialize(Player owner)
        {
            player = owner;
            _lockOnHandler = player.GetComponent<LockOnHandler>();
            _lastRollTime = -999f;
        }
        
        public void Tick()
        {
            HandleGroundCheck();
            Vector3 horizontalVelocity;
            if (isRolling) horizontalVelocity = HandleRoll();
            else horizontalVelocity = HandleMovement();
            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;
            HandleGravity();
            ApplyMovement();
        }

        public void StopMovement()
        {
            velocity = new Vector3(0, velocity.y, 0);
            CurrentSpeed = 0f;
        }

        private void HandleGroundCheck()
        {
            IsGrounded = Physics.CheckSphere(player.GroundCheck.position, player.playerStats.GroundCheckRadius, player.playerStats.GroundMask);
            if (IsGrounded && velocity.y < 0)
                velocity.y = GroundedYVelocity;
        }

        private Vector3 HandleMovement()
        {
            Vector2 input = player.PlayerInput.move;
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            Vector3 moveDir = camForward * input.y + camRight * input.x;
            
            if (moveDir.magnitude > 1f) moveDir.Normalize();
            float speed = player.PlayerInput.sprint ? player.playerStats.SprintSpeed : player.playerStats.WalkSpeed;
            Vector3 horizontalVelocity = moveDir * speed;
            CurrentSpeed = horizontalVelocity.magnitude;
            
            if (player.CanRotate)
            {
                if (_lockOnHandler != null && _lockOnHandler.IsLockedOn)
                {
                    Transform target = _lockOnHandler.GetCurrentTarget();
                    if (target != null)
                    {
                        Vector3 dirToTarget = target.position - transform.position;
                        dirToTarget.y = 0;

                        if (dirToTarget != Vector3.zero)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
                        }
                    }
                }
                else if (moveDir.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, player.playerStats.RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }

            return horizontalVelocity;
        }
        
        public void HandleJump()
        {
            if (IsGrounded)
            {
                velocity.y = Mathf.Sqrt(player.playerStats.JumpHeight * -2f * player.playerStats.Gravity);
                player.AnimationHandler.PlayJumpAnimation();
            }
        }

        private void HandleGravity()
        {
            if (isRolling && IsGrounded)
            {
                velocity.y = GroundedYVelocity;
                return;
            }
            if (!IsGrounded)
            {
                if (velocity.y < 0) velocity.y += player.playerStats.Gravity * player.playerStats.FallMultiplier * Time.deltaTime;
                else velocity.y += player.playerStats.Gravity * Time.deltaTime;
            }
        }

        private void ApplyMovement()
        {
            player.cc.Move(velocity * Time.deltaTime);
        }
        
        public void HandleRollInput()
        {
            if (IsGrounded && !isRolling)
            {
                _lastRollTime = Time.time;
                
                isRolling = true;
                rollTimer = 0f;
                
                Vector2 input = player.PlayerInput.move;
                if (input != Vector2.zero)
                {
                    Vector3 camForward = cameraTransform.forward;
                    Vector3 camRight = cameraTransform.right;
                    camForward.y = 0;
                    camRight.y = 0;
                    camForward.Normalize();
                    camRight.Normalize();
                    
                    rollDirection = (camForward * input.y + camRight * input.x).normalized;
                    transform.rotation = Quaternion.LookRotation(rollDirection);
                }
                else
                {
                    rollDirection = transform.forward;
                }

                player.AnimationHandler.PlayRollAnimation();
            }
        }

        private Vector3 HandleRoll()
        {
            rollTimer += Time.deltaTime;
            if (rollTimer >= player.playerStats.RollDuration)
            {
                isRolling = false;
                return Vector3.zero;
            }
            return rollDirection * player.playerStats.RollSpeed;
        }
    }
}