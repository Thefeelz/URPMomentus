using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsSetUp : MonoBehaviour
{
    [SerializeField] Slider mouseSensitivitySlider;
    [SerializeField] List<AudioClip> mainMenuMoosic = new List<AudioClip>();
    [SerializeField] List<AudioClip> randomlyGeneratedList = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;
    GameManager gameManager;
    int currentTrackNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mouseSensitivitySlider.value = gameManager.GetMouseSensitivity();
        // audioSource.PlayOneShot(RandomSong());
        //GenerateRandomList();
        //PlayRandomList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    AudioClip RandomSong()
    {
        int songChose = Random.Range(0, mainMenuMoosic.Count);
        return mainMenuMoosic[songChose];
    }
    void GenerateRandomList()
    {
        List<AudioClip> tempList = new List<AudioClip>();
        foreach (AudioClip clip in mainMenuMoosic)
        {
            tempList.Add(clip);
        }
        while(randomlyGeneratedList.Count != mainMenuMoosic.Count)
        {
            if (tempList.Count > 1)
            {
                int songToAdd = Random.Range(0, tempList.Count);
                randomlyGeneratedList.Add(tempList[songToAdd]);
                tempList.RemoveAt(songToAdd);
            }
            else
            {
                randomlyGeneratedList.Add(tempList[0]);
            }
        }
    }
    void PlayRandomList()
    {
        if (currentTrackNumber >= randomlyGeneratedList.Count)
            currentTrackNumber = 0;
        StartCoroutine(PlayNextSong());
    }

    IEnumerator PlayNextSong()
    {
        audioSource.PlayOneShot(randomlyGeneratedList[currentTrackNumber]);
        yield return new WaitForSeconds(randomlyGeneratedList[currentTrackNumber].length);
        currentTrackNumber++;
        PlayRandomList();
    }
}
