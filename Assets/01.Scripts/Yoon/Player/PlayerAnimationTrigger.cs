using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = transform.root.GetComponent<PlayerController>();
    }

    private void AnimationEndTrigger()
    {
        playerController.AnimationEndTrigger();
    }
}