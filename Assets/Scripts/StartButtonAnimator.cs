using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonAnimator : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("clicked", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SceneManager.LoadScene("GamePlay");
    }
}
