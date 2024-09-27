using UnityEngine;

public class FollowCanvas : MonoBehaviour
{
    private Transform playerTransform;

    private Vector3 newMovePosition;

    private void Start()
    {
        playerTransform = GameManager.Instance.PlayerTrm;
        newMovePosition = transform.position;
    }

    private void Update()
    {
        newMovePosition.x = playerTransform.position.x;
        transform.position = newMovePosition;
        transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
    }

}
