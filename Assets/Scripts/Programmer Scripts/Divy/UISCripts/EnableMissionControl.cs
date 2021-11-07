using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMissionControl : MonoBehaviour
{
	[SerializeField]
	Animator missionControlOpen;

	[SerializeField]
	GameObject missionControl;

	private void Start()
	{
		enableMissionControl();
	}
	private void enableMissionControl()
	{
		missionControl.SetActive(true);
		missionControlOpen.enabled = true;
		missionControlOpen.SetBool("isOpen",true);
	}
}
