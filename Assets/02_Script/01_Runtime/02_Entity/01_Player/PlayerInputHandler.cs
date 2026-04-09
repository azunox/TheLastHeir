using UnityEngine;
using UnityEngine.InputSystem;

namespace TheLastHeir.Runtime.Entity
{
    public class PlayerInputHandler : MonoBehaviour, PlayerInputs.IPlayerActions
    {
        public static PlayerInputHandler Instance { get; private set; }

        [Header("Movement Input")]
        public Vector2 move;
        public bool sprint;
        
        [Header("Action Inputs")]
        public bool AttackTriggered { get; private set; }
        public bool HeavyAttackTriggered { get; private set; }
        public bool RollTriggered { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool InventoryTriggered { get; private set; }
        public bool LockOnTriggered { get; private set; }
        public bool StatTriggered { get; private set; }
        public bool InteractTriggered { get; private set; }
        public bool MainMenuTriggered { get; private set; }
        public bool DialogueInput { get; private set; }
        public bool PotionTriggered { get; private set; }

        public bool canInput = true;

        [Header("Mouse/Look")]
        public Vector2 look;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        private PlayerInputs _inputActions;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            _inputActions = new PlayerInputs();
            _inputActions.Player.SetCallbacks(this);
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void LateUpdate()
        {
            ResetTriggers();
        }

        private void ResetTriggers()
        {
            AttackTriggered = false;
            HeavyAttackTriggered = false;
            RollTriggered = false;
            JumpTriggered = false;
            InventoryTriggered = false;
            LockOnTriggered = false;
            StatTriggered = false;
            InteractTriggered = false;
            MainMenuTriggered = false;
            DialogueInput = false;
            PotionTriggered = false;
        }
        

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            look = context.ReadValue<Vector2>();
            cameraHorizontalInput = look.x;
            cameraVerticalInput = look.y;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && canInput) 
            {
                JumpTriggered = true;
                DialogueInput = true;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed) sprint = true;
            else if (context.canceled) sprint = false;
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.performed && canInput) RollTriggered = true;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed && canInput)
            {
                AttackTriggered = true;
                DialogueInput = true;
            }
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.performed && canInput) HeavyAttackTriggered = true;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractTriggered = true;
                DialogueInput = true;
            }
        }

        public void OnTab(InputAction.CallbackContext context)
        {
            if (context.performed && canInput) LockOnTriggered = true;
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.performed) InventoryTriggered = true;
        }

        public void OnStats(InputAction.CallbackContext context)
        {
            if (context.performed) StatTriggered = true;
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.performed) MainMenuTriggered = true;
        }

        public void OnPotion(InputAction.CallbackContext context)
        {
            if (context.performed) PotionTriggered = true;
        }
        
        public void OnBlock(InputAction.CallbackContext context) { }
        public void OnUp(InputAction.CallbackContext context) { }
        public void OnDown(InputAction.CallbackContext context) { }
        public void OnEnter(InputAction.CallbackContext context) { }
    }
}