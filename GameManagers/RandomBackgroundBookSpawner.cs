using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class RandomBackgroundBookSpawner : MonoBehaviour 
	{
        public GameObject backgroundBookPrefab;
        public float minTimeNextSpawn = 10;
        public float maxTimeNextSpawn = 20;

        private GameObject player;
        private Animator _animator;
        private float nextSpawnTime;

        private void OnEnable()
        {
            SetInitialReferences();
        }

        void SetInitialReferences()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            _animator = backgroundBookPrefab.GetComponent<Animator>();

            nextSpawnTime = Random.Range(minTimeNextSpawn, maxTimeNextSpawn);
        }

        private void Update()
        {
            gameObject.transform.position = new Vector3(player.transform.position.x + 10.24f, gameObject.transform.position.y, gameObject.transform.position.z);

            if (Time.timeSinceLevelLoad > nextSpawnTime)
            {
                _animator.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[Random.Range(0, System.Enum.GetNames(typeof(Characters)).Length)];

                Instantiate(backgroundBookPrefab, gameObject.transform.position, Quaternion.identity);

                nextSpawnTime += Random.Range(minTimeNextSpawn, maxTimeNextSpawn);
            }
        }
    }
}

