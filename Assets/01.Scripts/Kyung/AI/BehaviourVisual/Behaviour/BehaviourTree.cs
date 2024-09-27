using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Behaviour
{
    [CreateAssetMenu(menuName = "SO/Behaviour/BehaviourTree", fileName = "BehaviourTree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;
        public State TreeState = State.RUNNING;
        public List<Node> Nodes = new List<Node>();
        public Blackboard Blackboard = new Blackboard(); 

        public State Update()
        {
            //if (RootNode.State == State.RUNNING)
            if (!TimeManager.Instance.IsPause)
                TreeState = RootNode.Update();
            return TreeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
#if UNITY_EDITOR
            node.name = type.Name;
            
            node.Guid = GUID.Generate().ToString();
            
            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
            Nodes.Add(node);
            
            if (Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);
            
            AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
            AssetDatabase.SaveAssets();
#endif
            return node;
        }

        public void DeleteNode(Node node)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
            Nodes.Remove(node);
            
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
#endif
        }

        public void AddChild(Node parent, Node Child)
        {
            
#if UNITY_EDITOR
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.Child = Child;
                EditorUtility.SetDirty(decorator);
            }

            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
                rootNode.Child = Child;
                EditorUtility.SetDirty(rootNode);
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.Children.Add(Child);
                EditorUtility.SetDirty(composite);
            }
#endif
        }
        
        public void RemoveChild(Node parent, Node Child)
        {
#if UNITY_EDITOR
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
                decorator.Child = null;
                EditorUtility.SetDirty(decorator);
            }
            
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (RemoveChild)");
                rootNode.Child = null;
                EditorUtility.SetDirty(rootNode);
            }
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
                composite.Children.Remove(Child);
                EditorUtility.SetDirty(composite);
            }
#endif
        }
        
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
#if UNITY_EDITOR
            
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.Child != null)
                children.Add(decorator.Child);
            
            RootNode rootNode = parent as RootNode;
            if (rootNode && rootNode.Child != null)
                children.Add(rootNode.Child);
            
            CompositeNode composite = parent as CompositeNode;
            if (composite)
                return composite.Children;
#endif

            return children;
        }

        public void Traverse(Node node, System.Action<Node> visiter)
        {
            if (node)
            {
                visiter?.Invoke(node);
                var children = GetChildren(node);
                children.ForEach(n => Traverse(n, visiter));
            }
        }
        
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            tree.Nodes = new List<Node>();
            Traverse(tree.RootNode, (n) =>
            {
                tree.Nodes.Add(n);
            });
            return tree;
        }

        public void Bind(EnemyBrain brain)
        {
            RootNode.Init(brain, Blackboard);
        }
    }
}
