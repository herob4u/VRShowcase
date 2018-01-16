using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    public class TriggerBase : MonoBehaviour
    {
        [SerializeField] private bool triggerOnce = false;
        public UnityEvent TriggerEnter;
        public UnityEvent TriggerExit;

        // Use this for initialization
        protected virtual void Start()
        {
            Collider collider = GetComponent<Collider>();

            if (!collider)
            {
                gameObject.AddComponent<BoxCollider>();
            }

            // Trigger Objects are always trigger based - duh
            collider.isTrigger = true;
        }

        virtual protected void OnTriggerEnter(Collider other)
        {
            TriggerEnter.Invoke();

            if(triggerOnce)
            {
                Destroy(this);
            }
        }

        virtual protected void OnTriggerExit(Collider other)
        {
            TriggerExit.Invoke();
        }

    }
}