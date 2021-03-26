using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class SetResolution : MonoBehaviour 
	{
        public static int pixelWidth;
        public static int pixelHeight;

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

        private void Start()
        {
            pixelWidth = Camera.main.pixelWidth;
            pixelHeight = Camera.main.pixelHeight;

            if (NewSaveGame.Instance.resolution == 1)
            {
                Screen.SetResolution(pixelWidth / 2, pixelHeight / 2, true);
            }
        }
    }
}

