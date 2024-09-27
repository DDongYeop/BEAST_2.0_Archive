using System;
using System.Collections;
using UnityEngine;

public class ParticleLoop : Particle
{
    private Vector3 _addPos;
    private Transform _rootTrm;

    private void Update()
    {
        if (_rootTrm)
            transform.position = _rootTrm.position + _addPos;
    }

    protected override IEnumerator ParticleCo()
    {
        _particle.Play();
        yield return null;
    }

    public void SetParentTrm(Transform trm, Vector3 addPos)
    {
        _rootTrm = trm;
        _addPos = addPos;
    }
}
