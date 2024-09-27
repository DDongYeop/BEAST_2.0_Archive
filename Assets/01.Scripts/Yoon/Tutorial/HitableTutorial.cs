using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class HitableTutorial : TutorialBase
{
    [SerializeField] private GameObject wallCollider;

    private bool isProcessing = false;
    private bool isHit = false;

    public override async UniTask ProcessTutorial()
    {
        Debug.Log($"{GetType().Name} Start");

        isProcessing = true;

        // ½ÃÀÛ ¸àÆ® °â Æ©Åä¸®¾ó ¾È³»
        await notifyText.DoText(notifyMessageList[0], 1f);

        // CanvasGroup On
        squareLineImage.gameObject.SetActive(true);
        await imageCanvasGroup.DOFade(1f, 1f);

        // Æ©Åä¸®¾ó Á¶°Ç È®ÀÎ
        await UniTask.WaitUntil(() => isHit == true);
        playerInput.IsActivate = false;

        // CanvasGroup off
        await imageCanvasGroup.DOFade(0f, 1f);
        squareLineImage.gameObject.SetActive(false);

        // ¸¶¹«¸® ¸àÆ®
        await notifyText.DoText(notifyMessageList[1], 1f);
        await notifyText.DoText(notifyMessageList[2], 1f);

        isProcessing = false;

        Debug.Log($"{GetType().Name} End");
    }

    public void HandleOnDamaged()
    {
        if (isProcessing)
        {
            isHit = true;
        }
    }
}
