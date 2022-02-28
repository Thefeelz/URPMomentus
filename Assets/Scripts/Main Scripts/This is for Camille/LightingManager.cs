using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] List<Animation> listOfRandomlyChosenAnimations = new List<Animation>();
    [SerializeField] float minimumTimeTillNewAnimation, maximumTimeTillNewAnimation;

    Animator attachedAnimator;
    // Start is called before the first frame update
    void Start()
    {
        attachedAnimator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChooseAnimationFromTheList()
    {
        int randomAnimationNumber = Random.Range(0, listOfRandomlyChosenAnimations.Count - 1);
        int timeDelayTillNextAnimation = Mathf.CeilToInt(Random.Range(minimumTimeTillNewAnimation, maximumTimeTillNewAnimation));
        StartCoroutine(PlayAnimation(listOfRandomlyChosenAnimations[randomAnimationNumber], timeDelayTillNextAnimation));
    }

    IEnumerator PlayAnimation(Animation animationToPlayNext, float timeToWaitUntilTheNextAnimationStarts)
    {
        animationToPlayNext.Play();
        yield return new WaitForSeconds(timeToWaitUntilTheNextAnimationStarts);
        animationToPlayNext.Stop();
        ChooseAnimationFromTheList();
    }
}
