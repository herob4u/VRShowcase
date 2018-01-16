using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSim.Entities
{
    public enum LightPresets { Normal, ProbeSlow, ProbeFast, Flicker };

    public class LightSource : MonoBehaviour
    {
        [Tooltip("The Light component to possess. Defaults to self if left empty. If no light component was found, the script invalidates and removes itself.")]
        public Light lightSource;

        public float minIntensity;
        public float maxIntensity;

        [Tooltip("Select a type of light to control the intensity behavior over time.")]
        public LightPresets lightType;


        private Dictionary<LightPresets, System.Action> intensityFunctions;

        private float time;

        // Validate that the Light component exists
        private void Awake()
        {
            if(lightSource == null)
            {
                lightSource = GetComponent<Light>();
                
                if(lightSource == null)
                {
                    Destroy(this);
                }
            }

            if(intensityFunctions == null || intensityFunctions.Count <= 0)
            {
                intensityFunctions = new Dictionary<LightPresets, System.Action>()
                {
                    {LightPresets.Normal, NormalIntensity},
                    {LightPresets.ProbeSlow, SlowProbeIntensity },
                    {LightPresets.ProbeFast, FastProbeIntensity },
                    {LightPresets.Flicker, FlickerIntensity }
                };
            }

            time = Time.time;
        }

        /// <summary>
        /// Toggles the controlled light on and off.
        /// </summary>
        public void Toggle()
        {
            lightSource.enabled = !lightSource.enabled;
            this.enabled = lightSource.enabled;
        }

        /// <summary>
        /// Turns off the controlled light and disables
        /// this script until turned on again.
        /// </summary>
        public void TurnOff()
        {
            lightSource.enabled = false;
            this.enabled = false;
        }

        /// <summary>
        /// Turns on the controlled light and enables 
        /// this script until turned off again.
        /// </summary>
        public void TurnOn()
        {
            lightSource.enabled = true;
            this.enabled = true;
        }


        // Control the dynamic behavior of the light, changing its
        // intensity over time depending on the preset function.
        private void FixedUpdate()
        {
            intensityFunctions[lightType].Invoke();
        }


        #region INTENSITY_FUNCTIONS
        // Intensity Functions that control the intensity of the light over time.

        private void NormalIntensity()
        {
            lightSource.intensity = maxIntensity;
            this.enabled = false;
        }

        private void SlowProbeIntensity()
        {
            lightSource.intensity = Mathf.Clamp(Mathf.Abs(Mathf.Sin(Time.time)) * maxIntensity, minIntensity, maxIntensity);
        }

        private void FastProbeIntensity()
        {
            lightSource.intensity = Mathf.Clamp(Mathf.Abs(Mathf.Sin(Time.time * 2)) * maxIntensity, minIntensity, maxIntensity);
        }

        private void FlickerIntensity()
        {
            if(DelayComplete(0.05f))
            lightSource.intensity = new System.Random().Next(0, 2) * maxIntensity;
        }


        // Returns true if a specified time has elapsed since the last reset.
        private bool DelayComplete(float delay)
        {
            if(Time.time - time > delay)
            {
                time = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}