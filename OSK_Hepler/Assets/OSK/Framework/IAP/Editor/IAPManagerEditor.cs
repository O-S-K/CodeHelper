using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OSK
{
	[CustomEditor(typeof(IAPManager))]
	public class IAPManagerEditor : Editor
	{
		#region Member Variables

		private Texture2D lineTexture;

		#endregion

		#region Properties

		private Texture2D LineTexture
		{
			get
			{
				if (lineTexture == null)
				{
					lineTexture = new Texture2D(1, 1);
					lineTexture.SetPixel(0, 0, new Color(37f/255f, 37f/255f, 37f/255f));
					lineTexture.Apply();
				}

				return lineTexture;
			}
		}

		#endregion

		#region Unity Methods

		private void OnDisable()
		{
			DestroyImmediate(LineTexture);
		}

		#endregion

		#region Public Methods

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			if (!IAPSettings.IsIAPEnabled)
			{
				EditorGUILayout.HelpBox("IAP is not enabled, please open the IAP Settings window and enable IAP.", MessageType.Warning);
			}

			if (GUILayout.Button("Open IAP Settings Window"))
			{
				IAPSettingsWindow.Open();
			}

            base.OnInspectorGUI();

            EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}

		#endregion

		#region Protected Methods

		#endregion

		#region Private Methods

		private void DrawProductEvents()
		{
			SerializedProperty purchaseEventsProp = serializedObject.FindProperty("purchaseEvents");

			for (int i = 0; i < IAPSettings.Instance.productInfos.Count; i++)
			{
				string productId = IAPSettings.Instance.productInfos[i].productId;

				SerializedProperty purchaseEventProp = null;

				if (purchaseEventsProp.arraySize == i)
				{
					purchaseEventProp = AddNewPurchaseEvent(purchaseEventsProp, purchaseEventsProp.arraySize, productId);
				}
				else
				{
					purchaseEventProp = purchaseEventsProp.GetArrayElementAtIndex(i);

					if (productId != purchaseEventProp.FindPropertyRelative("productId").stringValue)
					{
						int index = FindPurchaseEvent(purchaseEventsProp, productId, i + 1);

						if (index == -1)
						{
							purchaseEventProp = AddNewPurchaseEvent(purchaseEventsProp, i, productId);
						}
						else
						{
							purchaseEventsProp.MoveArrayElement(index, i);
						}
					}
				}

				EditorGUILayout.PropertyField(purchaseEventProp);

				if (purchaseEventProp.isExpanded)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(16);
					EditorGUILayout.PropertyField(purchaseEventProp.FindPropertyRelative("onProductPurchasedEvent"));
					EditorGUILayout.EndHorizontal();
				}
			}

			for (int i = purchaseEventsProp.arraySize - 1; i >= 0 && i >= IAPSettings.Instance.productInfos.Count; i--)
			{
				purchaseEventsProp.DeleteArrayElementAtIndex(i);
			}

			if (purchaseEventsProp.arraySize == 0)
			{
				EditorGUILayout.LabelField("There are no product ids set in the IAP Settings. Open the IAP Settings window to add your product ids.");
			}
		}

		private SerializedProperty AddNewPurchaseEvent(SerializedProperty purchaseEventsProp, int index, string productId)
		{
			purchaseEventsProp.InsertArrayElementAtIndex(index);

			SerializedProperty purchaseEventProp = purchaseEventsProp.GetArrayElementAtIndex(index);

			purchaseEventProp.FindPropertyRelative("productId").stringValue = productId;

			SerializedProperty eventsProp = purchaseEventProp.FindPropertyRelative("onProductPurchasedEvent");

			SerializedProperty persistentCalls = eventsProp.FindPropertyRelative ("m_PersistentCalls.m_Calls");

			for (int i = persistentCalls.arraySize - 1; i >= 0; i--)
			{
				persistentCalls.DeleteArrayElementAtIndex(i);
			}

			return purchaseEventProp;
		}

		private int FindPurchaseEvent(SerializedProperty purchaseEventsProp, string productId, int startIndex)
		{
			for (int i = startIndex; i < purchaseEventsProp.arraySize; i++)
			{
				if (productId == purchaseEventsProp.GetArrayElementAtIndex(i).FindPropertyRelative("productId").stringValue)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Begins a new foldout box, must call EndBox
		/// </summary>
		private bool BeginBox(SerializedProperty prop)
		{
			GUIStyle style		= new GUIStyle("HelpBox");
			style.padding.left	= 0;
			style.padding.right	= 0;
			style.margin.left = 0;

			GUILayout.BeginVertical(style);

			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(16f);

			prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);

			EditorGUILayout.EndHorizontal();

			if (prop.isExpanded)
			{
				DrawLine();
			}

			return prop.isExpanded;
		}

		/// <summary>
		/// Ends the box.
		/// </summary>
		private void EndBox()
		{
			GUILayout.EndVertical();
		}

		/// <summary>
		/// Draws a simple 1 pixel height line
		/// </summary>
		private void DrawLine()
		{
			GUIStyle lineStyle			= new GUIStyle();
			lineStyle.normal.background	= LineTexture;

			GUILayout.BeginVertical(lineStyle);
			GUILayout.Space(1);
			GUILayout.EndVertical();
		}

		#endregion
	}
}
