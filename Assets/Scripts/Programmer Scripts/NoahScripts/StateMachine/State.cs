using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected FiniteStateMachine mStateMachine; // StateMachine accessed by states
    protected Entity mEntity; // the Enitity that is using this state
    //Constructor for State class
    public State(Entity mEntity, FiniteStateMachine mStateMachine)
    {
        this.mEntity = mEntity;
        this.mStateMachine = mStateMachine;
    }
    //called whenever state is entered
    public virtual void StateEnter()
    {
      
    }
    //called whenever state is exited
    public virtual void StateExit()
    {

    }
    //called by mEntity every update
    public virtual void LogicUpdate()
    {

    }
    //called by mEntity every fixed update
    public virtual void PhysicsUpdate()
    {

    }

}
