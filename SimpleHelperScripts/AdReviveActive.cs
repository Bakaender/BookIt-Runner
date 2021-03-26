using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class AdReviveActive : MonoBehaviour 
	{
		void OnEnable()
		{
            if (!GameManager_Master.canRevive)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
		}
	}
}

