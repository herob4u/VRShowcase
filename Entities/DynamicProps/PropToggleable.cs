using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    public class PropToggleable : BaseUsable
    {
        private Animator propAnimator;
        private bool bIsBeingUsed;

        public bool bStartsOn;
        public float timeBeforeReuse = 2f;
        public UnityEvent UseStarted;
        public UnityEvent UseEnded;

        private void Start()
        {
            propAnimator = GetComponent<Animator>();

            if(propAnimator)
            { propAnimator.SetBool("StartsOn", bStartsOn); }

            UseEnded.AddListener(() => { OnUseEndHandler(); });
        }

        public override void Use()
        {
            if (!bIsBeingUsed)
            {
                bIsBeingUsed = true;

                propAnimator.ResetTrigger("Use");
                propAnimator.SetTrigger("Use");

                UseStarted.Invoke();
                Invoke("InvokeEndUse", timeBeforeReuse);
            }
        }

        private void InvokeEndUse()
        {
            UseEnded.Invoke();
        }

        private void OnUseEndHandler()
        {
            Debug.Log("Finished Using!");
            bIsBeingUsed = false;
        }
    }
}