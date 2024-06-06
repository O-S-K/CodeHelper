#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class MenuItemEditor : MonoBehaviour
{
    [MenuItem("Assets/Open with Photoshop")]
    private static void OpenPhotosshop()
    {
        Process photoViewer = new Process();
        photoViewer.StartInfo.FileName = @"D:\App Setup\Adobe Photoshop CC 2018\Photoshop";
        var inf = new FileInfo(AssetDatabase.GetAssetPath(Selection.activeObject));
        photoViewer.StartInfo.Arguments = inf.FullName;
        photoViewer.Start();
    }
    
    [MenuItem("Tools/Clear All Data")]
    private static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.Debug.Log("Clear All Data");
    }

    [MenuItem("Tools/Remove Missing Scripts In Obect")]
    public static void RemoveMissingScriptsInObect()
    {
        var allObject = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = allObject.Sum(GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
        foreach (var obj in allObject)
        {
            EditorUtility.SetDirty(obj);
        }

        AssetDatabase.Refresh();
        UnityEngine.Debug.Log($"<b><color=#ffa500ff>Removed {count} missing scripts</color></b>");
    }

    //[MenuItem("Tools/PushIndex+1")]
    //public static void ChangeNamePrefab()
    //{ 
    //    foreach (var obj in Selection.objects)
    //    {
    //        var Path = AssetDatabase.GetAssetPath(obj);
    //        string namePrefab = obj.name.Split(' ')[0] + " " + (int.Parse(obj.name.Split(' ')[1]) + 1) + ".prefab";
    //        var newPath = "Assets/Project/Resources/Levels/LevelSwap/" + namePrefab;
    //        File.Move(Path, newPath);
    //    }
    //    AssetDatabase.Refresh();
    //}


    //[MenuItem("Tools/ExportScene Package")]
    //public static void BuildPackage()
    //{
    //    var guids = AssetDatabase.FindAssets("", new string[]{
    //        "Assets/PrefabScene",
    //        "Assets/Gizmos"
    //    });

    //    var assets = new string[guids.Length];
    //    for (int i = 0; i < guids.Length; ++i)
    //        assets[i] = AssetDatabase.GUIDToAssetPath(guids[i]);

    //    var file = EditorUtility.SaveFilePanel("Export Package", "Assets/../..", "PrefabScene", "unitypackage");
    //    if (!string.IsNullOrEmpty(file))
    //        AssetDatabase.ExportPackage(assets, file, ExportPackageOptions.Default);
    //}
}

namespace AutoSaveScene
{
    // https://github.com/liortal53/AutoSaveScene/blob/master/Assets/Editor/AutoSaveScene.cs
}

