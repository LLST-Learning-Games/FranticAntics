using UnityEngine;
using UnityEngine.InputSystem;

namespace AntQueen
{
    
    public class InputReader : MonoBehaviour, QueenControls.IControlsActions
    {
        private QueenControls controls;

        private void OnEnable()
        {
            if (controls != null)
                return;

            controls = new QueenControls();
            controls.Controls.SetCallbacks(this);
            controls.Controls.Enable();
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            //Vector2 pos = Gamepad.current.leftStick.ReadValue();
            Vector2 pos = context.ReadValue<Vector2>();
        
            Debug.Log($"Movement Vector: ({pos.x},{pos.y})");
        }

        public void OnButtonTest(InputAction.CallbackContext context)
        {
            Debug.Log("you pressed w");
        }
        
        
    }
}