using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    /// <summary>
    /// A Basic Button is a one click trigger.
    /// It retains no memory of its current state. Pressing the button incorporates
    /// one press animation.
    /// </summary>
    public class BaseButton : BaseUsable
    {
        [SerializeField] protected bool useOnce;
        protected Animator buttonAnimator;


        // Sound management upon pressing button
        private AudioSource audioSource;
        [SerializeField] protected AudioClip clickSound;

        // Events
        public UnityEvent OnButtonClick;


        // Basic Initializations
        private void Awake()
        {
            buttonAnimator = GetComponent<Animator>();
        }


        public override void Use()
        {
            // Play the animation
            OnUseAnimationHandler();

            // Then Invoke the event to trigger functionalities
            // within the game.
            OnButtonClick.Invoke();

            // If it is a one time use button, then destroy the script
            // as it is no longer needed.
            if(useOnce)
            { Destroy(this); }
        }

        protected virtual void ButtonClickHandler()
        {
            Debug.Log("button clicked");
            audioSource.Stop();
            audioSource.clip = clickSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        protected virtual void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            OnButtonClick.AddListener(() => { ButtonClickHandler(); });
        }

        // Triggers the animation controller to play a specific animation sequence.
        // Basic buttons' animations are driven by a simple Press trigger that pushes the button in and out.
        protected virtual void OnUseAnimationHandler()
        {
            if (buttonAnimator)
            {
                buttonAnimator.ResetTrigger("Press");
                buttonAnimator.SetTrigger("Press");
            }
        }
    }
}