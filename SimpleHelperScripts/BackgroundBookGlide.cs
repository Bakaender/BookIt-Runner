using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class BackgroundBookGlide : MonoBehaviour 
	{
        [Header("The parallax layer they are spawned on speed")]
        public float parallaxSpeed;

        public float runSpeed;
        public float fallSpeed;
        public float runDistance = 15f;

        private GameManager_Master gameManagerMaster;
        private float distanceRan;

        private void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.ResetToOriginEvent += ResetOrigin;
        }

        void OnDisable()
        {
            gameManagerMaster.ResetToOriginEvent -= ResetOrigin;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
        }

        private void Update()
        {
            if (distanceRan < runDistance)
            {
                gameObject.transform.position += new Vector3(runSpeed * Time.deltaTime, fallSpeed *gameObject.transform.localScale.y * Time.deltaTime, 0);
                distanceRan += Mathf.Abs(runSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ResetOrigin()
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - NewWorldGen.originResetDistance * parallaxSpeed, gameObject.transform.position.y, 0);
        }
    }
}

