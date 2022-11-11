using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if BBG_IAP
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension; 
#endif

namespace OSK
{
	public class IAPTestScene : MonoBehaviour
	{
		#region Inspector Variables

		public IAPTestButton	testButtonPrefab;
		public Transform		testButtonContainer;

		#endregion

		#region Member Variables

		private ObjectPool testButtonPool = null;

		#endregion

		#region Unity Methods

		private void Start()
		{
			if (IAPSettings.IsIAPEnabled)
			{
				testButtonPool = new ObjectPool(testButtonPrefab.gameObject, 1, testButtonContainer);

				if (IAPManager.Instance.IsInitialized)
				{
					SetupButtons();
				}
				else
				{
					IAPManager.Instance.OnInitializedSuccessfully += OnIAPInitialized;
				}

				IAPManager.Instance.OnProductPurchased += OnProductPurchased;
			}
			else
			{
				CustomDebug.Log("IAPTestScene", "IAP is not enabled. Please open the IAP Settings window in Unity and enable IAP.");
			}
		}

		#endregion

		#region Private Methods

		private void OnIAPInitialized()
		{
			SetupButtons();
		}

		private void SetupButtons()
		{
			testButtonPool.ReturnAllObjectsToPool();

			for (int i = 0; i < IAPSettings.Instance.productInfos.Count; i++)
			{
				IAPSettings.ProductInfo	productInfo	= IAPSettings.Instance.productInfos[i];
				IAPTestButton			testButton	= testButtonPool.GetObject<IAPTestButton>();

				SetupTestButton(productInfo.productId, testButton);
			}
		}

		private void SetupTestButton(string productId, IAPTestButton testButton)
		{
			testButton.idText.text = productId;

			if (IAPSettings.IsIAPEnabled)
			{
				#if BBG_IAP

				Product product = IAPManager.Instance.GetProductInformation(productId);

				if (product == null)
				{
					SetErrorText(testButton, "Product does not exist");
				}
				else
				{
					if (!product.availableToPurchase)
					{
						SetErrorText(testButton, "Product is not available to purchase");
					}
					else
					{
						testButton.errorText.gameObject.SetActive(false);
					}

					testButton.nameText.gameObject.SetActive(true);
					testButton.descText.gameObject.SetActive(true);
					testButton.priceText.gameObject.SetActive(true);
					testButton.consumableText.gameObject.SetActive(true);

					testButton.nameText.text		= "Title: " + product.metadata.localizedTitle;
					testButton.descText.text		= "Description: " + product.metadata.localizedDescription;
					testButton.priceText.text		= "Price: " + product.metadata.localizedPriceString;
					testButton.consumableText.text	= "Type: " + product.definition.type + (product.definition.type != ProductType.Consumable ? " - Purchased: " + IAPManager.Instance.IsProductPurchased(productId) : "");

					testButton.Data					= productId;
					testButton.OnListItemClicked	= OnButtonClicked;
				}

				#endif
			}
			else
			{
				SetErrorText(testButton, "IAP is not enabled");
			}
		}

		private void SetErrorText(IAPTestButton testButton, string message)
		{
			testButton.errorText.text = message;

			testButton.errorText.gameObject.SetActive(true);
			testButton.nameText.gameObject.SetActive(false);
			testButton.descText.gameObject.SetActive(false);
			testButton.priceText.gameObject.SetActive(false);
			testButton.consumableText.gameObject.SetActive(false);

			testButton.OnListItemClicked = null;
		}

		private void OnButtonClicked(int index, object data)
		{
			#if BBG_IAP

			IAPManager.Instance.BuyProduct(data as string);

			#endif
		}

		private void OnProductPurchased(string productId)
		{
			// Refresh the buttons
			SetupButtons();
		}

		#endregion
	}
}
