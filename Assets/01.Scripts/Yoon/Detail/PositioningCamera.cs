using UnityEngine;

public class PositioningCamera : MonoBehaviour
{
    private Camera camera;

    private Transform playerTransform;

    private float initOffsetZ;
    private float offsetZ;

    [SerializeField] private float lerpSpeed = 3.0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Start()
    {
        playerTransform = GameManager.Instance.PlayerTrm;
        initOffsetZ = camera.transform.localPosition.z;
        SetOffsetZ(0);
    }

    public void SetOffsetZ(float offsetZ)
    {
        this.offsetZ = initOffsetZ + offsetZ;
    }

    private void LateUpdate()
    {
        if (camera != null)
        {
            Vector3 movePos = camera.transform.position;
            movePos.x = playerTransform.position.x;
            movePos.z = Mathf.Lerp(movePos.z, offsetZ, Time.deltaTime * lerpSpeed);
            camera.transform.position = movePos;
        }
    }
}
