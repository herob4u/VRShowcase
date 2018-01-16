using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSim.Entities
{
    public class DoorRotating : BaseDoor
    {
        [SerializeField] protected Vector3 rotation;

        private Quaternion finalPos;
        private Quaternion defaultRotation;

        float tLerp = 0.1f;

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

            // For rotating doors, reparent the door to the pivot to
            // simplify rotation code.
            transform.SetParent(origin, true);
            defaultRotation = this.transform.rotation;

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

            if (scalar == 1)
            finalPos = transform.rotation * Quaternion.Euler(rotation);

            else
            finalPos = defaultRotation ;

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
            if (!bIsMoving)
            {
                this.enabled = false;
            }


            //origin.rotation = Quaternion.Slerp(origin.rotation, Quaternion.Euler(finalPos), 0.5f);
            //transform.rotation.SetLookRotation(origin.position - transform.position);

            origin.rotation = Quaternion.Slerp(origin.rotation,finalPos, tLerp * speed);


            // Once the door is fully opened/closed, broadcast the associated events.
            if (SimilarRotations(origin.rotation, finalPos))
            {
                tLerp = 0.1f;

                // Snap the rotation to the final resting position for proper accuracy.
                origin.rotation = finalPos;

                Debug.Log("Finished Rotating");
                if (scalar == 1)
                {
                    OnFullyOpened.Invoke();
                }
                else if (scalar == -1)
                {
                    OnFullyClosed.Invoke();
                }
            }

            tLerp += Time.deltaTime;
        }

        
        /// <summary>
        /// Similar Rotation - Helper Function
        /// Given 2 Quaternions, indicates whether they are similar or equal.
        /// </summary>
        private bool SimilarRotations(Quaternion q1, Quaternion q2)
        {
            return Mathf.Abs(Quaternion.Angle(q1, q2)) < 0.1;
        }
    }
}