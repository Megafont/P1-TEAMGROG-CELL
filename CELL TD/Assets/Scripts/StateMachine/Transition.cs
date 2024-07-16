using System.Collections;
using System.Collections.Generic;

using UnityEngine;



/// <summary>
/// This delegate is used when calling a state machine state's constructor to
/// tell it what function to call to check its transition condition.
/// </summary>
/// <returns>True if the associated condition is true.</returns>
public delegate bool StateMachineConditionCheck();


/// <summary>
/// This class defines a transition in the state machine.
/// </summary>
public class Transition
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="toState">The state that this transition will move the state machine into.</param>
    /// <param name="conditionCheckDelegate">The delegate that checks the condition that's required for this transition to occur.</param>
    public Transition(IState toState, StateMachineConditionCheck conditionCheckDelegate)
    {
        ToState = toState;
        CheckTransitionCondition = conditionCheckDelegate;
    }



    public StateMachineConditionCheck CheckTransitionCondition { get; private set; }
    public IState ToState { get; private set; }
}
