using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new NodeInfo", menuName = "SO/TechTree/NodeInfo")]
public class NodeInfo : ScriptableObject
{
    [SerializeField] private string m_Name;
    public string Name => m_Name;

    [SerializeField] private Sprite m_Sprite;
    public Sprite Sprite => m_Sprite;
}
