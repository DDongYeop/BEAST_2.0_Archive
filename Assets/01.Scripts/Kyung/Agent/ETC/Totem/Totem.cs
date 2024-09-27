using UnityEngine;

public class Totem : MonoBehaviour
{
    // 토템의 HP
    // 토템의 스킬 (없을 수도 ?)
    
    
    public void GameOver()
    {
        GameManager.Instance.GameOver();
    }
}
