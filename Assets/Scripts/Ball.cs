using UnityEngine;
using UnityEngine.Events;

namespace GameLab.Tennis4Two {
    
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField]
        private Transform net;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        private int maxGroundHits = 1;

        [Header("Gravity Settings")]
        [SerializeField]
        private bool multiplyMass = true;
        [SerializeField]
        private float gravityMultiplier = 2f;
        [SerializeField]
        private float movementThreshold = 0.35f;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onReachedMaxGroundHits;

        [SerializeField]
        private int[] groundHits = new int[2];
        private Player lastPlayer;
        private new Rigidbody rigidbody;

        public Rigidbody GetRigidbody()
        {
            if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
            return rigidbody;
        }

        public Player GetLastPlayer()
        {
            return lastPlayer;
        }

        public void SetLastPlayer(Player player)
        {
            lastPlayer = player;
        }

        public int GetGroundHits(int playerNr)
        {
            return groundHits[playerNr];
        }

        public void ResetGroundHits(int playerNr) 
        {
            groundHits[playerNr] = 0;
        }

		private void OnCollisionEnter(Collision collision)
		{
            // check if we have a collision with the ground
            if(groundLayer == (groundLayer | (1 << collision.gameObject.layer))) 
            {
                int player;

                if(GetRigidbody().position.x < net.position.x) 
                {
                    player = 0;
                    ResetGroundHits(1);
                } else {
                    player = 1;
                    ResetGroundHits(0);
                }

                groundHits[player]++;

                CheckMaxGroundHits(groundHits[player]);
            }
		}

        private void CheckMaxGroundHits(int hits) {
            
            if(hits > maxGroundHits)
            {
                onReachedMaxGroundHits.Invoke();
            }
        }

		private void FixedUpdate()
        {
            if (enabled) {
                if (GetRigidbody().velocity.sqrMagnitude > movementThreshold) {
                    GetRigidbody().AddForce(Physics.gravity * (multiplyMass ? rigidbody.mass : 1f) * gravityMultiplier);
                }
            }
        }
    }
}


