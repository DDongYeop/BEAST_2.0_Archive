using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : AgentHealth
{
    private List<Material> _fadeMaterials = new List<Material>();
    private readonly int _alphaValue = Shader.PropertyToID("_AlphaValue");

    protected override void Awake()
    {
        base.Awake();

        AddFadeMat(transform);
    }

    private void AddFadeMat(Transform trm)
    {
        SpriteRenderer spriteRenderer; 
        for (int i = 0; i < trm.childCount; ++i)
        {
            trm.GetChild(i).TryGetComponent(out spriteRenderer);
            if (spriteRenderer)
                _fadeMaterials.Add(spriteRenderer.material);
            
            AddFadeMat(trm.GetChild(i));
        }
    }

    [ContextMenu("Damage")]
    private void Damage()
    {
        OnDamage(1000000000, Vector3.zero);
    }

    protected override IEnumerator EnemyDie()
    {
        WaveManager.Instance.MonsterDie();
        _brain.IsDie = true;
        _agentAnimator.OnDie();
        yield return new WaitForSeconds(.5f);
        _brain.MoneyDrop();
        StartCoroutine(Fade(1, 0, .5f));
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    private IEnumerator Fade(float startValue, float endValue, float time)
    {
        float currentTime = 0;

        while (currentTime <= time)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float t = currentTime / time;
            t = Mathf.Lerp(startValue, endValue, t);
            
            for (int i = 0; i < _fadeMaterials.Count; ++i)
                _fadeMaterials[i].SetFloat(_alphaValue, t);
        }
    }
}
