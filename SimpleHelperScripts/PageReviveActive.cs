using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class PageReviveActive : MonoBehaviour 
	{
		void OnEnable()
		{
            //HARDCODED number of pages to revive.
            if (!GameManager_Master.canRevivePages || NewSaveGame.Instance.pages < 10)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
	}
}

