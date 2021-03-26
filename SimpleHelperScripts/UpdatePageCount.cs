using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class UpdatePageCount : MonoBehaviour 
	{
        private Text pagesText;

        private void Start()
        {
            pagesText = gameObject.GetComponent<Text>();
        }

        private void Update()
        {
            pagesText.text = NewSaveGame.Instance.pages.ToString();
        }
    }
}

