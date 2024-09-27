using UnityEngine;

public class TechTreeOpener : MonoBehaviour
{
	//private Scene_OpenTechTree techTreeUI;
	private Scene_InGame ingameUI;

	[SerializeField] private TechTree techTree;

	private void Start()
	{
		if (techTree == null) return;


		techTree = techTree.Clone();
		techTree.rootNode.isPurchased = true;

		//techTreeUI = UIManager_TechTree.Instance.GetScene("Scene_OpenTechTree") as Scene_OpenTechTree;

		ingameUI = UIManager_InGame.Instance.GetScene("Scene_InGame") as Scene_InGame;

		(UIManager_InGame.Instance.GetScene("Scene_TechTree") as Scene_TechTree).DrawTechTree(techTree);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �÷��̾� �ݶ��̴��� ����� ��(isTrigger�� ��Ȱ��ȭ �Ǿ��ִ� �ݶ��̾�)
		if (!collision.isTrigger)
		{
			if (collision.attachedRigidbody.TryGetComponent(out PlayerController player))
			{
				//techTreeUI.ShowOpenTechTreeButton(true);
				ingameUI.ShowInteraction(true, "Tech Tree");
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			if (collision.attachedRigidbody.TryGetComponent(out PlayerController player))
			{
				//techTreeUI.ShowOpenTechTreeButton(false);
				ingameUI.ShowInteraction(false);
			}
		}
	}
}
