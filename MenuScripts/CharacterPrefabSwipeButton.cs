using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BookIt
{
    public class CharacterPrefabSwipeButton : MonoBehaviour
    {
        public GameObject prefabUICamera;
        public GameObject prefab1CameraPosition;
        public GameObject prefab2CameraPosition;
        public GameObject prefab3CameraPosition;

        private GameManager_Master gameManagerMaster;
        private bool isCameraMoving;
        public GameObject player;
        //public SpriteRenderer playerHatPosition;

        //public SpriteRenderer previewHat;

        private int usedBook;
        //private int usedHat;
        private string[] prefabSplit;

        private void Awake()
        {
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
        }

        private void Start()
        {
            UpdateSprites();

            isCameraMoving = false;
            if (NewSaveGame.Instance.usedPrefab == 0)
            {
                prefabUICamera.transform.position = prefab1CameraPosition.transform.position;
            }
            else if (NewSaveGame.Instance.usedPrefab == 1)
            {
                prefabUICamera.transform.position = prefab2CameraPosition.transform.position;
            }
            else
            {
                prefabUICamera.transform.position = prefab3CameraPosition.transform.position;
            }
        }

        public void UpdateSprites()
        {
            prefabSplit = NewSaveGame.Instance.prefabs[NewSaveGame.Instance.usedPrefab].Split(',');

            int.TryParse(prefabSplit[0], out usedBook);
            //int.TryParse(prefabSplit[1], out usedHat);

            player.GetComponent<Animator>().runtimeAnimatorController = CharacterDatabase.CharacterAnimations[usedBook];
            //playerHatPosition.sprite = CharacterDatabase.HatSprites[usedHat];

            //previewHat.sprite = CharacterDatabase.HatSprites[usedHat];
            BookSkinSelect.SelectedBookSkin = usedBook;
        }

        #region Swipe Input
        public void SwipeHandler(BaseEventData data)
        {
            HandleInput(data as PointerEventData);
        }

        private void HandleInput(PointerEventData pointerData)
        {
            if (!isCameraMoving)
            {
                if (Mathf.Abs(pointerData.delta.x) > Mathf.Abs(pointerData.delta.y))
                {
                    if (pointerData.delta.x > 25f)
                    {
                        if (NewSaveGame.Instance.usedPrefab > 0)
                        {
                            isCameraMoving = true;
                            if (NewSaveGame.Instance.usedPrefab == 1)
                            {
                                StartCoroutine(MoveCameraDecrease(prefab1CameraPosition.transform.position));
                            }
                            else if (NewSaveGame.Instance.usedPrefab == 2)
                            {
                                StartCoroutine(MoveCameraDecrease(prefab2CameraPosition.transform.position));
                            }
                            
                            NewSaveGame.Instance.usedPrefab--;
                            NewSaveGame.Instance.Save();
                        }
                    }
                    else if (pointerData.delta.x < -25f)
                    {
                        if (NewSaveGame.Instance.usedPrefab < 2)
                        {
                            isCameraMoving = true;
                            if (NewSaveGame.Instance.usedPrefab == 1)
                            {
                                StartCoroutine(MoveCameraIncrease(prefab3CameraPosition.transform.position));
                            }
                            else if (NewSaveGame.Instance.usedPrefab == 0)
                            {
                                StartCoroutine(MoveCameraIncrease(prefab2CameraPosition.transform.position));
                            }

                            NewSaveGame.Instance.usedPrefab++;
                            NewSaveGame.Instance.Save();
                        }
                    }
                }
            }   
        }
        #endregion

        private IEnumerator MoveCameraDecrease(Vector3 moveTo)
        {
            while (prefabUICamera.transform.position.x > moveTo.x)
            {
                //HARDCODED player preview camera swipe move speed.
                prefabUICamera.transform.position -= new Vector3(0.03f, 0, 0);
                yield return null;
            }

            UpdateSprites();
            prefabUICamera.transform.position = moveTo;
            isCameraMoving = false;
            gameManagerMaster.CallEnvetUpdateBookSkinsSelected();
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator MoveCameraIncrease(Vector3 moveTo)
        {
            while (prefabUICamera.transform.position.x < moveTo.x)
            {
                //HARDCODED player preview camera swipe move speed.
                prefabUICamera.transform.position += new Vector3(0.03f, 0, 0);
                yield return null;
            }

            UpdateSprites();
            prefabUICamera.transform.position = moveTo;
            isCameraMoving = false;
            gameManagerMaster.CallEnvetUpdateBookSkinsSelected();
            yield return new WaitForEndOfFrame();
        }
    }
}

