using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    public class Rotatable : MonoBehaviour
    {
        private bool bActive;
        private float currentSpeed;

        [SerializeField] protected float speed;
        [SerializeField] protected float accel;
        [SerializeField] protected int duration;
        [SerializeField] protected Vector3 rotation;
        [SerializeField] protected bool bStartsOn;

        [SerializeField] protected AudioClip startSound;
        [SerializeField] protected AudioClip moveSound;
        [SerializeField] protected AudioClip stopSound;

        private AudioSource audioSource;

        public UnityEvent OnStartRotation;
        public UnityEvent OnStopRotation;
        public UnityEvent OnMaxRotation;
        public UnityEvent OnMinRotation;

        private void Start()
        {
            // Rotations about the Zero Vector are invalid!
            if(rotation == Vector3.zero)
            {
                rotation = Vector3.forward;
            }

            this.enabled = bStartsOn;
            bActive = bStartsOn;

            // Event Subscription
            OnStartRotation.AddListener(() => { OnStartRotationHandler(); });
            OnStopRotation.AddListener(() => { OnStopRotationHandler(); });
            OnMaxRotation.AddListener(() => { OnMaxRotationHandler(); });
            OnMinRotation.AddListener(() => { OnMinRotationHandler(); });
        }


        public void StartRotation()
        {
            if (!bActive)
            {
                if(accel < 0)
                {
                    accel *= -1;
                }

                OnStartRotation.Invoke();
            }
        }

        public void StopRotation()
        {
            if (bActive)
            {
                if (accel > 0)
                {
                    accel *= -1;
                }

                OnStopRotation.Invoke();
            }
        }

        private void FixedUpdate()
        {
            // Smoothly increment/decrement our speed until we attain
            // maximum/minimum speed.
            currentSpeed = Mathf.Clamp(currentSpeed + accel, 0, speed);

            if (currentSpeed >= speed)
            {
                OnMaxRotation.Invoke();
            }
            else if (currentSpeed <= 0)
            {
                OnMinRotation.Invoke();
            }
            
            
            transform.Rotate(rotation, currentSpeed);
        }

        private void PlaySound(AudioClip clip, bool isLooping = false)
        {
            if (clip != null)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.loop = isLooping;
                audioSource.Play();
            }
        }

        #region EVENT_HANDLER

        protected virtual void OnStartRotationHandler()
        {
            PlaySound(startSound);

            this.enabled = true;
        }

        protected virtual void OnStopRotationHandler()
        {
            PlaySound(stopSound);
        }

        protected virtual void OnMaxRotationHandler()
        {
            bActive = true;
        }

        protected virtual void OnMinRotationHandler()
        {
            bActive = false;
            this.enabled = false;
        }

        #endregion

    }
}

