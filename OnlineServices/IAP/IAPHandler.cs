using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Purchasing;
using DoozyUI;

namespace BookIt
{
    public class IAPHandler : MonoBehaviour
    {
        public GameObject StoreUIBlockingPanel;

        //private UIConfirmPopup UIPopup;

        private void Start()
        {
            //UIPopup = FindObjectOfType<UIConfirmPopup>();
        }

        public void Fulfill(Product product)
        {
            if (product != null)
            {
                switch (product.definition.id)
                {
                    //case "com.ancientechogames.bookit.200.pages":
                    //    Analytics.Transaction("com.ancientechogames.bookit.200.pages", 0.99m, "USD", null, null);

                    //    NewSaveGame.Instance.pages += 200;
                    //    NewSaveGame.Instance.Save();
                    //    //UIPopup.ShowPopup("200 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.", " Confirm ");
                    //    UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "200 Pages IAP", "200 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.");

                    //    break;

                    //case "com.ancientechogames.bookit.750.pages":
                    //    Analytics.Transaction("com.ancientechogames.bookit.750.pages", 2.99m, "USD", null, null);

                    //    NewSaveGame.Instance.pages += 750;
                    //    NewSaveGame.Instance.Save();
                    //    //UIPopup.ShowPopup("750 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.", " Confirm ");
                    //    UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "750 Pages IAP", "750 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.");


                    //    break;

                    //case "com.ancientechogames.bookit.1500.pages":
                    //    Analytics.Transaction("com.ancientechogames.bookit.1500.pages", 4.99m, "USD", null, null);

                    //    NewSaveGame.Instance.pages += 1500;
                    //    NewSaveGame.Instance.Save();
                    //    //UIPopup.ShowPopup("1500 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.", " Confirm ");
                    //    UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "1500 Pages IAP", "1500 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.");


                    //    break;

                    case "com.ancientechogames.bookit.15000.pages":
                        Analytics.Transaction("com.ancientechogames.bookit.15000.pages", 0.99m, "USD", null, null);

                        NewSaveGame.Instance.pages += 15000;
                        NewSaveGame.Instance.Save();
                        //UIPopup.ShowPopup("1500 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.", " Confirm ");
                        UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "15000 Pages IAP", "15000 Pages added, you now have " + NewSaveGame.Instance.pages + " pages.");


                        break;

                    default:
                        Debug.Log(string.Format("Unrecognized productId \"{0}\"", product.definition.id));
                        break;
                }
            }

            StoreUIBlockingPanel.SetActive(false);
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Analytics.CustomEvent("PurchaseFailed", new Dictionary<string, object>
                        {
                            { i.definition.id, p }
                        }
                        );

            if (p == PurchaseFailureReason.PurchasingUnavailable)
            {
                //UIPopup.ShowPopup("Error: Purchasing unavailable", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Purchasing unavailable");

            }
            else if (p == PurchaseFailureReason.UserCancelled)
            {
                //UIPopup.ShowPopup("Canceled by user", " Ok ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Canceled by use");
            }
            else if (p == PurchaseFailureReason.DuplicateTransaction)
            {
                //UIPopup.ShowPopup("Error: Duplicate transaction", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Duplicate transaction");
            }
            else if (p == PurchaseFailureReason.ExistingPurchasePending)
            {
                //UIPopup.ShowPopup("Error: Purchase pending", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Purchase pending");
            }
            else if (p == PurchaseFailureReason.PaymentDeclined)
            {
                //UIPopup.ShowPopup("Error: Payment declined", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Payment declined");
            }
            else if (p == PurchaseFailureReason.ProductUnavailable)
            {
                //UIPopup.ShowPopup("Error: Product unavailable", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Product unavailable");
            }
            else if (p == PurchaseFailureReason.SignatureInvalid)
            {
                //UIPopup.ShowPopup("Error: Signature invalid", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Signature invalid");
            }
            else if (p == PurchaseFailureReason.Unknown)
            {
                //UIPopup.ShowPopup("Error: Unknown", " Confirm ");
                UIManager.ShowNotification("PagesConfirmNotification", 0f, false, "Purchase failed", "Error: Unknown");
            }

            StoreUIBlockingPanel.SetActive(false);
        }
    }
}