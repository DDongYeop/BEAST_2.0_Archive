using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// If there is only one child node, it must be assigned to the left node.
/// </summary>

public class UINode
{
    public UINode(TechTreeNode node, UINode parent)
	{
        Node = node;
        Parent = parent;
	}

    public TechTreeNode Node;

    public UINode Parent;
    public List<UINode> Children = new List<UINode>();

    public float X;
    public float Y;
    public float Mod;

    public TechTreeNodeUI NodeUI;

    public bool IsLeap => Children.Count == 0;
    public bool IsLeftMost { get
		{
            if (Parent == null) return true;

            return Parent.Children[0] == this;
		} }

    public float GetRightMost()
	{
        if (Children.Count == 0)
            return Y;

        return Children[Children.Count - 1].GetRightMost() + Mod;
	}

    public float GetLeftMost()
	{
        if (Children.Count == 0)
            return Y;

        return Children[Children.Count - 1].GetLeftMost() + Mod;
    }
}

public class Scene_TechTree : UI_Scene
{
	[Header("References")]
	public WeaponController weaponController;

	[Header("Tech Tree")]
    [SerializeField] private RectTransform _nodeParent;
    [SerializeField] private TechTreeNodeUI _nodeUI;
	[Space]
	[SerializeField] private float yNodeSpace;
	[SerializeField] private float xNodeSpace;
	[SerializeField] private Vector2 nodeParentPadding;
	[Space]
	[SerializeField] private float infoPanelAppearTime = 1f;

    private UINode rootNode;
	private UINode selectedNode;

	public override void ActiveWithMotion(SceneRoutineCallback callback = null)
	{
		base.ActiveWithMotion(callback);

		CenterTheTree(rootNode);
	}

	protected override void Start()
	{
		base.Start();

		GetComponent<CanvasGroup>().alpha = 1f;

		if (weaponController == null)
			weaponController = GameManager.Instance.PlayerTrm.Find("WeaponController").GetComponent<WeaponController>();

        BindEvent(Get<Image>("Image_Close").gameObject, (PointerEventData _data, Transform _transform) =>
		{
            UIManager_InGame.Instance.HideScene(this);
        });
		Get<Button>("Button_BuyWeapon").onClick.AddListener(PurchaseWeapon);
		GameManager.Instance.OnMoneyChange += OnMoneyChanged;

        UIManager_InGame.Instance.HideScene(this);
	}

    protected override void OnEnable()
    {
		base.OnEnable();
		if (TimeManager.Instance != null)
			TimeManager.Instance.IsPause = true;
	}   

	protected override void OnDisable()
    {
		base.OnDisable();
		if (TimeManager.Instance != null)
			TimeManager.Instance.IsPause = false;
	}

	private void OnDestroy()
	{
		Get<Button>("Button_BuyWeapon").onClick.RemoveListener(PurchaseWeapon);
		if (!GameManager.InstanceIsNull)
			GameManager.Instance.OnMoneyChange -= OnMoneyChanged;
	}

	#region WeaponInfo

	public void OnMoneyChanged(int amount)
	{
		DOTween.To(() => int.Parse(Get<TextMeshProUGUI>("Text_CoinCount").text), NewValue => Get<TextMeshProUGUI>("Text_CoinCount").text = NewValue.ToString(), amount, 0.2f);
		// 변경해야지
	}

	private void SetWeaponInfo(UINode node)
	{
		if (node == null) return;

		selectedNode = node;

		FetchInfoPanel(selectedNode);

		Get<TextMeshProUGUI>("Text_WeaponInfo").text = selectedNode.Node.weapon.WeaponName;
		Get<TextMeshProUGUI>("Text_WeaponCost").text = node.Node.cost.ToString();
		Get<Image>("Image_WeaponInfo").sprite = selectedNode.Node.weapon.WeaponSprite;
	}

	/// <summary>
	/// 무기 구매 및 무기 선택
	/// </summary>
	private void PurchaseWeapon()
	{
		if (selectedNode == null) return; // 현재 선택된 노드를 기준으로 진행

		// 만약 부모 노드가 구매되지 않았다면 구매 불가
		if (selectedNode.Parent != null && !selectedNode.Parent.Node.isPurchased)
			return;

		// 이미 구매된 노드라면 장착
		if (selectedNode.Node.isPurchased)
		{
			weaponController.AttemptChangeWeaponStat(selectedNode.Node.weapon.WeaponId);
		}
		else
		{
			// 재화 계산
			if (selectedNode.Node.cost > GameManager.Instance.Money) return;
			GameManager.Instance.AddMoney(-selectedNode.Node.cost);

			selectedNode.Node.isPurchased = true;
			// 구매된 노드의 UI 갱신
			selectedNode.NodeUI.SetNodeStatus(NODE_STATUS.Purchased);

			// 구매된 노드의 자식들의 UI 갱신. 구매가 가능하게 되었음을 표시
			foreach (var child in selectedNode.Children)
			{
				child.NodeUI.SetNodeStatus(NODE_STATUS.Available);
			}
		}

		// 무기 정포 패널 UI 갱신
		FetchInfoPanel(selectedNode);
	}

