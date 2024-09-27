using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { private set; get; }

    [Tooltip("Key : Time name, Value : TimeScale")]
    //[SerializeField] private SerializableDict<string, bool> m_times = new SerializableDict<string, bool>();
    public bool IsPause = false;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //public void SetDeltaTimeScale(string name, bool Scale)
    //{
    //    m_times[name] = Scale;
    //}

    //public bool GetTimeScale(string name)
    //{
    //    return m_times[name];
    //}

    //public float GetDeltaTime(string name)
    //{
    //    return m_times[name] * Time.deltaTime;
    //}
}
