using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AntQueen
{
    
    public class InputReader : MonoBehaviour, QueenControls.IControlsActions
    {
        private QueenControls controls;
        public Vector2 movement;
        public Vector2 target;

        public Action spawnAnt;

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
            movement= context.ReadValue<Vector2>();
        
            Debug.Log($"Movement Vector: ({movement.x},{movement.y})");
        }

        public void OnTargeting(InputAction.CallbackContext context)
        {
            target = context.ReadValue<Vector2>();
        
            Debug.Log($"Target Vector: ({target.x},{target.y})");
        }

        public void OnSpawnAnt(InputAction.CallbackContext context)
        {
            Debug.Log("spawn ant go now!");
            spawnAnt?.Invoke();
        }


        public void OnButtonTest(InputAction.CallbackContext context)
        {
            Debug.Log("you pressed w");
        }

    }
}