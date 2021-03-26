using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class AddAIPlatformsToList : MonoBehaviour 
	{
        private void Start()
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<NewWorldGen>().AddObjectToPlatformList(gameObject);
        }
    }
}