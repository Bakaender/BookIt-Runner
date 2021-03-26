using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BookIt
{
	public class BookSkinSelect : MonoBehaviour 
	{
        private AchievementReporter achievReporter;

        public static int SelectedBookSkin;
        public static bool confirmPurchase = false;
        public static bool cancelPurchase = false;
        private static Color SelectedColorStatic;

        [Header("Only color set on Default book will change this color for all.")]
        public Color SelectedColor;

        public Characters Name;
        public int UnlockCost;
        public Image LockedImage;      
        public Text CostText;
        //public Image PageImage;
        public GameObject ConfirmationPanel;
        public GameObject InsufficientPagesPanel;

        private GameManager_Master gameManagerMaster;
        private UIPrefabInit UIPrefabs;
        private GameObject player;

        private bool owned;

        private int usedBook;
        private int usedOther;
        private string[] prefabSplit;

        private void OnEnable()
        {
            if (Name == Characters.Default)
            {
                SelectedColorStatic = SelectedColor;
            }

            SetInitialReferences();
            gameManagerMaster.UpdateBookSkinsSelectedEvent += Selected;
            gameManagerMaster.UpdateBookSkinsSelectedEvent += PrefabChanged;
        }

        private void OnDisable()
        {
            gameManagerMaster.UpdateBookSkinsSelectedEvent -= Selected;
            gameManagerMaster.UpdateBookSkinsSelectedEvent -= PrefabChanged;

            //So if they select a locked book it changes preview back when closed.
            UIPrefabs.UpdatePrefabBook(NewSaveGame.Instance.usedPrefab, usedBook);

            //This hides the panel if the main bookshelf canvas is disabled before Couroutine can finish and hide it.
            InsufficientPagesPanel.SetActive(false);
        }

        private void SetInitialReferences()
        {
            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
            UIPrefabs = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIPrefabInit>();
            player = GameObject.FindGameObjectWithTag("Player");
            achievReporter = FindObjectOfType<AchievementReporter>();

            owned = false;

            //If we own the skin already disable locked image.
            for (int i = 0; i < NewSaveGame.Instance.ownedCharacters.Count; i++)
            {
                if ((int)Name == NewSaveGame.Instance.ownedCharacters[i])
                {
                    LockedImage.gameObject.SetActive(false);
                    owned = true;

                    CostText.text = "Owned";
                    CostText.color = Color.black;
                }
            }

            if (!owned)
            {
                CostText.text = UnlockCost.ToString();
                if (NewSaveGame.Instance.pages >= UnlockCost)
                {
                    CostText.color = Color.black;
                }
                else
                {
                    CostText.color = Color.red;
                }
            }

            prefabSplit = NewSaveGame.Instance.prefabs[NewSaveGame.Instance.usedPrefab].Split(',');
            int.TryParse(prefabSplit[0], out usedBook);
            int.TryParse(prefabSplit[1], out usedOther);

            BookSkinSelect.SelectedBookSkin = usedBook;

            if ((int)Name == SelectedBookSkin)
            {
                gameObject.GetComponent<Image>().color = SelectedColorStatic;
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }    
        }

        public void BookSkinClicked()
        {
            if (owned)
            {
                OwnedUpdate();
            }
            else
            {
                if (BookSkinSelect.SelectedBookSkin == (int)Name)
                {
                    //Ask if want to unlock.
                    if (NewSaveGame.Instance.pages >= UnlockCost)
                    {
                        ConfirmationPanel.GetComponentInChildren<Text>().text = "Spend " + UnlockCost + " pages to unlock " + Name + "?";
                        ConfirmationPanel.SetActive(true);

                        StartCoroutine(WaitForUnlockConfirm());
                    }
                    else
                    {
                        InsufficientPagesPanel.GetComponentInChildren<Text>().text = "Insufficient pages to unlock " + Name +
                                                                                     ". You need " + UnlockCost + " Pages.";
                        InsufficientPagesPanel.SetActive(true);

                        StartCoroutine(HideInsufficientPagesPanel());
                    }
                }
                else
                {
                    BookSkinSelect.SelectedBookSkin = (int)Name;
                    gameManagerMaster.CallEnvetUpdateBookSkinsSelected();

                    UIPrefabs.UpdatePrefabBook(NewSaveGame.Instance.usedPrefab, (int)Name);
                } 
            }
        }

        private void Selected()
        {
            if ((int)Name == SelectedBookSkin)
            {
                gameObject.GetComponent<Image>().color = SelectedColorStatic;
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }
        }

        private void PrefabChanged()
        {
            prefabSplit = NewSaveGame.Instance.prefabs[NewSaveGame.Instance.usedPrefab].Split(',');
            int.TryParse(prefabSplit[0], out usedBook);
            int.TryParse(prefabSplit[1], out usedOther);
        }

        private void OwnedUpdate()
        {
            BookSkinSelect.SelectedBookSkin = (int)Name;
            gameManagerMaster.CallEnvetUpdateBookSkinsSelected();
            UIPrefabs.UpdatePrefabBook(NewSaveGame.Instance.usedPrefab, (int)Name);

            //Updates the player skin
            UpdateSprites();

            string prefabSave = "";
            prefabSave += (int)Name;
            prefabSave += ",";
            prefabSave += usedOther.ToString();
            NewSaveGame.Instance.prefabs[NewSaveGame.Instance.usedPrefab] = prefabSave;
            NewSaveGame.Instance.Save();

            //Using event to update usedBook so that all instances update and set the prefab back to correct on OnDisable.
            gameManagerMaster.CallEnvetUpdateBookSkinsSelected();
        }

        private void UpdateSprites()
        {
            player.GetComponent<Animator>().runtimeAnimatorController = CharacterDatabase.CharacterAnimations[(int)Name];
        }

        private IEnumerator HideInsufficientPagesPanel()
        {
            yield return new WaitForSeconds(3f);

            InsufficientPagesPanel.SetActive(false);
        }

        private IEnumerator WaitForUnlockConfirm()
        {
            while (!confirmPurchase && !cancelPurchase)
            {
                yield return null;
            }

            yield return new WaitForFixedUpdate();
            FinishUnlock();
        }

        void FinishUnlock()
        {
            if (confirmPurchase)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonControllerMaster>().UIBookUnlockSound();
                NewSaveGame.Instance.pages -= UnlockCost;
                NewSaveGame.Instance.AddCharacter((int)Name);
                ConfirmationPanel.SetActive(false);
                confirmPurchase = false;
                LockedImage.gameObject.SetActive(false);
                owned = true;
                OwnedUpdate();
                CostText.text = "Owned";

#if UNITY_ANDROID
                achievReporter.ReportSkinsAchievementAndroid();
#elif UNITY_IOS
                achievReporter.ReportSkinsAchievementIos();
#endif
            }
            else if (cancelPurchase)
            {
                ConfirmationPanel.SetActive(false);
                cancelPurchase = false;
            }
        }
    }
}

