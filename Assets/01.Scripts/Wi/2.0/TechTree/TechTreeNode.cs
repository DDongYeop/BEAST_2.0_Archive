using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TechTree/Node")]
public class TechTreeNode : ScriptableObject
{
	public ThrownWeaponInfo weapon;
	public int cost;

	public List<TechTreeNode> children;

	[HideInInspector] public bool isPurchased;

	public virtual TechTreeNode Clone()
	{
		TechTreeNode clone = Instantiate(this);
		clone.children = children.ConvertAll(n => n.Clone());
		return clone;
	}
}
