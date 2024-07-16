using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class StateMachineTester : MonoBehaviour
{
    StateMachine _stateMachine;

    public int Value = 0;


    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();

        State_TestStateA stateA = new State_TestStateA(this.gameObject);
        State_TestStateB stateB = new State_TestStateB(this.gameObject);
        State_TestStateC stateC = new State_TestStateC(this.gameObject);

        Transition aToB = new Transition(stateB, TestCondition1);
        Transition bToC = new Transition(stateC, TestCondition2);
        Transition anyToA = new Transition(stateA, TestCondition0);

        _stateMachine.AddTransitionFromState(stateA, aToB);
        _stateMachine.AddTransitionFromState(stateB, bToC);

        _stateMachine.AddTransitionFromAnyState(anyToA);

        _stateMachine.SetState(stateA);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
            Value = 0;
        else if (Input.GetKeyUp(KeyCode.Alpha1))
            Value = 1;
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            Value = 2;
    }

    public bool TestCondition0()
    {
        return Value == 0;
    }

    public bool TestCondition1()
    {
        return Value == 1;
    }

    public bool TestCondition2()
    {
        return Value == 2;
    }

}

public class State_TestStateA : State_Base
{
    public State_TestStateA(GameObject parent)
        : base(parent)
    {

    }

}

public class State_TestStateB : State_Base
{
    public State_TestStateB(GameObject parent)
        : base(parent)
    {

    }

}

public class State_TestStateC : State_Base
{
    public State_TestStateC(GameObject parent)
        : base(parent)
    {

    }

}