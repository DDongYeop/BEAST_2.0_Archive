using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    private CinemachineVirtualCamera _followCam;
    private CinemachineBasicMultiChannelPerlin _camPerlin;

    public override void Init()
    {
        _followCam = GetComponent<CinemachineVirtualCamera>();
        _camPerlin = _followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StopShake()
    {        
        _camPerlin.m_AmplitudeGain = 0;
        StopAllCoroutines();
    }

    public void CameraShake(float power, float time)
    {
        StopAllCoroutines();
        StartCoroutine(CameraShakeCo(power, time));
    }

    private IEnumerator CameraShakeCo(float power, float time)
    {
        _camPerlin.m_AmplitudeGain = power;
        yield return new WaitForSeconds(time);
        _camPerlin.m_AmplitudeGain = 0;
    }
}
