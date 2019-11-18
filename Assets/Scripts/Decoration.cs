using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Decoration : MonoBehaviour
{
    [Header("Animation")]
    public Animator decorationAnimator;
    public string AnimationKey = "IsPlaying";

    [Header("Canvas")]
    public CanvasGroup decorationCanvas;
    public float canvasFadeDuration = .2f;

    private void Awake()
    {
        if (decorationCanvas != null)
        {
            decorationCanvas.alpha = 0;
        }
    }

    private void OnMouseDown()
    {
        if(decorationAnimator != null)
        {
            StartCoroutine(PlayAnimation(decorationAnimator, AnimationKey));
        }
    }

    IEnumerator PlayAnimation(Animator animator, string key)
    {
        animator.SetBool(key, true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        animator.SetBool(key, false);
    }

    private void OnMouseEnter()
    {
        if(decorationCanvas != null)
        {
            decorationCanvas.LeanAlpha(1, canvasFadeDuration);
        }
    }

    private void OnMouseExit()
    {
        if (decorationCanvas != null)
        {
            decorationCanvas.LeanAlpha(0, canvasFadeDuration);
        }
    }
}
