using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace VRSim.Entities
{
    /// <summary>
    /// A Toggle Button is one that remains either 
    /// of 2 states permanently (typically On/Off). Using
    /// the button invokes the Toggle function. Unlike the basic button, the Toggle button
    /// needs to retains memory of the last press to play the proper animations.
    /// </summary>
    public class ToggleButton : BaseButton
    {
        private bool bStartOn = true;
        public UnityEvent ToggledOn;
        public UnityEvent ToggledOff;

        protected override void Start()
        {
            base.Start();

            if (!buttonAnimator)
            { Debug.LogWarning("ToggleButton: No Animator Found. Cannot Animate on press."); }

            else
            {
                buttonAnimator.SetBool("IsOn", bStartOn);
            }
        }

        /// <summary>
        /// Overriden to Invoke additional events
        /// </summary>
        protected override void ButtonClickHandler()
        {
            base.ButtonClickHandler();

            // If the button state was previously on, then we invoke
            // the Toggle Off event. Otherwise, the button was ToggledOn.
            if (bStartOn)
            { ToggledOff.Invoke(); }
            else
            { ToggledOn.Invoke(); }

        }


        /// <summary>
        /// Plays the Toggle animation by firing
        /// a trigger to the animation controller.
        /// </summary>
        protected override void OnUseAnimationHandler()
        {
            if(!buttonAnimator) { return; }

            buttonAnimator.ResetTrigger("Press");
            buttonAnimator.SetTrigger("Press");

            // Toggle the state of the button
            buttonAnimator.SetBool("IsOn", bStartOn = !bStartOn);
        }
    }
}