using UnityEngine;
using System.Collections.Generic;

namespace GameLab.Tennis4Two {

    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField, Range(0, 1)]
        private int playerNr = 0;

        [SerializeField]
        private string playerName = "Player1";

        [SerializeField]
        private float hitForce = 10f;

        private Ball nextBall;
        private List<Ball> balls = new List<Ball>();


        public string GetPlayerName()
        {
            return playerName;
        }

        public void OnBallEnterZone(Collider other)
        {
            // add ball to balls list
            if (other.attachedRigidbody != null) {

                Ball ball = other.attachedRigidbody.GetComponent<Ball>();

                if (ball != null) {

                    if(balls.Contains(ball) == false) {
                        balls.Add(ball);
                    }
                }
            }
        }

        public void OnBallExitZone(Collider other)
        {
            // remove ball from balls list
            if (other.attachedRigidbody != null) {

                Ball ball = other.attachedRigidbody.GetComponent<Ball>();

                if (ball != null && balls.Contains(ball)) {
                    balls.Remove(ball);
                }
            }
        }

        private void FixedUpdate()
        {
            // check if player pressed hit button
            if (InputHandler.GetHitButton(playerNr)) {

                // get next available ball in list
                nextBall = GetNextBall();

                if (nextBall != null) {
                    Vector2 axes = InputHandler.GetLeftStickAxis(playerNr);
                    HitBall(nextBall, axes, axes.magnitude);
                }

            }
        }


        private void HitBall(Ball ball, Vector3 dir, float force)
        {
            // set last player on ball
            ball.SetLastPlayer(this);

            // apply punsh force
            force *= hitForce;

            // make force framerate independent (based on 50fps fixed timestamp)
            force *= 50 * Time.deltaTime;

            // apply force
            ball.GetRigidbody().isKinematic = false;
            ball.GetRigidbody().velocity = Vector3.zero;
            ball.GetRigidbody().AddForce(dir * force, ForceMode.Impulse);
        }

        private Ball GetNextBall()
        {
            for (int i = 0; i < balls.Count; i++) {
                if (balls[i].GetLastPlayer() != this) {
                    return balls[i];
                }
            }

            return null;
        }

        public Vector3 GetSpawnPoint() {

                return transform.position + new Vector3(
                   (Random.value - 0.5f) * transform.localScale.x * 0.75f,
                   (Random.value - 0.5f) * transform.localScale.y * 0.75f,
                   0f
                );
            
        }
    }
}

