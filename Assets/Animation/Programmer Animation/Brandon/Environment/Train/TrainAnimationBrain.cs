using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAnimationBrain : MonoBehaviour
{
    [SerializeField] List<Animator> carAnims = new List<Animator>();
    [SerializeField] List<ParticleSystem> rockFalling = new List<ParticleSystem>();
    [SerializeField] float particleTurnOffDelay, particleStartDelay;
    bool filledUp = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnOnParticles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOffRockFall()
    {
        foreach (ParticleSystem ps in rockFalling)
        {
            ps.Stop();
        }
    }
    public void SetTrainCarsToFull()
    {
        foreach(Animator a in carAnims)
        {
            a.SetBool("filledUp", true);
        }
    }
    public void SetFilledUpToTrue()
    {
        if (filledUp) { return; }
        GetComponent<Animator>().SetBool("filledUp", true);
        filledUp = true;
    }
    void TurnOnParticlez()
    {
        foreach (ParticleSystem ps in rockFalling)
        {
            ps.Play();
        }
    }
    IEnumerator TurnOffParticles()
    {
        yield return new WaitForSeconds(particleTurnOffDelay);
        TurnOffRockFall();
        SetTrainCarsToFull();
    }
    IEnumerator TurnOnParticles()
    {
        yield return new WaitForSeconds(particleStartDelay);
        TurnOnParticlez();
        StartCoroutine(TurnOffParticles());
    }
}
