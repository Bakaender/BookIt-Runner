using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class RandomBackgroundGlideSpawner : MonoBehaviour 
	{
        [Header("The parallax layer they are spawned on speed")]
        public float parallaxSpeed;

        public GameObject rightSpawnPoint;
        public GameObject leftSpawnPoint;

        public GameObject backgroundGlideBookPrefab;
        public float minTimeNextGlideSpawn = 9;
        public float maxTimeNextGlideSpawn = 14;
        public float glideBookMinScale = 0.7f;
        public float glideBookMaxScale = 1.2f;

        private GameObject player;
        private float nextGlideSpawnTime;
        private GameObject glideBookTempRef;

        private int spawnSide;
        private float nextScale;
        private Vector3 nextSpawnPoint;

        private void OnEnable()
        {
            SetInitialReferences();
        }

        void SetInitialReferences()
        {
            player = GameObject.FindGameObjectWithTag("Player");  
        }

        private void Start()
        {
            spawnSide = Random.Range(0, 2);

            if (spawnSide == 0)
            {
                nextSpawnPoint = new Vector3(leftSpawnPoint.transform.position.x, leftSpawnPoint.transform.position.y - Random.Range(0, 2), leftSpawnPoint.transform.position.z);
            }
            else
            {
                nextSpawnPoint = new Vector3(rightSpawnPoint.transform.position.x, rightSpawnPoint.transform.position.y - Random.Range(0, 2), rightSpawnPoint.transform.position.z);
            }

            nextScale = Random.Range(glideBookMinScale, glideBookMaxScale);

            nextGlideSpawnTime = Random.Range(minTimeNextGlideSpawn, maxTimeNextGlideSpawn);
        }

        private void Update()
        {
            gameObject.transform.position = new Vector3(player.transform.position.x * parallaxSpeed + 10.24f, gameObject.transform.position.y, gameObject.transform.position.z);

            if (Time.timeSinceLevelLoad > nextGlideSpawnTime)
            {              
                glideBookTempRef = Instantiate(backgroundGlideBookPrefab, nextSpawnPoint, Quaternion.identity);

                Animator spawnedAnim = glideBookTempRef.GetComponent<Animator>();
                spawnedAnim.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[Random.Range(0, System.Enum.GetNames(typeof(Characters)).Length)];
                spawnedAnim.Play(Animator.StringToHash("PlayerGlide"));

                glideBookTempRef.transform.localScale = new Vector3(-nextScale, nextScale, nextScale);

                if (spawnSide == 0)
                {
                    glideBookTempRef.GetComponent<BackgroundBookGlide>().runSpeed *= -1;
                    glideBookTempRef.transform.localScale = new Vector3(nextScale, nextScale, nextScale);
                }

                spawnSide = Random.Range(0, 2);

                if (spawnSide == 0)
                {
                    nextSpawnPoint = new Vector3(leftSpawnPoint.transform.position.x, leftSpawnPoint.transform.position.y - Random.Range(0, 2), leftSpawnPoint.transform.position.z);
                }
                else
                {
                    nextSpawnPoint = new Vector3(rightSpawnPoint.transform.position.x, rightSpawnPoint.transform.position.y - Random.Range(0, 2), rightSpawnPoint.transform.position.z);
                }

                nextScale = Random.Range(glideBookMinScale, glideBookMaxScale);

                nextGlideSpawnTime += Random.Range(minTimeNextGlideSpawn, maxTimeNextGlideSpawn);
            }
        }
    }
}

