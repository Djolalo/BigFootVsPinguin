using UnityEngine;

public class AnimationEndBehaviour : StateMachineBehaviour
{
    public string animationName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = animator.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.OnAnimationFinished(animationName);
        }
    }
}