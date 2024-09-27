using System;
using Behaviour;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditor : EditorWindow
{
#if UNITY_EDITOR
    private BehaviourTreeView _treeView;
    private InspectorView _inspectorView;
    private IMGUIContainer _blackboardView;

    private SerializedObject _treeObject;
    private SerializedProperty _blackboardProperty;
 
    [MenuItem("Window/BehaviourTreeEditor")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        if (EditorApplication.isPlaying)
            return;
        
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        /*// VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);*/

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/07.UI/Kyung/BehaviourEditor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/07.UI/Kyung/BehaviourEditor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviourTreeView>();
        _inspectorView = root.Q<InspectorView>();
        _blackboardView = root.Q<IMGUIContainer>();
        _blackboardView.onGUIHandler = () =>
        {
            if (_treeObject != null)
            {
                _treeObject.Update();
                EditorGUILayout.PropertyField(_blackboardProperty);
                _treeObject.ApplyModifiedProperties();
            }
        };
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        
        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeObject.GetComponent<BehaviourTreeRunner>();
                if (runner)
                    tree = runner.Tree;
            }
        }

        if (Application.isPlaying)
        {
            if (_treeView != null&& tree)
                _treeView.PopulateView(tree);
        }
        else
        {
            if (_treeView != null&& tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                _treeView.PopulateView(tree);
        }

        if (tree != null)
        {
            _treeObject = new SerializedObject(tree);
            _blackboardProperty = _treeObject.FindProperty("Blackboard");
        }
    }

    private void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNodeStates();
    }
#endif
}
