using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine 
{
    
    public State currentState { get; private set; } //state that the stateMachine is curently in. Updates as state changes

    // initalizes the starting state 
    public void InitializeStateMachine(State initialState)
    {
        currentState = initialState;
        currentState.StateEnter();
    }
    //exits the stand it is currently in, sets currentState to the passed state, enters the new state
    public void ChangeState(State mState)
    {
        //temp debug for testing. Will be removed in final version
        Debug.Log("Switching from " + currentState + " to" + mState);
        currentState.StateExit();
        currentState = mState;
        currentState.StateEnter();
    }
    // debug command that is called when an error occurs that causes a state to function incorrectly
    //TODO: create catch statements for potential permanant use
    public void ResetState(State resetState)
    {
        Debug.LogWarning(currentState + " error. Returning to default " + resetState + " state");
        currentState.StateExit();
        currentState = resetState;
        currentState.StateEnter();
    }


}
