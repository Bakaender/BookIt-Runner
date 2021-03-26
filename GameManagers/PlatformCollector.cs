using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
	public class PlatformCollector : MonoBehaviour 
	{
        private NewWorldGen newWorldGenScript;

        private void Awake()
        {
            newWorldGenScript = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<NewWorldGen>();
            if (newWorldGenScript == null)
            {
                Debug.LogError("NewWorldGen not found in PlatformCollector script Awake. Platforms won't be removed properly");
            }
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            switch (GameManager_Master.gameState)
            {
                case GameState.Menu:
                    if (target.tag == "AIPlatform")
                    {
                        //HARDCODED 39.75f is hardcoded. Will have to change if initial platform layout changes.
                        //I could instead combine everything as 1 and use a big collider size instead of hardcoding. Probably.
                        target.transform.position = new Vector3(target.transform.position.x + 39.75f, target.transform.position.y, 0);
                        newWorldGenScript.lastManuallyPlacedObject = target.gameObject;
                    }

                    break;

                case GameState.Tutorial:
                    if (target.tag == "AIPlatform")
                    {
                        //HARDCODED 39.75f is hardcoded. Will have to change if initial platform layout changes.
                        //I could instead combine everything as 1 and use a big collider size instead of hardcoding. Probably.
                        target.transform.position = new Vector3(target.transform.position.x + 39.75f, target.transform.position.y, 0);
                        newWorldGenScript.lastManuallyPlacedObject = target.gameObject;
                    }

                    break;

                case GameState.Playing:
                    if (target.tag == "Platform" || target.tag == "MovingPlatform")
                    {
                        if (newWorldGenScript != null)
                        {
                            newWorldGenScript.IncreaseDifficulty();
                            newWorldGenScript.RemoveObjectFromObjectList(target.transform.root.gameObject);
                            newWorldGenScript.numberOfObjectsToSpawn++;
                        }
                        else
                            Debug.LogError("newWorldGen not found in PlatformCollector script.");
                    }
                    else if (target.tag == "AIPlatform")
                    {
                        if (newWorldGenScript != null)
                        {
                            newWorldGenScript.RemoveObjectFromObjectList(target.transform.gameObject);
                        }
                        else
                            Debug.LogError("newWorldGen not found in PlatformCollector script.");
                    }
                    //??? Could just destroy anything else because physics settings it won't destroy backgrounds.
                    else if (target.tag == "SuperJump")
                    {
                        Destroy(target.gameObject);
                    }
                    else if (target.tag == "Currency")
                    {
                        Destroy(target.gameObject);
                    }

                    break;

                case GameState.Paused:
                    break;

                case GameState.GameOver:
                    break;

                default:
                    break;
            }  
        }
    }
}