// namespace PrefabEditor
// {
//     #region PrefabEditor Window
//     using UnityEditor.IMGUI.Controls;
//     using UnityObject = UnityEngine.Object;
//     public class PrefabEditor : EditorWindow
//     {
//         TreeViewState treeViewState;
//         PrefabTreeView treeView;
//         Transform preRoot;
//
//         [MenuItem("Window/Prefab Editor")]
//         static void ShowWindow()
//         {
//             var window = GetWindow<PrefabEditor>();
//             window.titleContent = new GUIContent("Prefab Editor");
//             window.Show();
//         }
//
//         void OnSelectionChange()
//         {
//             if (treeView != null && IsPrefabObject())
//             {
//                 var root = GetPrefabRoot();
//                 treeView.TargetPrefab = root;
//                 treeView.SetSelection(Selection.instanceIDs);
//                 var id = Selection.activeInstanceID;
//                 if (id != root.GetInstanceID())
//                 {
//                     treeView.FrameItem(id);
//                 }
//             }
//
//             Repaint();
//         }
//
//         void OnGUI()
//         {
//             DoToolbar();
//
//             if ((treeView == null && !IsPrefabObject()) || (treeView != null && treeView.TargetPrefab == null))
//             {
//                 GUILayout.Label("Please select prefab.");
//             }
//             else
//             {
//                 if (treeViewState == null)
//                 {
//                     treeViewState = new TreeViewState();
//                 }
//
//                 if (IsPrefabObject())
//                 {
//                     var root = GetPrefabRoot();
//                     if (root != preRoot || treeView == null)
//                     {
//                         treeView = new PrefabTreeView(treeViewState, root);
//                     }
//                     preRoot = root;
//                 }
//
//                 DoTreeView();
//             }
//
//         }
//
//         bool IsPrefabObject()
//         {
//             return Selection.activeGameObject != null && PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.Prefab;
//         }
//
//         Transform GetPrefabRoot()
//         {
//             return PrefabUtility.FindPrefabRoot(Selection.activeGameObject).transform;
//         }
//
//         void DoTreeView()
//         {
//             var rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
//             treeView.OnGUI(rect);
//         }
//
//         void DoToolbar()
//         {
//             GUILayout.BeginHorizontal(EditorStyles.toolbar);
//             GUILayout.FlexibleSpace();
//             GUILayout.EndHorizontal();
//         }
//     }
//
//     public class PrefabTreeView : TreeView
//     {
//         public Transform TargetPrefab { get; set; }
//         int selectId;
//
//         public PrefabTreeView(TreeViewState state, Transform targetPrefab) : base(state)
//         {
//             TargetPrefab = targetPrefab;
//
//             Reload();
//         }
//
//         protected override TreeViewItem BuildRoot()
//         {
//             return new TreeViewItem { id = 0, depth = -1 };
//         }
//
//         protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
//         {
//             if (TargetPrefab == null)
//             {
//                 return null;
//             }
//
//             var rows = GetRows() ?? new List<TreeViewItem>(200);
//             rows.Clear();
//
//             var item = CreateTreeViewItemForGameObject(TargetPrefab.gameObject);
//             root.AddChild(item);
//             rows.Add(item);
//             if (TargetPrefab.childCount > 0)
//             {
//                 if (IsExpanded(item.id))
//                 {
//                     AddChildrenRecursive(TargetPrefab, item, rows);
//                 }
//                 else
//                 {
//                     item.children = CreateChildListForCollapsedParent();
//                 }
//             }
//
//             SetupDepthsFromParentsAndChildren(root);
//
//             return rows;
//         }
//
//         void AddChildrenRecursive(Transform transform, TreeViewItem item, IList<TreeViewItem> rows)
//         {
//             var childCount = transform.childCount;
//             item.children = new List<TreeViewItem>(childCount);
//             for (var i = 0; i < childCount; i++)
//             {
//                 var childTransform = transform.GetChild(i);
//                 var childItem = CreateTreeViewItemForGameObject(childTransform.gameObject);
//                 item.AddChild(childItem);
//                 rows.Add(childItem);
//
//                 if (childTransform.childCount > 0)
//                 {
//                     if (IsExpanded(childItem.id))
//                     {
//                         AddChildrenRecursive(childTransform, childItem, rows);
//                     }
//                     else
//                     {
//                         childItem.children = CreateChildListForCollapsedParent();
//                     }
//                 }
//             }
//         }
//
//         static TreeViewItem CreateTreeViewItemForGameObject(GameObject gameObject)
//         {
//             return new TreeViewItem(gameObject.GetInstanceID(), -1, gameObject.name);
//         }
//
//         protected override IList<int> GetAncestors(int id)
//         {
//             var transform = GetGameObject(id).transform;
//
//             var ancestors = new List<int>();
//             while (transform.parent != null)
//             {
//                 ancestors.Add(transform.parent.gameObject.GetInstanceID());
//                 transform = transform.parent;
//             }
//
//             return ancestors;
//         }
//
//         protected override IList<int> GetDescendantsThatHaveChildren(int id)
//         {
//             var stack = new Stack<Transform>();
//
//             var start = GetGameObject(id).transform;
//             stack.Push(start);
//
//             var parents = new List<int>();
//             while (stack.Count > 0)
//             {
//                 var current = stack.Pop();
//                 parents.Add(current.gameObject.GetInstanceID());
//                 for (var i = 0; i < current.childCount; i++)
//                 {
//                     if (current.childCount > 0)
//                     {
//                         stack.Push(current.GetChild(i));
//                     }
//                 }
//             }
//
//             return parents;
//         }
//
//         GameObject GetGameObject(int instanceID)
//         {
//             return EditorUtility.InstanceIDToObject(instanceID) as GameObject;
//         }
//
//         protected override void RowGUI(RowGUIArgs args)
//         {
//             var e = Event.current;
//
//             var gameObject = GetGameObject(args.item.id);
//             if (gameObject == null)
//             {
//                 return;
//             }
//
//             var toggleRect = args.rowRect;
//             toggleRect.x = GetContentIndent(args.item);
//             toggleRect.width = 200.0f;
//
//             if (e.type == EventType.MouseUp && toggleRect.Contains(e.mousePosition))
//             {
//                 switch (e.button)
//                 {
//                     case 0:
//                         if (args.item.id == selectId)
//                         {
//                             BeginRename(args.item, 0.3f);
//                         }
//                         break;
//                     case 1:
//                         var menu = new GenericMenu();
//
//                         menu.AddItem(new GUIContent("Create Empty"), false, obj =>
//                         {
//                             var instance = PrefabUtility.InstantiatePrefab(TargetPrefab.gameObject) as GameObject;
//                             var prefabChildren = TargetPrefab.GetComponentsInChildren<Transform>(true);
//                             var instanceChildren = instance.GetComponentsInChildren<Transform>(true);
//
//                             var parent = GetInstanceFromPrefab(gameObject.transform, prefabChildren, instanceChildren);
//                             var newObject = new GameObject("GameObject");
//                             newObject.transform.SetParent(parent);
//
//                             PrefabUtility.ReplacePrefab(instance, PrefabUtility.GetPrefabParent(instance), ReplacePrefabOptions.ConnectToPrefab);
//
//                             Reload();
//                             instanceChildren = instance.GetComponentsInChildren<Transform>(true);
//                             prefabChildren = TargetPrefab.GetComponentsInChildren<Transform>(true);
//                             SetSelection(new int[] { GetPrefabFromInstance(newObject.transform, prefabChildren, instanceChildren).gameObject.GetInstanceID() },
//                                 TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
//
//                             UnityObject.DestroyImmediate(instance, true);
//                         }, "item 1");
//
//                         if (gameObject.transform.parent != null)
//                         {
//                             menu.AddItem(new GUIContent("Delete"), false, obj =>
//                             {
//                                 var instance = PrefabUtility.InstantiatePrefab(TargetPrefab.gameObject) as GameObject;
//                                 var prefabChildren = TargetPrefab.GetComponentsInChildren<Transform>(true);
//                                 var instanceChildren = instance.GetComponentsInChildren<Transform>(true);
//
//                                 foreach (var selectedId in state.selectedIDs)
//                                 {
//                                     var selectObject = GetGameObject(selectedId);
//                                     if (PrefabUtility.FindPrefabRoot(selectObject) != TargetPrefab.gameObject)
//                                     {
//                                         continue;
//                                     }
//
//                                     var parent = selectObject.transform.parent;
//                                     if (parent != null && !Selection.Contains(parent.gameObject))
//                                     {
//                                         UnityObject.DestroyImmediate(GetInstanceFromPrefab(selectObject.transform, prefabChildren, instanceChildren).gameObject, true);
//                                     }
//                                 }
//
//                                 PrefabUtility.ReplacePrefab(instance, PrefabUtility.GetPrefabParent(instance), ReplacePrefabOptions.ConnectToPrefab);
//
//                                 UnityObject.DestroyImmediate(instance, true);
//
//                                 Reload();
//                             }, "item 2");
//                         }
//
//                         menu.ShowAsContext();
//
//                         break;
//                 }
//
//                 selectId = args.item.id;
//             }
//
//             if (!args.isRenaming)
//             {
//                 var style = DefaultStyles.label;
//                 var transform = gameObject.transform;
//                 var activeSelf = true;
//                 while (transform.parent != null)
//                 {
//                     if (!transform.parent.gameObject.activeSelf)
//                     {
//                         activeSelf = false;
//                         break;
//                     }
//                     transform = transform.parent;
//                 }
//                 if (args.selected)
//                 {
//                     style.normal.textColor = gameObject.activeSelf && activeSelf ? Color.white : new Color(1.0f, 1.0f, 1.0f, 0.5f);
//                 }
//                 else
//                 {
//                     if (EditorGUIUtility.isProSkin)
//                     {
//                         style.normal.textColor = gameObject.activeSelf && activeSelf ? new Color(0.7f, 0.7f, 0.7f, 1.0f) : new Color(0.7f, 0.7f, 0.7f, 0.5f);
//                     }
//                     else
//                     {
//                         style.normal.textColor = gameObject.activeSelf && activeSelf ? Color.black : new Color(0.0f, 0.0f, 0.0f, 0.5f);
//                     }
//                 }
//                 GUI.Label(toggleRect, gameObject.name, style);
//             }
//         }
//
//         protected override bool CanRename(TreeViewItem item)
//         {
//             return true;
//         }
//
//         protected override void RenameEnded(RenameEndedArgs args)
//         {
//             GetGameObject(args.itemID).name = args.newName;
//             Reload();
//             AssetDatabase.SaveAssets();
//         }
//
//         protected override void SelectionChanged(IList<int> selectedIds)
//         {
//             Selection.instanceIDs = selectedIds.ToArray();
//         }
//
//         protected override bool CanStartDrag(CanStartDragArgs args)
//         {
//             return args.draggedItemIDs.Where(id => GetGameObject(id) == null).Count() == 0;
//         }
//
//         protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
//         {
//             DragAndDrop.PrepareStartDrag();
//
//             var sortedDraggedIDs = SortItemIDsInRowOrder(args.draggedItemIDs);
//
//             var unityObjects = new List<UnityObject>(sortedDraggedIDs.Count);
//             foreach (var id in sortedDraggedIDs)
//             {
//                 var unityObject = EditorUtility.InstanceIDToObject(id);
//                 if (unityObject != null)
//                 {
//                     unityObjects.Add(unityObject);
//                 }
//             }
//
//             DragAndDrop.objectReferences = unityObjects.ToArray();
//
//             var title = unityObjects.Count > 1 ? "<Multiple>" : unityObjects[0].name;
//             DragAndDrop.StartDrag(title);
//         }
//
//         protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
//         {
//             var draggedObjects = DragAndDrop.objectReferences;
//             if (draggedObjects.Where(draggedObject => draggedObject as GameObject == null).Count() > 0)
//             {
//                 return DragAndDropVisualMode.None;
//             }
//
//             if (args.performDrop)
//             {
//                 var instance = PrefabUtility.InstantiatePrefab(TargetPrefab.gameObject) as GameObject;
//                 var prefabChildren = TargetPrefab.GetComponentsInChildren<Transform>(true);
//                 var instanceChildren = instance.GetComponentsInChildren<Transform>(true);
//                 var transforms = new List<Transform>(draggedObjects.Length);
//                 var destroyInstances = new List<GameObject>();
//                 destroyInstances.Add(instance);
//                 foreach (var draggedObject in draggedObjects)
//                 {
//                     var gameObject = draggedObject as GameObject;
//                     if (gameObject == null)
//                     {
//                         foreach (var destroyInstance in destroyInstances)
//                         {
//                             UnityObject.DestroyImmediate(destroyInstance, true);
//                         }
//
//                         return DragAndDropVisualMode.None;
//                     }
//
//                     var transform = GetInstanceFromPrefab(gameObject.transform, prefabChildren, instanceChildren);
//                     if (transform == null)
//                     {
//                         if (PrefabUtility.GetPrefabType(gameObject) == PrefabType.Prefab)
//                         {
//                             var tmp = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;
//                             destroyInstances.Add(tmp);
//                             transforms.Add(tmp.transform);
//                         }
//                         else
//                         {
//                             var tmp = GameObject.Instantiate(gameObject);
//                             tmp.name = gameObject.name;
//                             transforms.Add(tmp.transform);
//                             destroyInstances.Add(tmp);
//                         }
//                     }
//                     else
//                     {
//                         transforms.Add(transform);
//                     }
//                 }
//
//                 RemoveItemsThatAreDescendantsFromOtherItems(transforms);
//
//                 switch (args.dragAndDropPosition)
//                 {
//                     case DragAndDropPosition.UponItem:
//                     case DragAndDropPosition.BetweenItems:
//                         var parent = args.parentItem != null
//                             ? GetInstanceFromPrefab(GetGameObject(args.parentItem.id).transform, prefabChildren, instanceChildren) : null;
//
//                         if (!IsValidReparenting(parent, transforms))
//                         {
//                             foreach (var destroyInstance in destroyInstances)
//                             {
//                                 UnityObject.DestroyImmediate(destroyInstance, true);
//                             }
//
//                             return DragAndDropVisualMode.None;
//                         }
//
//                         foreach (var transform in transforms)
//                         {
//                             transform.SetParent(parent);
//                         }
//
//                         if (args.dragAndDropPosition == DragAndDropPosition.BetweenItems)
//                         {
//                             int insertIndex = args.insertAtIndex;
//                             for (var i = transforms.Count - 1; i >= 0; i--)
//                             {
//                                 var transform = transforms[i];
//                                 insertIndex = GetAdjustedInsertIndex(parent, transform, insertIndex);
//                                 transform.SetSiblingIndex(insertIndex);
//                             }
//                         }
//                         break;
//                     case DragAndDropPosition.OutsideItems:
//                         foreach (var transform in transforms)
//                         {
//                             transform.SetParent(instance.transform);
//                         }
//                         break;
//                     default:
//                         throw new ArgumentOutOfRangeException();
//                 }
//
//                 PrefabUtility.ReplacePrefab(instance, PrefabUtility.GetPrefabParent(instance), ReplacePrefabOptions.ConnectToPrefab);
//
//                 Reload();
//                 prefabChildren = TargetPrefab.GetComponentsInChildren<Transform>(true);
//                 instanceChildren = instance.GetComponentsInChildren<Transform>(true);
//                 SetSelection(transforms.Where(t => t != null).Select(t => GetPrefabFromInstance(t, prefabChildren, instanceChildren).gameObject.GetInstanceID()).ToList(),
//                     TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
//
//                 foreach (var destroyInstance in destroyInstances)
//                 {
//                     UnityObject.DestroyImmediate(destroyInstance, true);
//                 }
//             }
//
//             return DragAndDropVisualMode.Move;
//         }
//
//         Transform GetInstanceFromPrefab(Transform target, Transform[] prefabChildren, Transform[] instanceChildren)
//         {
//             for (var i = 0; i < prefabChildren.Length; i++)
//             {
//                 if (prefabChildren[i] == target)
//                 {
//                     return instanceChildren[i];
//                 }
//             }
//
//             return null;
//         }
//
//         Transform GetPrefabFromInstance(Transform target, Transform[] prefabChildren, Transform[] instanceChildren)
//         {
//             for (var i = 0; i < instanceChildren.Length; i++)
//             {
//                 if (instanceChildren[i] == target)
//                 {
//                     return prefabChildren[i];
//                 }
//             }
//
//             return null;
//         }
//
//         int GetAdjustedInsertIndex(Transform parent, Transform transformToInsert, int insertIndex)
//         {
//             if (transformToInsert.parent == parent && transformToInsert.GetSiblingIndex() < insertIndex)
//             {
//                 return insertIndex--;
//             }
//
//             return insertIndex;
//         }
//
//         bool IsValidReparenting(Transform parent, List<Transform> transformsToMove)
//         {
//             if (parent == null)
//             {
//                 return true;
//             }
//
//             foreach (var transformToMove in transformsToMove)
//             {
//                 if (transformToMove == parent)
//                 {
//                     return false;
//                 }
//
//                 if (IsHoveredAChildOfDragged(parent, transformToMove))
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//
//
//         bool IsHoveredAChildOfDragged(Transform hovered, Transform dragged)
//         {
//             var transform = hovered.parent;
//             while (transform)
//             {
//                 if (transform == dragged)
//                 {
//                     return true;
//                 }
//                 transform = transform.parent;
//             }
//             return false;
//         }
//
//         static bool IsDescendantOf(Transform transform, List<Transform> transforms)
//         {
//             while (transform != null)
//             {
//                 transform = transform.parent;
//                 if (transforms.Contains(transform))
//                 {
//                     return true;
//                 }
//             }
//             return false;
//         }
//
//         static void RemoveItemsThatAreDescendantsFromOtherItems(List<Transform> transforms)
//         {
//             transforms.RemoveAll(t => IsDescendantOf(t, transforms));
//         }
//     }
//     #endregion
//
//     #region PrefabEditor Assets menu
//     // https://gist.github.com/ulrikdamm/338392c3b0900de225ec6dd10864cab4
//     public class EditPrefab
//     {
//         static System.Object getPrefab(System.Object selection)
//         {
//             var prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object)selection);
//             if (prefabType == PrefabType.PrefabInstance)
//             {
//                 var prefab = PrefabUtility.GetCorrespondingObjectFromSource((UnityEngine.Object)selection);
//                 return prefab;
//             }
//
//             if (prefabType == PrefabType.Prefab)
//             {
//                 return selection;
//             }
//
//             return null;
//         }
//
//         [MenuItem("Assets/Edit prefab")]
//         public static void editPrefab()
//         {
//             EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
//
//             var currentScene = EditorSceneManager.GetActiveScene().path;
//
//             var prefab = getPrefab(Selection.activeObject);
//             if (prefab == null) { UnityEngine.Debug.Log("Couldn't find prefab"); return; }
//
//             var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
//             var instance = (GameObject)PrefabUtility.InstantiatePrefab((UnityEngine.Object)prefab, scene);
//             Selection.activeObject = instance;
//             SceneView.lastActiveSceneView.FrameSelected();
//
//             var returnButton = new ReturnToSceneGUI();
//             returnButton.previousScene = currentScene;
//             returnButton.prefabInstance = instance;
//         }
//     }
//
//     public sealed class ReturnToSceneGUI
//     {
//         public string previousScene;
//         public GameObject prefabInstance;
//
//         public ReturnToSceneGUI()
//         {
//             SceneView.duringSceneGui += RenderSceneGUI;
//         }
//
//         public void RenderSceneGUI(SceneView sceneview)
//         {
//             var style = new GUIStyle();
//             style.margin = new RectOffset(10, 10, 10, 10);
//
//             Handles.BeginGUI();
//             GUILayout.BeginArea(new Rect(20, 20, 180, 300), style);
//             var rect = EditorGUILayout.BeginVertical();
//             GUI.Box(rect, GUIContent.none);
//
//             if (GUILayout.Button("Apply and return", new GUILayoutOption[0]))
//             {
//                 PrefabUtility.ReplacePrefab(prefabInstance, PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance), ReplacePrefabOptions.ConnectToPrefab);
//                 SceneView.duringSceneGui -= RenderSceneGUI;
//                 EditorSceneManager.OpenScene(previousScene);
//             }
//
//             if (GUILayout.Button("Discard changes", new GUILayoutOption[0]))
//             {
//                 SceneView.duringSceneGui -= RenderSceneGUI;
//                 EditorSceneManager.OpenScene(previousScene);
//             }
//
//             EditorGUILayout.EndVertical();
//             GUILayout.EndArea();
//             Handles.EndGUI();
//         }
//     }
//     #endregion
// }

