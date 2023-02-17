/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class IAPManager : MonoBehaviour, IStoreListener
{
    // 구매 과정을 제어하는 함수 제공
    private IStoreController m_StoreController = null;

    // 여러 플랫폼을 위한 확장 처리를 제공
    private IExtensionProvider m_StoreExtensionProvider = null;

    private ITransactionHistoryExtensions m_TransactionHistoryExtensions = null;

    private bool m_PurchasInProgress;
    private string productIdInProcessing;

    string environment = "production";
    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);


            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }
        catch (Exception e)
        {
            // An error occurred during services initialization.
        }
    }

    public void Init()
    {
        // // If we haven't set up the Unity Purchasing reference
        // if (m_StoreController == null)
        // {
        //     // Begin to configure our connection to Purchasing
        //     InitializePurchasing();
        // }
    }
    public void InitializePurchasing()
    {
        if (IsInitailize)
            return;

        // Create a builder, first passing in a suite of Unity provided stores.
        var module = StandardPurchasingModule.Instance();
        Debug.Log("Before Builder Store Module" + module.appStore.ToString());
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);
        Debug.Log("After Builder Store Module" + module.appStore.ToString());

        // 상품 등록
        for (int i = 0; i < GameManager.Instance.Data.serializableInAppData.datas.Count; i++)
        {
            InAppData inAppData = GameManager.Instance.Data.serializableInAppData.datas[i];

            string ID = inAppData.inapp_product_id;
            ProductType productType = inAppData.product_type;
            string ISO_ID = inAppData.inapp_ios_store_id;
            string Android_ID = inAppData.inapp_google_store_id;
            Debug.Log("ID : " + ID);
            Debug.Log("ISO_ID : " + ISO_ID);
            Debug.Log("Android_ID : " + Android_ID);

            builder.AddProduct(id: inAppData.inapp_product_id, productType, new IDs()
            {
                { inAppData.inapp_ios_store_id, AppleAppStore.Name },
                { inAppData.inapp_google_store_id, GooglePlay.Name },
            });

            // bulider.AddProduct(id: inAppData.InApp_ID, ProductType.Consumable, new IDs()
            // {
            //     { GameManager.Instance.Data.InAppData[i].Product_IOS_Name, AppleAppStore.Name }, 
            //     { GameManager.Instance.Data.InAppData[i].Product_Android_Name, GooglePlay.Name },
            // });
        }

        UnityPurchasing.Initialize(this, builder);
    }


    public bool BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitailize)
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
                return true;
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                return false;
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
            return false;
        }
        return false;
    }

    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitailize)
        {
            Debug.Log("RestorePurchases FAIL.Not initialized.");
            return;
        }
        // If we are running on an Apple device ...
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.OSXEditor)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started...");
            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                if (!result)
                {
                    Debug.Log("Resrore Failed or No Purchas Available Item");
                }
                else
                {

                }
            });
        }
        // Otherwise ...
        else
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("RestorePurchases started...");
*//*
                List<InAppData> restoreInAppDatas = GameManager.Instance.Data.serializableInAppData.datas.FindAll(x => x.product_type == ProductType.NonConsumable);
                for (int i = 0; i < restoreInAppDatas.Count; i++)
                {
                    InAppData restoreInAppData = restoreInAppDatas[i];
                    string inapp_product_id = restoreInAppData.inapp_product_id;
                    if (string.IsNullOrEmpty(inapp_product_id)) continue;

                    // 유저 구매 기록 확인
                    UserDetailStoreDataBase userPurchaseData = GameManager.Instance.UserData.m_UserStoreData.GetInAppPurchaseData(restoreInAppData);
                    if (userPurchaseData == null) continue;

                    // 영수증 검증
                    if (HadPurchased(inapp_product_id) == false) continue;

                    // 아이템 보유 확인
                    List<RewardItemData> restoreItemDatas = new List<RewardItemData>();
                    for (int k = 0; k < restoreInAppData.target_item_ids.Length; k++)
                    {
                        int target_item_id = restoreInAppData.target_item_ids[k];
                        double target_item_count = restoreInAppData.target_item_counts[k];
                        UserDetailItemDataBase userDetailItemDataBase = GameManager.Instance.ItemManager.GetUserDetailItemData(target_item_id);
                        if (userDetailItemDataBase == null)
                        {
                            RewardItemData prevData = restoreItemDatas.Find(x => x.ItemData.id == target_item_id);
                            if (prevData != null)
                            {
                                ItemDataBase itemDataBase = GameManager.Instance.ItemManager.GetItemData(target_item_id);
                                RewardItemData newData = new RewardItemData(itemDataBase, target_item_count);
                                restoreItemDatas.Add(newData);
                            }
                            else
                            {
                                prevData.Count += target_item_count;
                            }
                        }
                    }
                    GameManager.Instance.ItemManager.AddItem(restoreItemDatas.ToArray());
                }*//*
            }
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL.Not supported on this platform.Current = " + Application.platform);
            }
        }
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        foreach (var item in controller.products.all)
        {
            Debug.Log(string.Join(" - ", new[]
            {
                item.transactionID,
                item.metadata.localizedTitle,
                item.metadata.localizedDescription,
                item.metadata.isoCurrencyCode,
                item.metadata.localizedPrice.ToString(),
                item.metadata.localizedPriceString,
                item.transactionID,
                item.definition.type.ToString(),
                item.receipt
            }
            ));
        }

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        m_TransactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is Your App Correctly UpLoaded On The Relevant Publisher Console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                Debug.Log("no Products available for purchase!");
                break;
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        // 뒤끝 영수증 검증 처리
        if (Application.platform == RuntimePlatform.Android)
        {
            *//*
            BackendReturnObject validation = Backend.Receipt.IsValidateGooglePurchase(args.purchasedProduct.receipt, "receiptDescription", false);
            // 영수증 검증에 성공한 경우
            if (validation.IsSuccess())
            {
                HandlePurchaseSuccess(args);
            }
            // 영수증 검증에 실패한 경우 
            else
            {
                // Or ... an unknown product has been purchased by this user. Fill in additional products here....
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }*//*

            HandlePurchaseSuccess(args);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            *//*
            BackendReturnObject validation = Backend.Receipt.IsValidateApplePurchase(args.purchasedProduct.receipt, "receiptDescription");
            // 영수증 검증에 성공한 경우
            if (validation.IsSuccess())
            {
                HandlePurchaseSuccess(args);
            }
            // 영수증 검증에 실패한 경우 
            else
            {
                // Or ... an unknown product has been purchased by this user. Fill in additional products here....
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }*//*
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Debug.isDebugBuild)
                HandlePurchaseSuccess(args);
        }

        Debug.Log(Application.platform);

        return PurchaseProcessingResult.Complete;
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning("Purchase Failed " + product.definition.id + failureReason);
        Debug.Log("Store Specific eroor code : " + m_TransactionHistoryExtensions.GetLastStoreSpecificPurchaseErrorCode());


        if (m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription() != null)
        {
            Debug.Log("Purchase failure description message : " + m_TransactionHistoryExtensions.GetLastPurchaseFailureDescription().message);
        }

        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

        InAppData inAppData = GameManager.Instance.Data.serializableInAppData.datas.Find(x => x.inapp_product_id.Equals(product.definition.id));
        if (inAppData == null)
            return;

        // UIConfirmPopup _UIConfirmPopup = GameManager.Instance.UIManager.ShowPopupUI<UIConfirmPopup>();
        // if (_UIConfirmPopup != null)
        // {
        //     string strExplain = string.Format("{0} 상품의 구매에 실패하였습니다.\n{1}", 
        //         GameManager.Instance.LanguageMgr.GetLocalizeText(inAppData.textcode_name), failureReason);
        //     _UIConfirmPopup
        //         .ShowPopup("구매 실패", strExplain)
        //         .ShowConfirmButton(() => { _UIConfirmPopup.ClosePopup(); });
        // }
    }


    public bool IsInitailize => m_StoreController != null && m_StoreExtensionProvider != null;



    public string GetLocalizedDescription(string ItemID)
    {
        if (!IsInitailize)
            return null;

        var _Product = m_StoreController.products.WithID(ItemID);

        if (_Product != null)
        {
            return _Product.metadata.localizedPriceString;
        }
        return null;
    }

    public bool HadPurchased(string productId)
    {
        if (!IsInitailize)
            return false;

        var product = m_StoreController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }


    public void HandlePurchaseSuccess(PurchaseEventArgs args)
    {
        if (args == null)
            return;

        List<InAppData> inAppDatas = GameManager.Instance.Data.serializableInAppData.datas;
        for (int i = 0; i < inAppDatas.Count; i++)
        {
            InAppData inAppData = inAppDatas[i];
            if (String.Equals(args.purchasedProduct.definition.id, inAppData.inapp_product_id, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                if (!Debug.isDebugBuild)
                {
                    UnityEngine.Analytics.Analytics.CustomEvent(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                }

                // List<RewardItemData> rewardItemDatas = new List<RewardItemData>();
                // for (int k = 0; k < inAppData.target_item_ids.Length; k++)
                // {
                //     int target_item_id = inAppData.target_item_ids[k];
                //     double target_item_count = inAppData.target_item_counts[k];
                //     ItemDataBase itemDataBase = GameManager.Instance.ItemManager.GetItemData(target_item_id);
                //     if (itemDataBase == null) continue;
                //     // rewardItemDatas.Add(new RewardItemData(itemDataBase, target_item_count));
                // }
                // if (GameManager.Instance.UserData.m_UserStoreData.PurchaseInAppItem(inAppData) != null)
                // {
                //     GameManager.Instance.ItemManager.AddItem(rewardItemDatas.ToArray());
                //     UIReceivedRewardPopup _UIReceivedRewardPopup = GameManager.Instance.UIManager.ShowPopupUI<UIReceivedRewardPopup>();
                //     if (_UIReceivedRewardPopup != null)
                //     {
                //         _UIReceivedRewardPopup.Setup(rewardItemDatas.ToArray());
                //     }
                // }
                // GameManager.Instance.UILobby.OpenPresentPopup()
                // GameManager.Instance.LobbyGame.UI.PopUpUI.Packages.PurchasedPackage(ProductCherryBlossomPackage);
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                break;
            }
        }
    }

}
*/