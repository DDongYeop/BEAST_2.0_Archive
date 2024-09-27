using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    [SerializeField] private float MoveSpeed = 0.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(float x)
    {
        rigidbody.velocity = new Vector2(x, 0) * MoveSpeed;
        FlipSprite(x);
    }

    public void FlipSprite(float x)
    {
        bool isMoving = Mathf.Abs(x) > Mathf.Epsilon; // Epsilon : 0에 가장 가까운, 아주 작은 실수값
        if (isMoving)
        {
            float flip = Mathf.Sign(x);
            transform.localScale = new Vector3(flip, 1, 1);
        }
    }

    // 즉시 멈춤
    public void StopImmediately(bool withYAxis = true)
    {
        if (withYAxis)
        {
            rigidbody.velocity = Vector2.zero;
        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
    }
}
