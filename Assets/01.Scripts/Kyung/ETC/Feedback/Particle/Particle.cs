using System.Collections;
using UnityEngine;

public class Particle : PoolableMono
{
    [SerializeField] private float _lifeTime;
    protected ParticleSystem _particle;

    protected virtual void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        StartCoroutine(ParticleCo());
    }

    protected virtual IEnumerator ParticleCo()
    {
        _particle.Play();
        yield return new WaitForSeconds(_lifeTime);
        PoolManager.Instance.Push(this);
    }
}
