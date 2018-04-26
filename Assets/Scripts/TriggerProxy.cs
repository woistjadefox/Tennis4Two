using UnityEngine;
using UnityEngine.Events;

namespace GameLab.Tennis4Two {
    
    [RequireComponent(typeof(Collider))]
    public class TriggerProxy : MonoBehaviour
    {
        [System.Serializable]
        private sealed class UnityEventCollider : UnityEvent<Collider> { }

        [Header("Settings")]
        [SerializeField]
        private bool ignoreIncomingTriggers = true;
        [SerializeField]
        private LayerMask layerMask;

        [Header("Events")]
        [SerializeField]
        private UnityEventCollider onTriggerEnter;

        [SerializeField]
        private UnityEventCollider onTriggerExit;


        private void OnTriggerEnter(Collider other)
        {
            if (IsValid(other) == false) return;
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsValid(other) == false) return;
            onTriggerExit.Invoke(other);
        }

        private bool IsValid(Collider other)
        {
            if (ignoreIncomingTriggers && other.isTrigger) return false;
            return layerMask == (layerMask | (1 << other.gameObject.layer));
        }

    }
}

