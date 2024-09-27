using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private Transform throwTransform;
    private Vector3 throwPositionOffset = new Vector3(-0.1f, 0.2f, 0);

    [Header("�߻� ��� ǥ��")]
    [SerializeField] private Vector2 throwRange;
    [SerializeField] private int countOfPoints;
    [SerializeField] private float timeIntervalInPoints = 0.01f;

    [Header("DisplayAndEffect")]
    [SerializeField] private PositioningCamera positioningCamera;
    [SerializeField] private DisplayThrownWeapon mainDisplay;
    [SerializeField] private DisplayThrownWeapon subDisplay;
    [SerializeField] private ParticleSystem skillNotifyEffect;

    // Component
    private WeaponController weaponController;
    private LineRenderer lineRenderer;

    // Throw
    [Header("Stat")]
    [SerializeField] private ThrownWeaponStat equipWeaponStat; // ���� ���� ���� ���� ����
    public string EquipWeaponId => equipWeaponStat.WeaponId; 

    private ThrowInfo throwInfo;

    // ���� get; private set;
    public SkillData equipSkillData; // ���� ���� ���� ���� ����
    private int throwCount;
    public int ThrowCount => throwCount;

    private float maxForceValue = 49;

    private void Awake()
    {
        weaponController = transform.Find("WeaponController").GetComponent<WeaponController>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = countOfPoints;
        lineRenderer.enabled = false;

        throwInfo = new ThrowInfo();
        throwInfo.throwForce = throwForce;
    }

    private void OnEnable()
    {
        weaponController.OnWeaponStatChanged += AcceptChangeWeaponStat;
        weaponController.OnTresureDataChanged += AcceptChangeTreasureData;
    }

    private void OnDisable()
    {
        weaponController.OnWeaponStatChanged -= AcceptChangeWeaponStat;
        weaponController.OnTresureDataChanged -= AcceptChangeTreasureData;
    }

    #region Aiming

    public bool Aiming(Vector2 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            throwInfo.DragDirection = direction;
        }
        // else
        // {
        //     throwInfo.DragDirection = Vector2.zero;
        //     return false;
        // }

        mainDisplay.SetForwardAngle(throwInfo.ReverseDragDirection);
        subDisplay.SetLayerToFront("UI");

        // ���� �����ֱ�
        Vector2 throwVector = throwInfo.Force * (maxForceValue / equipWeaponStat.WeaponMass);
        ShowTrajectory(throwVector);

        // ī�޶� �ܾƿ� (��Ÿ��� ������ ������ ����.)
        if ((throwVector.magnitude) * 0.05f >= 5f)
        {
            positioningCamera.SetOffsetZ(-(throwVector.magnitude) * 0.05f);
        }

        return true;
    }

    public void EndAiming()
    {
        lineRenderer.enabled = false;

        mainDisplay.OnDisplay(equipWeaponStat);
        subDisplay.OnDisplay(equipWeaponStat);

        positioningCamera.SetOffsetZ(0);
    }

    // �߻� ��� ǥ��
    private void ShowTrajectory(Vector2 force)
    {
        float timeStep = timeIntervalInPoints; 

        Vector3[] trajectoryPoints = new Vector3[countOfPoints];
        Vector3 startPosition = throwTransform.position;
        Vector3 velocity = force / equipWeaponStat.WeaponMass;

        for (int i = 0; i < countOfPoints; i++)
        {
            trajectoryPoints[i] = startPosition;

            Vector3 acceleration = Physics2D.gravity;
            velocity += acceleration * timeStep;
            startPosition += velocity * timeStep;
        }

        // ��� �ð�ȭ
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(trajectoryPoints);
    }

    #endregion

    public void Throw()
    {
        // ���� ���� �� �߻�
        ThrownWeapon thrownWeapon = PoolManager.Instance.Pop(equipWeaponStat.WeaponId) as ThrownWeapon;
        thrownWeapon.transform.parent = null;
        thrownWeapon.transform.position = throwTransform.position;
        thrownWeapon.ThrowThisWeapon(throwInfo.Force);

        // 0.4�� �� ���� �̹��� ���ֱ�
        StartCoroutine(OnDisplayAfterDelay(0.5f));

        // ����
        PoolManager.Instance.Pop("ThrowSound");

        // ������ ������ �� ��ų�� �ߵ��ȴٸ� ����Ʈ�� ���ش�.
        // if (throwCount >= equipSkillData.CountForRecharge)
        // {
        //     skillNotifyEffect.Play();
        // }
    }

    private IEnumerator OnDisplayAfterDelay(float deleyTime = 0f)
    {
        // ���� �̹��� ���ֱ�
        mainDisplay.OffDisplay();
        subDisplay.SetLayerToBack();

        yield return new WaitForSeconds(deleyTime);

        mainDisplay.OnDisplay(equipWeaponStat);
        subDisplay.OnDisplay(equipWeaponStat);
    }

    // ���� ���� ����
    private void AcceptChangeWeaponStat(ThrownWeaponStat newWeaponStat)
    {
        if (equipWeaponStat == newWeaponStat)
        {
            return;
        }
        
        equipWeaponStat = newWeaponStat;
        StartCoroutine(OnDisplayAfterDelay());
    }

    // ���� ����
    private void AcceptChangeTreasureData(SkillData treasureData)
    {
        if (equipSkillData == treasureData)
        {
            return;
        }

        equipSkillData = treasureData;

        Scene_InGame _UI = UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame;
        _UI.OnThrow(throwCount, equipSkillData.CountForRecharge);
    }
}