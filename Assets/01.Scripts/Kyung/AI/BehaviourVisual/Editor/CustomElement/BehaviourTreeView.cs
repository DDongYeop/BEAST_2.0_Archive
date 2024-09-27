using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Behaviour
{
    public class BehaviourTreeView : GraphView
    {
        public Action<NodeView> OnNodeSelected;
        
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }

        private BehaviourTree _tree;

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());
            
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/07.UI/Kyung/BehaviourEditor/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(_tree);
            AssetDatabase.SaveAssets();
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }

        public void PopulateView(BehaviourTree tree)
        {
            _tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.RootNode == null)
            {
                tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }
            
            tree.Nodes.ForEach(n => CreateNodeView(n));
            
            tree.Nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parentView = FindNodeView(n);
                    NodeView childView = FindNodeView(c);

                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                });
            });
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && startPort.node != endPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
        {
            if (graphviewchange.elementsToRemove != null)
            {
                graphviewchange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                        _tree.DeleteNode(nodeView.Node);
                    
                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        _tree.RemoveChild(parentView.Node, childView.Node);
                    }
                });
            }

            if (graphviewchange.edgesToCreate != null)
            {
                graphviewchange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.AddChild(parentView.Node, childView.Node);
                });
            }

            if (graphviewchange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    NodeView view = n as NodeView;
                    view.SortChildren();
                });
            }
            
            return graphviewchange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Delete", (a) => DeleteSelection());
            
            var actionTypes = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in actionTypes)
                evt.menu.AppendAction($"[{type.BaseType.Name}]/[{type.Name}]", (a) => CreateNode(type));
            
            var compositeTypes = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in compositeTypes)
                evt.menu.AppendAction($"[{type.BaseType.Name}]/[{type.Name}]", (a) => CreateNode(type));
            
            var decoratorTypes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in decoratorTypes)
                evt.menu.AppendAction($"[{type.BaseType.Name}]/[{type.Name}]", (a) => CreateNode(type));
        }

        private void CreateNode(System.Type type)
        {
            Node node = _tree.CreateNode(type);
            CreateNodeView(node);
        }

        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        public void UpdateNodeStates()
        {
            nodes.ForEach(n =>
            {
                NodeView view = n as NodeView;
                view.UpdateState();
            });
        }
    }
}
