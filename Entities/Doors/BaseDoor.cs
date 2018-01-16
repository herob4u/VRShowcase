using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    public class BaseDoor : BaseUsable
    {
        [Tooltip("The Pivot point or origin used as a reference for translation or rotation of the door.")]
        [SerializeField] protected Transform origin;

        [Tooltip("Starts the door in the open position.")]
        [SerializeField] protected bool bStartsOpen = false;     // Start the door at the opened position if true

        [Tooltip("Sound to be played when the door starts opening, starts closing, or moves.")]
        [SerializeField] protected AudioClip openSound;
        [SerializeField] protected AudioClip moveSound;
        [SerializeField] protected AudioClip stopSound;
        [SerializeField] protected AudioClip closeSound;

        [Tooltip("Induced delay before the door actually starts moving upon a trigger.")]
        public float delay;

        [Tooltip("Time in seconds before the door goes back to automatically closing. Values less than zero mean the door never closes automatically.")]
        public float timeBeforeClose;
        public float speed;

        [SerializeField] protected UnityEvent OnStartOpen;
        [SerializeField] protected UnityEvent OnStartClose;
        [SerializeField] protected UnityEvent OnFullyOpened;
        [SerializeField] protected UnityEvent OnFullyClosed;

        protected int scalar = 1;               // Dictates the direction of movement (close or open) depending on the last state of the door.

        protected bool bIsMoving;               // Indicates whether the door is currently being opened or closed.
        protected bool bIsOpen;

        private AudioSource audioSource;

        // Initialize base class defaults and call
        // derived class initializer.
        private void Start()
        {
            // Add an Audio Source to the door to manage playing sounds.
            audioSource = GetComponent<AudioSource>();

            if(audioSource == null)
            { audioSource = gameObject.AddComponent<AudioSource>(); }

            // Start disabled if the door is not being used. This minimizes unecessary overhead
            // by disabling the Update function.
            this.enabled = false;

            if(origin == null)
            { origin = this.transform; }

            // Add A Collider if none exists
            if(!GetComponent<Collider>())
            { gameObject.AddComponent<BoxCollider>(); }

            speed = 0.2f; // experimental
            bIsMoving = false;

            // Assign all events to their respective handler.
            OnStartOpen.AddListener(() => { this.StartOpeningHandler(); });
            OnStartClose.AddListener(() => { this.StartClosingHandler(); });
            OnFullyOpened.AddListener(() => { this.FullyOpenedHandler(); });
            OnFullyClosed.AddListener(() => { this.FullyClosedHandler(); });

            // Call the post intialization function to set specific class defaults.
            Initialize();
        }


        // Function responsible for executing open/close
        // sequence of door.
        public override void Use()
        {
            Debug.Log("Began Using Door.");
            if (bIsOpen)
            {
                scalar = -1;
                OnStartClose.Invoke();
                Debug.Log("STARTED CLOSING");
            }
            else
            {
                scalar = 1;
                OnStartOpen.Invoke();
                Debug.Log("STARTED OPENING");
            }
        }

        // Overridable initialize function to setup class defaults
        // of derived classes. Always after base Start function.
        protected virtual void Initialize() { }


        #region EVENT_HANDLERS
        protected virtual void FullyOpenedHandler()
        {
            Debug.Log("Fully Opened!");
            audioSource.Stop();

            if (stopSound != null)
            {
                audioSource.clip = stopSound;
                audioSource.loop = false;
                audioSource.Play();
                Invoke("OnAudioDone", stopSound.length);    // Invoke the handling function when audio finishes playing.
            }

            bIsMoving = false;
            bIsOpen = true;

            // Automatically closes the door after the alloted time has passed.
            if(timeBeforeClose > 0 && !bIsMoving)
            Invoke("Use", timeBeforeClose);
        }

        protected virtual void FullyClosedHandler()
        {
            Debug.Log("Fully Closed!");
            audioSource.Stop();

            if (stopSound != null)
            {
                audioSource.clip = stopSound;
                audioSource.loop = false;
                audioSource.Play();
                Invoke("OnAudioDone", stopSound.length);    // Invoke the handling function when audio finishes playing.
            }

            bIsMoving = false;
            bIsOpen = false;
        }

        protected virtual void StartOpeningHandler()
        {
            audioSource.Stop();
            
            if(openSound != null)
            {
                audioSource.clip = openSound;
                audioSource.loop = false;
                audioSource.Play();
                Invoke("OnAudioDone", openSound.length);    // Invoke the handling function when audio finishes playing.
            }

            bIsMoving = true;
        }

        protected virtual void StartClosingHandler()
        {
            audioSource.Stop();

            if(closeSound != null)
            {
                audioSource.clip = closeSound;
                audioSource.loop = false;
                audioSource.Play();
                Invoke("OnAudioDone", closeSound.length);    // Invoke the handling function when audio finishes playing.
            }

            bIsMoving = true;
        }

        // Triggered when an audio clip finishes playing.
        protected virtual void OnAudioDone()
        {
            if(bIsMoving && moveSound != null)
            {
                audioSource.Stop();
                audioSource.clip = moveSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        #endregion

    }
}