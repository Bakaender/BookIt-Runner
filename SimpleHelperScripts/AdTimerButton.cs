using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using DoozyUI;

namespace BookIt
{
    public class AdTimerButton : MonoBehaviour
    {
        public int numberOfPagesReward = 100;
        public int hoursBetweenAds = 3;
        public Text timerText;

        //private GameManager_Master gameManagerMaster;

        private string playerPrefsString = "PagesForAdTime";
        private DateTime _lastAdTime;

        private TimeSpan timeDif;

        private void Awake()
        {
            //gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();

            if (PlayerPrefs.HasKey(playerPrefsString))
            {
                _lastAdTime = DateTime.Parse(PlayerPrefs.GetString(playerPrefsString));
            }
            else
            {
                _lastAdTime = DateTime.MinValue;
            }
        }

        private void Update()
        {
            if (DateTime.Now < _lastAdTime.AddHours(hoursBetweenAds))
            {
                timeDif = _lastAdTime.AddHours(hoursBetweenAds) - DateTime.Now;
                //timerText.text = timeDif.Hours.ToString() + ":" + timeDif.Minutes.ToString() + ":" + timeDif.Seconds.ToString();
                timerText.text = string.Format("{0:0}:{1:00}:{2:00}", timeDif.Hours, timeDif.Minutes, timeDif.Seconds);
            }
            else
            {
                timerText.text = "0:00:00";
            }
        }

        public void ShowAdForPages()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (DateTime.Now >= _lastAdTime.AddHours(hoursBetweenAds))
            {
                if (Advertisement.IsReady("rewardedVideo"))
                {
                    var options = new ShowOptions();
                    options.resultCallback = HandleShowResult;

                    Advertisement.Show("rewardedVideo", options);
                }
                else
                {
                    NewSaveGame.Instance.pages += numberOfPagesReward;
                    NewSaveGame.Instance.Save();

                    _lastAdTime = DateTime.Now;
                    PlayerPrefs.SetString(playerPrefsString, DateTime.Now.ToString());
                    PlayerPrefs.Save();

                    //gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("You got 50 pages!", " Confirm ");
                    UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Pages for ad view", "You got " + numberOfPagesReward + " pages!");
                }
            }
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        void HandleShowResult(ShowResult result)
        {
            if (result == ShowResult.Finished)
            {
                NewSaveGame.Instance.pages += numberOfPagesReward;
                NewSaveGame.Instance.Save();

                _lastAdTime = DateTime.Now;
                PlayerPrefs.SetString(playerPrefsString, DateTime.Now.ToString());
                PlayerPrefs.Save();

                //gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("You got 50 pages!", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Pages for ad view", "You got " + numberOfPagesReward + " pages!");
            }
            else if (result == ShowResult.Skipped)
            {
                //gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("Ad skipped", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Pages for ad view", "Ad Skipped");
            }
            else if (result == ShowResult.Failed)
            {
                //gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("Ad failed", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Pages for ad view", "Ad Failed");
            }
        }
#endif
    }
}

