using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events; 

namespace VRSim.Entities
{
    public class DoorSliding : BaseDoor
    {
        [SerializeField]
        protected Vector3 direction;

        private Vector3 finalPos;
        private float tLerp = 0.1f;

        protected override void Initialize()
        {
            // To simplify code, assume doors always start closed. Ensures consistency further on - Trust me...
            bIsMoving = false;
            bIsOpen = false;

            // Translate the door to the open position if designated as "Starts Open"
            if (bStartsOpen)
            {
                Use();
            }
        }

        public override void Use()
        {
            // Edge Case: Do nothing if the door is currently moving.
            // Allow the door to finish its current swing to avoid any bugs.
            if (bIsMoving)
            {
                return;
            }


            base.Use();

            finalPos = origin.position + direction * scalar;
            this.enabled = true;
        }


        /// <summary>
        /// FixedUpdate
        /// When the door is enabled, the FixedUpdate function incrementally interpolates
        /// the position of the door to match the desired end location. The door is then 
        /// disabled until it receives another use command.
        /// </summary>
        private void FixedUpdate()
        {
            // Once movement is complete, disable the door until the next trigger.
            if(!bIsMoving)
            {
                this.enabled = false;
            }

            transform.position = Vector3.Lerp(transform.position, finalPos, speed * tLerp);

            Debug.Log("Door Pos: " + transform.position);
            Debug.Log("Final Pos: " + finalPos);

            // Once the door is fully opened/closed, broadcast the associated events.
            if (Mathf.Approximately(transform.position.magnitude, finalPos.magnitude))
            {
                // Reset the lerp variable
                tLerp = 0.1f; 

                if(scalar == 1)             // Positive Direction Indicates door was being opened
                {
                    OnFullyOpened.Invoke();
                }
                else if(scalar == -1)
                {
                    OnFullyClosed.Invoke();
                }
            }

            // Keep slightly incrementing the lerp variable
            // to recreate an ease in effect.
            tLerp += 0.1f;
        }
    }
}