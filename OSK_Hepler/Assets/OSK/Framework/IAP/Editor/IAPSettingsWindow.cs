using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace OSK
{
	public class IAPSettingsWindow : CustomEditorWindow
	{
		#region Inspector Variables

		private SerializedObject settingsSerializedObject;

		private bool showPluginError;

		#endregion

		#region Properties

		private SerializedObject SettingsSerializedObject
		{
			get
			{
				if (settingsSerializedObject == null)
				{
					settingsSerializedObject = new SerializedObject(IAPSettings.Instance);
				}

				return settingsSerializedObject;
			}
		}

		#endregion

		#region Public Methods

		[MenuItem ("Window/IAP Settings")]
		public static void Open()
		{
			EditorWindow.GetWindow<IAPSettingsWindow>("IAP Settings");
		}

		#endregion

		#region Draw Methods

		public override void DoGUI()
		{
			SettingsSerializedObject.Update();

			DrawIAPSettings();

			GUI.enabled = true;

			SettingsSerializedObject.ApplyModifiedProperties();
		}

		private void DrawIAPSettings()
		{
			BeginBox("IAP Settings");

			Space();

			SerializedProperty productInfosProp = SettingsSerializedObject.FindProperty("productInfos");

			EditorGUILayout.PropertyField(productInfosProp, true);

			if (productInfosProp.isExpanded)
			{
				Space();
			}

			EndBox();
		}

		private void DrawEnableDisableButtons()
		{
			if (!IAPSettings.IsIAPEnabled)
			{
				EditorGUILayout.HelpBox("IAP is not enabled, please import the IAP plugin using the Services window then click the button below.", MessageType.Info);

				if (DrawButton("Enable IAP"))
				{
					if (!EditorUtilities.CheckNamespacesExists("UnityEngine.Purchasing"))
					{
						showPluginError = true;
					}
					else
					{
						showPluginError = false;

						EditorUtilities.SyncScriptingDefineSymbols("BBG_IAP", true);
					}
				}

				if (showPluginError)
				{
					EditorGUILayout.HelpBox("The Unity IAP plugin was not been detected. Please import the Unity IAP plugin using the Services window and make sure there are no compiler errors in your project. Consult the documentation for more information.", MessageType.Error);
				}
			}
			else
			{
				if (DrawButton("Disable IAP"))
				{
					// Remove BBG_IAP from scripting define symbols
					EditorUtilities.SyncScriptingDefineSymbols("BBG_IAP", false);
				}
			}

			GUI.enabled = IAPSettings.IsIAPEnabled;
		}

		#endregion
	}
}
