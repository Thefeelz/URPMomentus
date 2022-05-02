using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    
    [SerializeField] FMODUnity.EventReference footstepsRef;
    [SerializeField] FMODUnity.EventReference slideRef;
    [SerializeField] FMODUnity.EventReference throwingRef;
    [SerializeField] FMODUnity.EventReference ProjectileRef;
    [SerializeField] FMODUnity.EventReference WeaponAttackNoiseRef;


    FMOD.Studio.EventInstance footsteps;
    FMOD.Studio.EventInstance slide;
    FMOD.Studio.EventInstance throwing;

    public P_Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsRef);
        slide = FMODUnity.RuntimeManager.CreateInstance(slideRef);
        throwing = FMODUnity.RuntimeManager.CreateInstance(throwingRef);

        playerMovement = GetComponent<P_Movement>();

    }

    public void PlayFootsteps()
    {
        if (playerMovement.isGrounded) 
            FMODUnity.RuntimeManager.PlayOneShot(footstepsRef, transform.position);
    }

    public void PlaySlide()
    {
        FMODUnity.RuntimeManager.PlayOneShot(slideRef, transform.position);
    }

    public void ThrowSword()
    {
        FMODUnity.RuntimeManager.PlayOneShot(throwingRef, transform.position);
    }

    public void ProjectileNoise()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ProjectileRef, transform.position);
    }

    public void WeaponAttackNoise()
    {
        FMODUnity.RuntimeManager.PlayOneShot(WeaponAttackNoiseRef, transform.position);
    }
}
