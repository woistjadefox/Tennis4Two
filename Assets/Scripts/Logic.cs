using UnityEngine;

namespace GameLab.Tennis4Two {
    
    public class Logic : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool debugInput = true;

        private void Start()
        {
            Time.timeScale = 0.25f;
            InputHandler.Init(debugInput);
        }

    }
}

