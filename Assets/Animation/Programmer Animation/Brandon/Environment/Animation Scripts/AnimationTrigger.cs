using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator animOne;
    [SerializeField] string boolName;
    [SerializeField] bool boolStatus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TriggerAnimation() { animOne.SetBool(boolName, boolStatus); }
}
