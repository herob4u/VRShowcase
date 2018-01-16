using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRSim.Entities
{
    public class Debug_Trigger : MonoBehaviour
    {
        [SerializeField] public BaseUsable useObj;

        private void OnTriggerEnter(Collider other)
        {
            useObj.Use();
        }

        private void OnTriggerExit(Collider other)
        {
            useObj.Use();
        }
    }
}