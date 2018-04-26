using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameLab.Tennis4Two {
    
    public class Logic : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float debugTimeScale = 1f;
        [SerializeField]
        private bool debugInput = true;

        [Header("References")]
        [SerializeField]
        private Player[] players;
        [SerializeField]
        private Text scoreP1;
        [SerializeField]
        private Text scoreP2;

        private int[] scores;

        private void Start()
        {
            Time.timeScale = debugTimeScale;

            InputHandler.Init(debugInput);

            scores = new int[players.Length];
        }

        public Player GetPlayer(int player)
        {
            return players[player];
        }

        public void CountPointForPlayer(int player) 
        {
            scores[player]++;
            UpdateScoresUI();
        }

        public void UpdateScoresUI() 
        {
            scoreP1.text = scores[0].ToString();
            scoreP2.text = scores[1].ToString();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}