namespace FrequentSettingsShortcut
{
    public class FrequentSettingsShortcut : EditorWindow
    {
        //[MenuItem("Project Settings/test")]
        //static void EditorPlaying()
        //{
        //    EditorApplication.ExecuteMenuItem(path);
        //}

        // [MenuItem("Project Settings/Input")]
        // private static void Input()
        // {
        //     SettingsService.OpenProjectSettings("Project/Input Manager");
        // }
        //
        // [MenuItem("Project Settings/Tags and Layers")]
        // private static void TaL()
        // {
        //     SettingsService.OpenProjectSettings("Project/Tags and Layers");
        // }
        //
        // [MenuItem("Project Settings/Audio")]
        // private static void Audio()
        // {
        //     SettingsService.OpenProjectSettings("Project/Audio");
        // }
        //
        // [MenuItem("Project Settings/Time")]
        // private static void Time()
        // {
        //     SettingsService.OpenProjectSettings("Project/Time");
        // }
        //
        // [MenuItem("Project Settings/Player")]
        // private static void Player()
        // {
        //     SettingsService.OpenProjectSettings("Project/Player");
        // }
        //
        // [MenuItem("Project Settings/Physics")]
        // private static void Physics()
        // {
        //     SettingsService.OpenProjectSettings("Project/Physics");
        // }
        //
        // [MenuItem("Project Settings/Physics 2D")]
        // private static void Physics2D()
        // {
        //     SettingsService.OpenProjectSettings("Project/Physics 2D");
        // }
        //
        // [MenuItem("Project Settings/Quality")]
        // private static void Quality()
        // {
        //     SettingsService.OpenProjectSettings("Project/Quality");
        // }
        //
        // [MenuItem("Project Settings/Graphics")]
        // private static void Graphics()
        // {
        //     SettingsService.OpenProjectSettings("Project/Graphics");
        // }
        //
        // [MenuItem("Project Settings/Editor")]
        // private static void Editor()
        // {
        //     SettingsService.OpenProjectSettings("Project/Editor");
        // }
        //
        // [MenuItem("Project Settings/Script Execution Order")]
        // private static void SeO()
        // {
        //     SettingsService.OpenProjectSettings("Project/Script Execution Order");
        // }
    }
}
#endif