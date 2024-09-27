using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ExtenstionMethod
{
    public static async UniTask DoText(this TextMeshProUGUI textUI, string text, float duration)
    {
        await textUI.DOFade(1f, 0.8f);

        textUI.maxVisibleCharacters = 0;
        textUI.text = text;
        
        await DOTween.To(() => textUI.maxVisibleCharacters, x => textUI.maxVisibleCharacters = x, textUI.text.Length, duration);
        await UniTask.Delay(2000);

        await textUI.DOFade(0f, 0.8f);
        textUI.text = string.Empty;
    }

    public static async UniTask DoFadeLoop(this Image image, int loopCount)
    {
        for (int i = 0; i < loopCount; i++)
        {
            await image.DOFade(0f, 0.5f);
            await image.DOFade(1f, 0.5f);
        }
    }

    public static async UniTask WaitForTouchInput(this TutorialBase tutorialBase)
    {
        await UniTask.WaitUntil(() => Input.touchCount > 0);
    }

    public static async UniTaskVoid DelayAfterAction(this MonoBehaviour tutorialBase, float delayDuration, Action action)
    {
        await UniTask.WaitForSeconds(delayDuration);
        action();
    }
}
