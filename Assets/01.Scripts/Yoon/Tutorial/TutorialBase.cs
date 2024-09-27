using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public abstract class TutorialBase : MonoBehaviour
{
    protected TextMeshProUGUI notifyText;
    protected CanvasGroup imageCanvasGroup;
    protected Image handIconImage;

    protected PlayerController playerController;
    protected PlayerInput playerInput;

    [TextArea(2, 5)]
    [SerializeField] protected List<string> notifyMessageList = new List<string>();

    [SerializeField] protected Image squareLineImage;

    public void SetUIProperty(TutorialController tutorialController)
    {
        notifyText = tutorialController.NotifyText;
        imageCanvasGroup = tutorialController.ImageCanvasGroup;
        handIconImage = tutorialController.HandIconImage;

        this.playerController = tutorialController.PlayerController;
        this.playerInput = tutorialController.PlayerController.PlayerInput;
    }

    public abstract UniTask ProcessTutorial();
}
