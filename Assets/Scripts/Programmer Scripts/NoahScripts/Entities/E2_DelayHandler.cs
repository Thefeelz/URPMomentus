using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DelayHandler : MonoBehaviour
{
    public Enemy2 mEnemy { get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        mEnemy = this.gameObject.GetComponent<Enemy2>();
    }

    //calls coroutine with passed parameters. States are not monobehavior so they can not call the coroutine directly
    public void callCoroutine(string sName, float fTime)
    {
        StartCoroutine(stateSwitch(sName, fTime));
    }

    //uses a string and a float with a switch statement to change to the state of the passed name, after fTime, if conditions are met
    IEnumerator stateSwitch(string sName, float fTime)
    {
        yield return new WaitForSeconds(fTime);
        switch (sName)
        {
            case "attackState":
                if (mEnemy.stateMachine.currentState == mEnemy.slowState)
                {
                    //raycast will determine if the enemy is phasing through a wall / out of bounds
                    RaycastHit hit;
                    Vector3 point = mEnemy.myTarget.transform.position - mEnemy.transform.position;
                    // enemy will only jump if it is not currently phasing through a wall. Otherwise it will generate a new random time
                    if (Physics.Raycast(transform.position, point, out hit, Mathf.Infinity) && hit.transform.tag != "Walls")
                    {
                        mEnemy.stateMachine.ChangeState(mEnemy.attackState);
                    }
                    // if the enemy is currently phasing through a wall, a new random amount of time is chosen, afterwhich the check is attempted again
                    else
                    {
                        mEnemy.slowData.circleStart = false;
                    }
                }
                break;

            case "slowState":
                mEnemy.stateMachine.ChangeState(mEnemy.slowState);
                break;
            case "moveState":
                mEnemy.stateMachine.ChangeState(mEnemy.moveState);
                break;
        }
    }
}
