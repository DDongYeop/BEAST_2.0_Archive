using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class RunableTutorial : TutorialBase
{
    // 1. 움직임 (뒤로만)
    public override async UniTask ProcessTutorial()
    {
        Debug.Log($"{GetType().Name} Start");

        // 게임의 배경 설명
        await notifyText.DoText(notifyMessageList[0], 1f);
        await notifyText.DoText(notifyMessageList[1], 1f);

        // CanvasGroup On
        handIconImage.transform.localPosition = squareLineImage.transform.localPosition;
        handIconImage.gameObject.SetActive(true);
        squareLineImage.gameObject.SetActive(true); 
        await imageCanvasGroup.DOFade(1f, 1f);
        
        // 튜토리얼 조건 안내 + 힌트
        handIconImage.DoFadeLoop(3).Forget();
        await notifyText.DoText(notifyMessageList[2], 1f);

        // 튜토리얼 조건 확인
        playerInput.IsActivate = true;
        await UniTask.WaitUntil(() => playerInput.IsMoveInputIn);
        this.DelayAfterAction(1.5f, () =>
        {
            // playerInput.HandleTouchEnded();
            playerInput.IsActivate = false;
        }).Forget();

        // CanvasGroup off
        await imageCanvasGroup.DOFade(0f, 1f);
        handIconImage.gameObject.SetActive(false);
        squareLineImage.gameObject.SetActive(false); 

        // 마무리 멘트
        await notifyText.DoText(notifyMessageList[3], 1f);

        Debug.Log($"{GetType().Name} End");
    }
}
