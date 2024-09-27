using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStick : MonoBehaviour //공격하면 꽂히는거
{
    private Queue<Transform> _stickTrm = new Queue<Transform>();
    [SerializeField] private float _weaponStickTime = 0.075f;

    public void AddObj(Transform trm, Transform part)
    {
        StartCoroutine(LookTargetCo(trm, part));
        trm.GetComponent<ThrownWeapon>().ObjectStick();
        trm.SetParent(part);
        _stickTrm.Enqueue(trm);
    }

    public void RemoveObj()
    {
        if (_stickTrm.Count == 0)
            return;
        
        Transform obj = _stickTrm.Dequeue();
        obj.GetComponent<ThrownWeapon>().ObjectFall();
    }

    public void RemoveAll()
    {
        Transform trm;
        while (_stickTrm.Count > 0)
        {
            trm = _stickTrm.Dequeue();
            trm.GetComponent<ThrownWeapon>().ObjectFall();
        }
    }

    private IEnumerator LookTargetCo(Transform trm, Transform part)
    {
        Vector2 newPos = part.position - trm.position;
        float startZ = trm.eulerAngles.z;
        float endZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg - 90;
        Vector3 startPos = trm.position;
        Vector3 endPos = trm.position + ((Vector3)trm.GetComponent<Rigidbody2D>().velocity.normalized * 0.25f);
        
        float currentTime = 0;

        while (currentTime <= _weaponStickTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float t = currentTime / _weaponStickTime;
            trm.rotation = Quaternion.Euler(0,0, Mathf.LerpAngle(startZ, endZ, t));
            trm.position = Vector3.Lerp(startPos, endPos, t);
        }
    }
    
    public int StickCount()
    {
        return _stickTrm.Count;
    }
}
