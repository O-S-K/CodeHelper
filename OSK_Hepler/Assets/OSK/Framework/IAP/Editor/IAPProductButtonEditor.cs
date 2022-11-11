using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OSK
{
	[CustomEditor(typeof(IAPProductButton))]
	public class IAPProductButtonEditor : Editor
	{
		#region Inspector Variables

		#endregion

		#region Member Variables

		#endregion

		#region Properties

		#endregion

		#region Unity Methods

		#endregion

		#region Public Methods

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("productIndex"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("titleText"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("descriptionText"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("priceText"));

			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties();
		}

		#endregion
	}
}
