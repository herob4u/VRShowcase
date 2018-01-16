using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSim.Entities
{
    public class TriggerPush : TriggerBase
    {
        private enum PushDirection { None, Forward, Right}

        [Tooltip("If set, auto-determines the push direciton using the object's local coordinates.")]
        [SerializeField] private PushDirection localPushDirection;
        
        [Tooltip("Multiplier for the push direction.")]
        public float pushStrength;

        [Tooltip("If the local push direction is set to None, uses this override value to drive the push direction and strength.")]
        public Vector3 overridePushDirection;

        public bool bPhysicsForce;

        private List<Collider> pushObjects = new List<Collider>();

        protected override void Start()
        {
            base.Start();

            pushObjects = new List<Collider>();

            switch(localPushDirection)
            {
                case PushDirection.Forward:
                    overridePushDirection = transform.forward * pushStrength;
                    break;
                case PushDirection.Right:
                    overridePushDirection = transform.right * pushStrength;
                    break;
            }
        }


        // Once an object enters the trigger, add it to out list of objects to update.
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if(other.GetComponent<Rigidbody>())
            {
                pushObjects.Add(other);

                // Verify the push trigger is enabled once the list of objects is no longer empty.
                if (pushObjects.Count <= 1)
                {
                    this.enabled = true;
                }
            }
        }

        // Once the object leaves the trigger zone, remove it from our tracking list
        // as we no longer need to update.
        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            if(pushObjects.Contains(other))
            {
                pushObjects.Remove(other);
            }
        }

        // Keep Pushing the objects along the provided direction.
        // If no objects exist in the zone, disable this script.
        private void Update()
        {
            if(pushObjects.Count <= 0)
            {
                this.enabled = false;
            }

            foreach(Collider pushObj in pushObjects)
            {
                if (bPhysicsForce)
                {
                    pushObj.GetComponent<Rigidbody>().velocity = overridePushDirection;
                }
                else
                {
                    pushObj.transform.Translate(overridePushDirection, Space.World);
                }
            }

        }
    }
}