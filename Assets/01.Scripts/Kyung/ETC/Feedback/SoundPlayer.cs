using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : PoolableMono
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Init()
    {
        StartCoroutine(AudioPlayCo());
    }

    private IEnumerator AudioPlayCo()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length + 1f);
        PoolManager.Instance.Push(this);
    }
}
