using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class PagesNumber : MonoBehaviour 
	{
		void Update() 
		{
            gameObject.GetComponent<Text>().text = NewSaveGame.Instance.pages.ToString();
		}
	}
}