	private void FetchInfoPanel(UINode node)
	{
		TextMeshProUGUI text = Get<TextMeshProUGUI>("Text_BuyWeapon");
		if (node.Node.isPurchased)
		{
			Get<Image>("Image_Unavailable").gameObject.SetActive(false);

			if (weaponController.CurrentWeaponStat.WeaponId == node.Node.weapon.WeaponId)
				text.text = "장착됨";
			else
				text.text = "장착";
		}
		else if (node.Parent != null && !node.Parent.Node.isPurchased)
		{
			Get<Image>("Image_Unavailable").gameObject.SetActive(true);
			text.text = "구매 불가";
		}
		else
		{
			Get<Image>("Image_Unavailable").gameObject.SetActive(false);
			text.text = "구매";
		}
	}

	#endregion

	#region Tech Tree Layout

	public void DrawTechTree(TechTree tree)
    {
	    weaponController.AttemptChangeWeaponStat(tree.rootNode.weapon.WeaponId);
	    
        rootNode = WrapNode(tree.rootNode, null);

		// 위치 설정
		CalculateInitialPosition(rootNode);
        AlignChild(rootNode);
        ResolveConflicts(rootNode);


		// 테크트리가 중앙에 오도록 정렬
		float shift = GetEdgeValue(true, rootNode) + GetEdgeValue(false, rootNode);
		rootNode.Mod -= shift * 0.5f;
		rootNode.Y -= shift * 0.5f;
		
		// 위치 이동
		Apportion(rootNode);
        PositionNodes(rootNode);

		Traversal(rootNode, node =>
		{
			node.NodeUI.transform.SetSiblingIndex(0);
		});

		// UI 설정
		FitNodeParent(rootNode);
		SetWeaponInfo(rootNode);
		foreach (UINode child in rootNode.Children)
			child.NodeUI.SetNodeStatus(NODE_STATUS.Available);
	}

	/// <summary>
	/// TechTreeNode로 이루어진 트리와 동일한 구조의 UINode 트리를 제작한다
	/// </summary>
	/// <param name="node"></param>
	/// <param name="parent"></param>
	/// <returns></returns>
	private UINode WrapNode(TechTreeNode node, UINode parent)
	{
        UINode wrapNode = new UINode(node, parent);

        TechTreeNodeUI nodeUI = Instantiate(_nodeUI, _nodeParent);
        wrapNode.NodeUI = nodeUI;
        nodeUI.gameObject.name = $"Image_{node.name}";
		nodeUI.backImage.onClick.AddListener(() => SetWeaponInfo(wrapNode));

        node.children.ForEach(child => wrapNode.Children.Add(WrapNode(child, wrapNode)));

        return wrapNode;
	}

	/// <summary>
	/// 트리 구조에서 좌측에 있는 노드를 반환
	/// </summary>
	/// <param name="node"></param>
	/// <returns>좌측에 있는 노드. 없으면 <see langword="null"/></returns>
    private UINode GetLeftSibling(UINode node)
	{
        if (node.Parent == null) return null;

        int index = node.Parent.Children.IndexOf(node);
        if (index > 0)
		{
            return node.Parent.Children[index - 1];
		}

        return null;
	}

	/// <summary>
	/// 초기 위치 계산
	/// </summary>
	private void CalculateInitialPosition(UINode node)
	{
        if (node.Parent != null)
            node.X = node.Parent.X + xNodeSpace;
		else
			node.X = xNodeSpace;

        node.Children.ForEach(child => CalculateInitialPosition(child));

        node.Y = node.IsLeftMost ? 0 : GetLeftSibling(node).Y + yNodeSpace;
	}

