using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

#if BBG_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension; 
#endif


namespace OSK
{
    public class IAPManager : SingletonMono<IAPManager>
#if BBG_IAP
	, IStoreListener
#endif
    {
        #region Classes

        [System.Serializable]
        public class OnProductPurchasedEvent : UnityEngine.Events.UnityEvent { }

        [System.Serializable]
        public class PurchaseEvent
        {
            public string productId = "";
            public OnProductPurchasedEvent onProductPurchasedEvent = null;
        }

        #endregion

        #region Inspector Variables

        public List<OnProductPurchasedEvent> purchaseEvents = null;

        #endregion

        #region Member Variables

        private const string LogTag = "IAPManager";

#if BBG_IAP
		private IStoreController	storeController;
		private IExtensionProvider 	extensionProvider;
#endif

        private HashSet<string> purchasedNonConsumables;

        #endregion

        #region Properties

        /// <summary>
        /// Save data id
        /// </summary>
        public string SaveId { get { return "iap_manager"; } }

        /// <summary>
        /// Callback that is invoked when the IAPManager has successfully initialized and has retrieved the list of products/prices
        /// </summary>
        public System.Action OnInitializedSuccessfully { get; set; }

        /// <summary>
        /// Callback that is invoked when a product is purchased, passes the product id that was purchased
        /// </summary>
        public System.Action<string> OnProductPurchased { get; set; }

        /// <summary>
        /// Returns true if IAP is initialized
        /// </summary>
        public bool IsInitialized
        {
#if BBG_IAP
			get { return storeController != null && extensionProvider != null; }
#else
            get { return false; }
#endif
        }

        #endregion

        #region Unity Methods

        protected void Awake()
        {
            purchasedNonConsumables = new HashSet<string>();
        }

        private void Start()
        {
#if BBG_IAP

			Logger.Log(LogTag, "Start");

			// Initialize IAP
			ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			// Add all the product ids to teh builder
			for (int i = 0; i < IAPSettings.Instance.productInfos.Count; i++)
			{
				IAPSettings.ProductInfo productInfo = IAPSettings.Instance.productInfos[i];

				Logger.Log(LogTag, "Adding product to builder, id: " + productInfo.productId + ", consumable: " + productInfo.consumable);

				builder.AddProduct(productInfo.productId, productInfo.consumable ? ProductType.Consumable : ProductType.NonConsumable);
			}

			Logger.Log(LogTag, "Initializing IAP now...");

			UnityPurchasing.Initialize(this, builder);

#endif
        }

        #endregion

        #region Public Methods

#if BBG_IAP

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			Logger.Log(LogTag, "Initialization successful!");

			storeController		= controller;
			extensionProvider	= extensions;

			if (OnInitializedSuccessfully != null)
			{
				OnInitializedSuccessfully();
			}
		}

		public void OnInitializeFailed(InitializationFailureReason failureReason)
		{
			Logger.LogError(LogTag, "Initializion failed! Reason: " + failureReason);
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Logger.LogError(LogTag, "Purchase failed for product id: " + product.definition.id + ", reason: " + failureReason);
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			Product product = args.purchasedProduct;

			Logger.Log(LogTag, "Purchase successful for product id: " + product.definition.id);

			if (product.definition.type != ProductType.Consumable)
			{
				purchasedNonConsumables.Add(product.definition.id);
			}

			if (OnProductPurchased != null)
			{
				OnProductPurchased(product.definition.id);
			}

			for (int i = 0; i < IAPSettings.Instance.productInfos.Count; i++)
			{
				if (IAPSettings.Instance.productInfos[i].productId == product.definition.id)
                {
                    OnProductPurchasedEvent purchaseEvent = purchaseEvents[i];
                    if (purchaseEvent != null)
                        purchaseEvent.Invoke();
				}
			}

			return PurchaseProcessingResult.Complete;
		}

		/// <summary>
		/// Starts the buying process for the given product id
		/// </summary>
		public void BuyProduct(string productId)
		{
			Logger.Log(LogTag, "BuyProduct: Purchasing product with id: " + productId);

			if (IsInitialized)
			{
				Product product = storeController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product == null)
				{
					Logger.LogError(LogTag, "BuyProduct: product with id \"" + productId + "\" does not exist.");
				}
				else if (!product.availableToPurchase)
				{
					Logger.LogError(LogTag, "BuyProduct: product with id \"" + productId + "\" is not available to purchase.");
				}
				else
				{
					storeController.InitiatePurchase(product);
				}
			}
			else
			{
				Logger.LogWarning(LogTag, "BuyProduct: IAPManager not initialized.");
			}
		}

		/// <summary>
		/// Gets the products store information
		/// </summary>
		public Product GetProductInformation(string productId)
		{
			if (IsInitialized)
			{
				return storeController.products.WithID(productId);
			}

			return null;
		}

#endif

        /// <summary>
        /// Returns true if the given product id has been purchased, only for non-consumable products, consumable products will always return false.
        /// </summary>
        public bool IsProductPurchased(string productId)
        {
            return purchasedNonConsumables.Contains(productId);
        }

        /// <summary>
        /// Restores the purchases if platform is iOS or OSX
        /// </summary>
        public void RestorePurchases()
        {
            CustomDebug.Log(LogTag, "RestorePurchases: Restoring purchases");

#if BBG_IAP
			if (IsInitialized)
			{
				if ((Application.platform == RuntimePlatform.IPhonePlayer ||
				     Application.platform == RuntimePlatform.OSXPlayer))
				{
					extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result) => {});
				}
				else
				{
					Logger.LogWarning(LogTag, "RestorePurchases: Device is not iOS, no need to call this method.");
				}
			}
			else
			{
				Logger.LogWarning(LogTag, "RestorePurchases: IAPManager not initialized.");
			}
#endif
        }

        #endregion
    }
}