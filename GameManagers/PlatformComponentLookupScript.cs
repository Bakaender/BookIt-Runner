using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class PlatformComponentLookupScript : MonoBehaviour 
	{
        public int poolIndex;

        public BoxCollider2D platformCollider;
        public Transform easyPagePosition;
        public Transform mediumPagePosition;
        public Transform hardPagePosition;
        public Transform extremePagePosition;

        public PlatformRaiser[] platformRaiser;
        public GameObject premiumCurrency;
        public GameObject premiumDoubleCurrency;

        private void Awake()
        {
            platformCollider = GetComponent<BoxCollider2D>();
            easyPagePosition = gameObject.transform.Find("easyspawn");
            mediumPagePosition = gameObject.transform.Find("mediumspawn");
            hardPagePosition = gameObject.transform.Find("hardspawn");
            extremePagePosition = gameObject.transform.Find("extremespawn");

            platformRaiser = gameObject.transform.GetComponentsInChildren<PlatformRaiser>();
        }
    }
}

