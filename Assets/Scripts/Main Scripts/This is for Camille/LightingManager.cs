using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] float minimumTimeTillNewAnimation, maximumTimeTillNewAnimation;

    Animator attachedAnimator;
    AnimationClip[] clipInfo;
    // Start is called before the first frame update
    void Start()
    {
        attachedAnimator = GetComponent<Animator>();
        clipInfo = attachedAnimator.runtimeAnimatorController.animationClips;
        ChooseAnimationFromTheList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChooseAnimationFromTheList()
    {
        int randomAnimationNumber = Random.Range(0, clipInfo.Length - 1);
        int timeDelayTillNextAnimation = Mathf.CeilToInt(Random.Range(minimumTimeTillNewAnimation, maximumTimeTillNewAnimation));
        StartCoroutine(PlayAnimation(clipInfo[randomAnimationNumber], timeDelayTillNextAnimation));
    }

    IEnumerator PlayAnimation(AnimationClip animationToPlayNext, float timeToWaitUntilTheNextAnimationStarts)
    {
        attachedAnimator.Play(animationToPlayNext.name);
        yield return new WaitForSeconds(timeToWaitUntilTheNextAnimationStarts);
        ChooseAnimationFromTheList();
    }
}
