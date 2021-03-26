using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class DontDestroyOnLoad : MonoBehaviour 
	{
        static bool created = false;

        private void Awake()
        {
            if (!created)
            {
                DontDestroyOnLoad(gameObject);
                created = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}
}

