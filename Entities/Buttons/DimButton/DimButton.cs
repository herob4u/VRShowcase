using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRSim.Entities
{
    public enum Modes { Increment = 1, Decrement = -1 };

    public class DimButton : BaseButton
    {
        float value = 100;
        public byte minVal = 0;
        public byte maxVal = 100;
        public float increment;

        public List<Light> dimLights;
        
        
        // Indicates whether the button will increment or decrement on next press.
        private Modes buttonMode = Modes.Decrement;
   
        private void DimValue()
        {
            value = Mathf.Clamp(value + increment * (int)buttonMode, minVal, maxVal);

            if(dimLights != null && dimLights.Count > 0 )
            {
                foreach(Light light in dimLights)
                {
                    light.intensity = value;
                }
            }
        }

        /// <summary>
        /// Use (Override)
        /// Uses the Button, effectively triggering the click event
        /// and apply the incremental dimming effect.
        /// </summary>
        public override void Use()
        {
            base.Use();
            DimValue();
        }

        /// <summary>
        /// Set Button Mode
        /// Exposes the mode of the button (Increments or Decrement)
        /// for toggling purposes
        /// </summary>
        public void SetButtonMode(Modes mode)
        {
            buttonMode = mode;
        }

        private void Update()
        {
            Debug.Log(value);
        }

        public float Value
        {
            get { return (float)(value / maxVal); }
        }
    }
}

