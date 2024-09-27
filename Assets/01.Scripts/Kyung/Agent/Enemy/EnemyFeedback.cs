using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    [SerializeField] private string _currentBoss;
    [SerializeField] private List<TrailRenderer> _trails = new List<TrailRenderer>();

    [SerializeField] private List<Vector2> _dustEffectPos;
    [SerializeField] private Vector3 _spiderWebPos;

    private void Start()
    {
        ShowAttackTrailFalse();
    }

#region AnimationEvent

    public void ShowAttackTrailTrue()
    {
        foreach (var trail in _trails)
            trail.enabled = true;
    }

    public void ShowAttackTrailFalse()
    {
        foreach (var trail in _trails)
            trail.enabled = false;
    }

    public void DustEffect(int num)
    {
        Transform trm;

        if (num < 0)
            trm = PoolManager.Instance.Pop($"DustEffect{_currentBoss}Weak").transform;
        else 
            trm = PoolManager.Instance.Pop($"DustEffect{_currentBoss}").transform;
        
        trm.SetParent(transform);
        trm.localPosition = _dustEffectPos[Mathf.Abs(num)];
    }

#endregion
}
