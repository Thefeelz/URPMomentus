using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunToggle : MonoBehaviour
{
    [SerializeField] Material inRangeMaterial, outOfRangeMaterial;
    [SerializeField] int maxRangeForTurningOn;
    CharacterStats player;
    MeshRenderer attachedMesh;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterStats>();
        attachedMesh = GetComponent<MeshRenderer>();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
            CheckRange();
    }
    void CheckRange()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= maxRangeForTurningOn)
        {
            animator.SetBool("inRange", true);
            attachedMesh.material = inRangeMaterial;
        }
        else
        {
            animator.SetBool("inRange", false);
            attachedMesh.material = outOfRangeMaterial;
        }
    }
}
