using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowableTutorial : TutorialBase
{
    // 2. 조준과 발사
    public override async UniTask ProcessTutorial()
    {
        Debug.Log($"{GetType().Name} Start");

        // 시작 멘트
        await notifyText.DoText(notifyMessageList[0], 1f);

        // CanvasGroup On
        handIconImage.transform.localPosition = squareLineImage.transform.localPosition;
        handIconImage.gameObject.SetActive(true);
        squareLineImage.gameObject.SetActive(true);
        await imageCanvasGroup.DOFade(1f, 1f);

        // 튜토리얼 안내
        handIconImage.DoFadeLoop(3).Forget();
        await notifyText.DoText(notifyMessageList[1], 1f);

        // 튜토리얼 조건 확인
        playerInput.IsActivate = true;
        await UniTask.WaitUntil(() => playerInput.IsThrowReady);
        await UniTask.Delay(1000);

        // CanvasGroup off
        await imageCanvasGroup.DOFade(0f, 1f);
        handIconImage.gameObject.SetActive(false);
        squareLineImage.gameObject.SetActive(false);

        // 마무리 멘트
        await notifyText.DoText(notifyMessageList[2], 1f);

        Debug.Log($"{GetType().Name} End");
    }
}
