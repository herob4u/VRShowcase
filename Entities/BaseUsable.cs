using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSim.Entities
{
    public class BaseUsable : MonoBehaviour, IUsable
    {
        [Tooltip("Sets another game object to act as a parent to drive the current object's transformations. Defaults to self if left empty.")]
        [SerializeField] protected Transform parent;

        [Tooltip("Indicates the object can be used by simply touching it without the use of external triggers.")]
        [SerializeField] protected bool bTouchTrigger;
 

        public virtual void Use()
        { }


        /// <summary>
        /// Event dispatched when another collider enters
        /// this object's collider trigger area.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(gameObject.name + "Collided with " + collision.gameObject.name);

            Use();
        }
    }
}