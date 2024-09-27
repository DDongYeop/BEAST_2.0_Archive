using UnityEngine.UI;

public class Scene_OpenTechTree : UI_Scene
{
	private bool isTechTreeOpen = false;
	public bool IsTechTreeOpen { get => isTechTreeOpen; set => isTechTreeOpen = value; }

	protected override void Start()
	{
		base.Start();

		ShowOpenTechTreeButton(false);

		Get<Button>("Button_OpenTechTree").onClick.AddListener(OpenTechTree);
	}

	private void OnDestroy()
	{
		Get<Button>("Button_OpenTechTree").onClick.RemoveListener(OpenTechTree);
	}

	private void OpenTechTree()
	{
		UIManager_InGame.Instance.ShowScene("Scene_TechTree", true);
	}

	public void OnVisibleChangedHandler(bool visible)
	{
		isTechTreeOpen = visible;

		if (visible)
		{
			UIManager_TechTree.Instance.HideScene(this);
		}
		else
		{
			UIManager_TechTree.Instance.ShowScene(this);
		}
	}

	public void ShowOpenTechTreeButton(bool active)
	{
		if (isTechTreeOpen && active) return;

		Get<Button>("Button_OpenTechTree").gameObject.SetActive(active);
	}
}
