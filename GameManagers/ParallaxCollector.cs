using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class ParallaxCollector : MonoBehaviour 
	{
        public string tagToCollect;

        private RandomBackgroundBookSpawner bookSpawner;

        private void OnEnable()
        {
            bookSpawner = FindObjectOfType<RandomBackgroundBookSpawner>();
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            //only works on tiled same sized objects. 
            //HARDCODED 3 is also hardcoded.
            if (target.CompareTag(tagToCollect))
            {
                target.transform.position = new Vector3(target.transform.position.x + target.GetComponent<Collider2D>().bounds.size.x * 3, target.transform.position.y, 0);
                bookSpawner.transform.position += new Vector3(target.GetComponent<Collider2D>().bounds.size.x, 0, 0);
            }
        }
    }
}