    private void AlignChild(UINode node)
	{
        node.Children.ForEach(child => AlignChild(child));

		if (node.IsLeap) return;

        float desireY = node.Y;

        if (node.Children.Count == 1)
		{
            desireY = node.Children[0].Y;
		}
        else if (node.Children.Count > 1)
		{
            float mid = (node.Children[0].Y + node.Children[node.Children.Count - 1].Y) * 0.5f;
            desireY = mid;
		}

        if (node.IsLeftMost)
		{
            node.Mod = -desireY;
		}
        else
            node.Mod = node.Y - desireY;
	}

	private void ResolveConflicts(UINode node)
	{
		foreach (UINode child in node.Children)
		{
			ResolveConflicts(child);
		}

		for (int i = 0; i < node.Children.Count; i++)
		{
			for (int j = i + 1; j < node.Children.Count; j++)
			{
				UINode leftNode = node.Children[i];
				UINode rightNode = node.Children[j];

				float shiftAmount = GetConflictShiftAmount(leftNode, rightNode);

				if (shiftAmount > 0)
				{
					ApplyShift(rightNode, shiftAmount);

					node.Mod -= shiftAmount * 0.5f;

					if (j - i > 1)
					{
						for (int k = i + 1; k < j; ++k)
						{
							node.Children[k].Y += shiftAmount / (j - i + 1f);
						}
					}
				}
			}
		}
	}

	private float GetConflictShiftAmount(UINode leftNode, UINode rightNode)
	{
		var rightContour = GetContour(leftNode, 0, true, new Dictionary<int, float>());
		var leftContour = GetContour(rightNode, 0, false, new Dictionary<int, float>());


		float maxOverlap = 0;

		foreach (int depth in rightContour.Keys)
		{
			if (leftContour.ContainsKey(depth))
			{
				float overlap = rightContour[depth] - leftContour[depth];
				maxOverlap = Mathf.Max(maxOverlap, overlap + yNodeSpace);
			}
		}

		return maxOverlap;
	}

	private void ApplyShift(UINode node, float shiftAmount)
	{
		node.Y += shiftAmount;
		node.Mod += shiftAmount;
	}

	private Dictionary<int, float> GetContour(UINode node, float modSum, bool isRight, Dictionary<int, float> contour)
	{
		if (!contour.ContainsKey((int)node.X))
		{
			contour[(int)node.X] = node.Y + modSum;
		}
		else
		{
			if (isRight)
				contour[(int)node.X] = Mathf.Max(contour[(int)node.X], node.Y + modSum);
			else
				contour[(int)node.X] = Mathf.Min(contour[(int)node.X], node.Y + modSum);
		}

		modSum += node.Mod;

		foreach (UINode child in node.Children)
		{
			GetContour(child, modSum, isRight, contour);
		}

		return contour;
	}

	private void Apportion(UINode node)
	{
        node.Children.ForEach(child => { 
			child.Y += node.Mod; 
			child.Mod += node.Mod;
			} );
		node.Mod = 0;

        node.Children.ForEach(child => Apportion(child));
	}

    private void PositionNodes(UINode node)
	{
        node.NodeUI.rectTransform.anchoredPosition = new Vector2(node.X, node.Y);
		node.NodeUI.Init(node, node.Parent);

		node.Children.ForEach(child => PositionNodes(child));
	}

	private void FitNodeParent(UINode rootNode)
	{
		float depth = GetTreeXSize(rootNode);
		float mostFar = Mathf.Max(GetEdgeValue(true, rootNode), Mathf.Abs(GetEdgeValue(false, rootNode))) + nodeParentPadding.y;
		_nodeParent.sizeDelta = new Vector2(depth + nodeParentPadding.x, mostFar * 2f);
	}

	private void CenterTheTree(UINode rootNode)
	{
		float y = Screen.height / rootNode.NodeUI.canvas.scaleFactor * -0.5f - rootNode.Y;
		(_nodeParent.parent.transform as RectTransform).anchoredPosition = new Vector2(short.MaxValue, y);
	}

	private float GetTreeXSize(UINode node)
	{
		if (node.IsLeap)
			return node.X;

		float max = node.X;
		node.Children.ForEach(child =>
		{
			max = Mathf.Max(max, GetTreeXSize(child));
		});

		return max;
	}
	
	private float GetEdgeValue(bool getRight, UINode node)
	{
		if (node.IsLeap)
			return node.Y;

		UINode child = node.Children[getRight ? node.Children.Count - 1 : 0];
		return GetEdgeValue(getRight, child) + node.Mod;
	}

	private void Traversal(UINode node, Action<UINode> action)
	{
		action?.Invoke(node);
		node.Children.ForEach(child => Traversal(child, action));
	}

	#endregion
}