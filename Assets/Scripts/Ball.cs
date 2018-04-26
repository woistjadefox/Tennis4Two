using UnityEngine;
using UnityEngine.Events;

namespace GameLab.Tennis4Two {
    
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {

        [Header("Settings")]
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

        [Header("References")]
        [SerializeField]
        private Transform net;
        [SerializeField]
        private Logic logic;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onGroundHit;
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

        public Transform GetNet()
        {
            return net;
        }

        public void SetNet(Transform target)
        {
            net = target;
        }

        public void SetLogic(Logic target) 
        {
            logic = target;
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
                HandleGorundHit();
            }
		}

		private void Start()
		{
			if(net == null || logic == null) 
            {
                Debug.LogWarning(gameObject.name + ": missing references on script!");
                GetRigidbody().isKinematic = true;
                enabled = false;
            }
		}

        private void HandleGorundHit() 
        {
            // trigger UnityEvent for effect etc.
            onGroundHit.Invoke();

            // check if the ball hits the ground on the left side of the net
            if (GetRigidbody().position.x < net.position.x) {
                
                groundHits[0]++;
                ResetGroundHits(1);

                if (HasMaxGroundHits(0)) {
                    // count point for player 1
                    logic.CountPointForPlayer(1);

                    // spawn to player 0
                    GetRigidbody().isKinematic = true;
                    GetRigidbody().position = logic.GetPlayer(0).transform.position;
                }

            } else {

                groundHits[1]++;
                ResetGroundHits(0);

                if (HasMaxGroundHits(1)) {
                    // count point for player 0
                    logic.CountPointForPlayer(0);

                    // spawn to player 1
                    GetRigidbody().isKinematic = true;
                    GetRigidbody().position = logic.GetPlayer(1).transform.position;
                }
            }
        }

        private bool HasMaxGroundHits(int player) {
            
            if(groundHits[player] > maxGroundHits)
            {
                SetLastPlayer(null);
                onReachedMaxGroundHits.Invoke();
                return true;
            }

            return false;
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


