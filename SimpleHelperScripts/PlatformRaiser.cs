using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class PlatformRaiser : MonoBehaviour 
	{
        public GameObject platformToRaise;
        public Vector3 moveDistance;
        public float moveTimeSeconds;

        private float movedTime;
        private Vector3 adjustedMoveDistance;
        private bool isMoving;

        private AudioSource _audioSource;

        private GameManager_Master gameManagerMaster;

        private Vector3 distanceMoved;

        private void OnEnable()
        {
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
            _audioSource = platformToRaise.GetComponent<AudioSource>();

            gameManagerMaster.ResetMovingPlatformsEvent += ResetMovingPlatform;
        }

        private void OnDisable()
        {
            ResetMovingPlatform();

            gameManagerMaster.ResetMovingPlatformsEvent -= ResetMovingPlatform;
        }

        public void ResetMovingPlatform()
        {
            platformToRaise.transform.position -= distanceMoved;
            distanceMoved = Vector3.zero;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                StartCoroutine(MovePlatform());
            }
        }

        private IEnumerator MovePlatform()
        {
            if (_audioSource != null)
            {
                _audioSource.Play();
            }
            
            adjustedMoveDistance = new Vector2(moveDistance.x / moveTimeSeconds, moveDistance.y / moveTimeSeconds);
            isMoving = true;
            movedTime = 0;
            
            while (isMoving)
            {
                movedTime += Time.deltaTime;
                if (movedTime >= moveTimeSeconds)
                {
                    isMoving = false;
                    break;
                }

                platformToRaise.transform.position += adjustedMoveDistance * Time.deltaTime;

                distanceMoved += adjustedMoveDistance * Time.deltaTime;

                yield return new WaitForFixedUpdate();
            }

            if (_audioSource != null)
            {
                _audioSource.Stop();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}