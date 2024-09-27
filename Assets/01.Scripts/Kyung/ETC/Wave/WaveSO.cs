using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "SO/Wave/WaveData", fileName = "Wave")]
public class WaveSO : ScriptableObject
{
    public List<Wave> Waves;
}
