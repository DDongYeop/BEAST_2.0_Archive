using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TechTree/Tech Tree")]
public class TechTree : ScriptableObject
{
	public TechTreeNode rootNode;

	public List<TechTreeNode> GetChildren(TechTreeNode parent)
	{
		List<TechTreeNode> children = new List<TechTreeNode>();

		children.AddRange(parent.children);

		return children;
	}

	public void Traverse(TechTreeNode node, System.Action<TechTreeNode> visitor)
	{
		if (node != null)
		{
			visitor?.Invoke(node);
			var children = GetChildren(node);
			children.ForEach((n) => Traverse(n, visitor));
		}
	}

	public TechTree Clone()
	{
		TechTree tree = Instantiate(this);

		tree.rootNode = rootNode.Clone();

		return tree;
	}
}
