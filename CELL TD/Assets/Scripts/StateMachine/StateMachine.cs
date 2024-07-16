using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



/// <summary>
/// This state machine can be used for anything. You just need to override the State_Base class to make your own states for that thing.
/// </summary>
/// <remarks>
/// NOTE: Any class that uses this state machine should have a [RequireComponent(typeof(StateMachine))] attribute on it so that
///       the GameObject will automatically get a StateMachine component. Then the class that uses it can just call GetComponent<StateMachine>()
///       to grab a reference to it.
/// </remarks>
public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private IState _previousState;


    // This list holds all transitions that can happen from any state.
    private List<Transition> _fromAnyStateTransitions = new List<Transition>();

    // This dictionary holds all transitions that can only happen from a specified state.
    private Dictionary<string, List<Transition>> _fromStateTransitions = new Dictionary<string, List<Transition>>();

    // This dictionary holds all states that the state machine knows about from the transitions that have been set.
    private Dictionary<string, IState> _statesByNameLookup = new Dictionary<string, IState>();
    private Dictionary<Type, IState> _statesByTypeLookup = new Dictionary<Type, IState>();



    void Awake()
    {
        IsEnabled = true;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!IsEnabled)
            return;


        bool wasStateChanged = DoTransitionsCheck();

        _currentState?.OnUpdate();           
    }

    /// <summary>
    /// This function checks to see if a state machine transition should occur this frame or not.
    /// </summary>
    /// <returns>Returns true if a state machine transition was triggered, or false otherwise.</returns>
    private bool DoTransitionsCheck()
    {
        // First check transitions that can only occur in the current state.
        // If no matches are found, then check all transitions that can occur from any other state.
        bool transitionTriggered = DoSpecificStateTransitionsCheck();        
        return transitionTriggered ? transitionTriggered : DoAnyStateTransitionsCheck();
    }

    /// <summary>
    /// This function checks all transitions that can only occur in the current state to see if any of them return true.
    /// </summary>
    /// <returns>Returns true if a state machine transition was triggered, or false otherwise.</returns>
    private bool DoSpecificStateTransitionsCheck()
    {
        if (CurrentState == null ||
            !_fromStateTransitions.TryGetValue(CurrentState.Name, out List<Transition> transitions))
            return false;

        foreach (Transition t in transitions)
        {
            // Check that we are not already in this transition's target state, and that
            // the condition for this transition is true.
            if (CheckTransitionCondition(t))
            { 
                return true;
            }

        } // end foreach


        return false;
    }

    /// <summary>
    /// This function checks all transitions that can occur from any other state to see if any of them return true.
    /// </summary>
    /// <returns>Returns true if a state machine transition was triggered, or false otherwise.</returns>
    private bool DoAnyStateTransitionsCheck()
    {
        foreach (Transition t in _fromAnyStateTransitions)
        { 
            // Check that we are not already in this transition's target state, and that
            // the condition for this transition is true.
            if (CheckTransitionCondition(t))
            {
                return true;
            }

        } // end foreach


        return false;
    }
    
    /// <summary>
    /// This function simply calls the CheckTransitionCondition() delegate for the specified
    /// transition. If it returns true, then it will call ChangeState() to activate the transition.
    /// </summary>
    /// <param name="t">The transition whose condition is to be checked.</param>
    /// <returns>True if it triggered a state machine transition, and false otherwise.</returns>
    private bool CheckTransitionCondition(Transition t)
    {
        // Check that we are not already in this transition's target state, and that
        // the condition for this transition is true.
        if (_currentState.Name != t.ToState.Name &&
            t.CheckTransitionCondition())
        {
            return ChangeState(t.ToState);
        }


        return false;
    }

    /// <summary>
    /// Sets the state of the state machine. In practice the SetState() methods should be avoided outside of setting the starting state, as it is better to use the transitions to control state changes.
    /// </summary>
    /// <param name="state">The state to switch to.</param>
    /// <returns>True if the state machine has switched states successfully.</returns>
    public bool SetState(IState state)
    {
        if (state == null)
            throw new ArgumentNullException("The passed in state is null!");


        return ChangeState(state);
    }

    /// <summary>
    /// Sets the state of the state machine. In practice the SetState() methods should be avoided outside of setting the starting state, as it is better to use the transitions to control state changes.
    /// </summary>
    /// <param name="state">The C# class name of the state to switch to.</param>
    /// <param name="forceTransitionEvenIfAlreadyInState">If true, then a transition to the specified state will occurr even if it is already in that state.</param>
    /// <returns>True if the state machine has switched states successfully.</returns>
    public bool SetState(string stateClassName, bool forceTransitionEvenIfAlreadyInState = false)
    {
        if (string.IsNullOrWhiteSpace(stateClassName))
            throw new Exception("The passed in state name string is null or empty!");


        _statesByNameLookup.TryGetValue(stateClassName, out IState state);
        if (state != null)
        {
            return ChangeState(state, forceTransitionEvenIfAlreadyInState);
        }
        else
        {
            LogUnknownStateError(stateClassName);
            return false;
        }

    }

    /// <summary>
    /// Sets the state of the state machine. In practice the SetState() methods should be avoided outside of setting the starting state, as it is better to use the transitions to control state changes.
    /// </summary>
    /// <param name="state">The C# class type of the state to switch to.</param>
    /// <param name="forceTransitionEvenIfAlreadyInState">If true, then a transition to the specified state will occurr even if it is already in that state.</param>
    /// <returns>True if the state machine has switched states successfully.</returns>
    public bool SetState(Type stateType, bool forceTransitionEvenIfAlreadyInState = false)
    {
        if (stateType == null)
            throw new Exception("The passed in state type is null!");


        _statesByTypeLookup.TryGetValue(stateType, out IState state);
        if (state != null)
        {
            return ChangeState(state, forceTransitionEvenIfAlreadyInState);
        }
        else
        {
            LogUnknownStateError(stateType.GetType().Name);
            return false;
        }
    }

    /// <summary>
    /// Changes the state of the state machine.
    /// </summary>
    /// <param name="state">The state to switch to.</param>
    /// <param name="forceTransitionEvenIfAlreadyInState">If true, then a transition to the specified state will occurr even if it is already in that state.</param>
    /// <returns>True if the state machine has switched states successfully.</returns>
    private bool ChangeState(IState state, bool forceTransitionEvenIfAlreadyInState = false)
    {
        bool isInList = _statesByNameLookup.ContainsKey(state.Name);


        // If we are already in the specified state, then simply return unless forceTransitionEvenIfAlreadyInState is true.
        if (forceTransitionEvenIfAlreadyInState && _currentState == state)
            return false;


        // If enabled, then register the state if it is not in the list.
        if (!isInList && AllowUnknownStates)
        {
            AddStateToLookupTables(state);
            isInList = true;
        }


        // If the passed in state is in the list, then switch to that state.
        if (isInList)
        {
            // Cache the previous state.
            _previousState = _currentState;

            // Call the current state's OnExit() method.
            _currentState?.OnExit();

            if (EnableDebugLogging && _currentState != null)
                Debug.Log($"Exited state \"{CurrentState.Name}\".");

            // Set the current state to the new State.
            _currentState = state;
            // Call the new state's OnEnter() method.
            _currentState?.OnEnter();

            if (EnableDebugLogging)
               Debug.Log($"Entered state \"{state.Name}\".");

            return true;
        }
        else
        {
            LogUnknownStateError(state.Name);
            return false;
        }
    }

    /// <summary>
    /// Returns the state machine to its previous state if there is one.
    /// Does nothing if the current and previous states are the same.  
    /// </summary>
    /// <param name="reinitializeTheState">If true, then the state machine will call the new state's OnEnter() method, otherwise it won't.</param>
    /// <returns>Returns true if the state reverted successfully, or false otherwise.</returns>
    public bool ReturnToPreviousState(bool reinitializeTheState = true)
    {
        if (_previousState != null)
        {
            // Call the current state's OnExit() method.
            _currentState?.OnExit();

            // Swap the current and previous states.
            IState temp = _currentState;
            _currentState = _previousState;
            _previousState = temp;

            // If reinitializeTheState is true, then call the new state's OnEnter() method.
            if (reinitializeTheState)
                _currentState?.OnEnter();


            return true;
        }

        return false;
    }

    private void LogUnknownStateError(string stateName)
    {
        Debug.LogError($"State Machine could not switch to unknown state \"{stateName}\", because it is not known from any of the specified transitions! That means the state machine would be stuck in this state forever if this were allowed! If you are not using transitions, then enable StateMachine.AllowUnknownStates to disable this error.");
    }

    /// <summary>
    /// This function adds a transition from a specific state.
    /// </summary>
    /// <param name="fromState">The state this transition can occur in.</param>
    /// <param name="transition">A transition object that defines the state to transition into and the condition that must be met for this transition to occur.</param>
    /// <exception cref="Exception">ArgumentNullException</exception>
    public void AddTransitionFromState(IState fromState, Transition transition)
    {
        if (fromState == null)
            throw new ArgumentNullException("The passed in fromState is null!");
        if (transition == null)
            throw new ArgumentNullException("The passed in transition is null!");


        // Check if there is already a list for this from state in the dictionary. If not, create one.
        if (!_fromStateTransitions.ContainsKey(fromState.Name))
            _fromStateTransitions.Add(fromState.Name, new List<Transition>());

        // Add this transition into the appropriate list in the dictionary if it isn't already there.
        if (!_fromStateTransitions[fromState.Name].Contains(transition))
            _fromStateTransitions[fromState.Name].Add(transition);


        // Add the from state to the states list if it isn't already there.
        AddStateToLookupTables(fromState);

        // Add the transition's target state to the states list if it isn't already there.
        AddStateToLookupTables(transition.ToState);
    }

    /// <summary>
    /// This is the same as AddTransitionFromState() except that it lets you add a list of such transitions all in one function call.
    /// </summary>
    public void AddTransitionsFromState(params (IState fromState, Transition transition)[] transitionsInfo)
    {
        if (transitionsInfo == null || transitionsInfo.Length == 0)
            throw new ArgumentNullException("The passed in transition info array is null or empty!");


        for (int i = 0; i < transitionsInfo.Length; i++)
        {
            if (transitionsInfo[i].fromState == null || transitionsInfo[i].transition == null)
                Debug.LogError($"Skipping the transition at index {i} because the fromState, transition, or both are null!");

            AddTransitionFromState(transitionsInfo[i].fromState,
                                   transitionsInfo[i].transition);
        }
    }

    /// <summary>
    /// This function adds a transition that can occur from any other state.
    /// </summary>
    /// <param name="transition">A transition object that defines the state to transition into and the condition that must be met for this transition to occur.</param>
    /// <exception cref="Exception">ArgumentNullException</exception>
    public void AddTransitionFromAnyState(Transition transition)
    {
        if (transition == null)
            throw new Exception("The passed in transition is null!");


        if (!_fromAnyStateTransitions.Contains(transition))
            _fromAnyStateTransitions.Add(transition);


        // Add the transition's target state to the states list if it isn't already there.
        AddStateToLookupTables(transition.ToState);
    }

    /// <summary>
    /// This is the same as AddTransitionFromAnyState() except that it lets you add a list of such transitions all in one function call.
    /// </summary>
    public void AddTransitionsFromAnyState(params Transition[] transitions)
    {
        if (transitions == null || transitions.Length == 0)
            throw new ArgumentNullException("The passed in transitions array is null or empty!");


        for (int i = 0; i < transitions.Length; i++)
        {
            Transition transition = transitions[i];

            if (transition == null)
                Debug.LogError($"Skipping the transition at index {i} because it is null!");

            AddTransitionFromAnyState(transition);
        }
    }

    /// <summary>
    /// This method adds a state into the state machine without being attached to any transitions.
    /// A state added this way will never be entered unless you add transitions for it, or enter it
    /// manually via the SetState() method.
    /// </summary>
    /// <param name="state"></param>
    public void AddState(IState state)
    {
        if (state == null)
            throw new ArgumentNullException("The passed in state is null!");

        AddStateToLookupTables(state);
    }

    public void AddStates(params IState[] states)
    {
        if (states == null || states.Length == 0)
            throw new ArgumentNullException("The passed in states array is null or empty!");


        for (int i = 0; i < states.Length; i++)
        {
            IState state = states[i];

            if (state == null)
                Debug.LogError($"Skipping the state at index {i} because it is null!");

            AddStateToLookupTables(state);
        }
    }

    private void AddStateToLookupTables(IState state)
    {
        // Add the state to the states by name lookup table if it isn't already there.
        if (!_statesByNameLookup.ContainsKey(state.Name))
            _statesByNameLookup.Add(state.Name, state);

        // Add the state to the states by type lookup table if it isn't already there.
        if (!_statesByTypeLookup.ContainsKey(state.GetType()))
            _statesByTypeLookup.Add(state.GetType(), state);
    }



    /// <summary>
    /// If enabled, the SetState(IState state) method will let you set a state that is not already in the state machine's list,
    /// and in this case it will add it to the list. Normally, the state machine knows about states from all transitions that have
    /// been added. If you set a state that is not referenced by any transitions, then that means there is no way to leave said state
    /// other than by calling SetState() again. As such, this option should probably be left off most of the time.
    /// </summary>
    public bool AllowUnknownStates = false;

    /// <summary>
    /// Returns the current state.
    /// </summary>
    public IState CurrentState { get { return _currentState; } }

    /// <summary>
    /// If enabled, the state machine will write a debug log every time it enters or exits a state, including the name of that state.
    /// </summary>
    public bool EnableDebugLogging { get; set; }

    /// <summary>
    /// Setting this to false will disable the state machine. It will simply bail out at the start of Update() so no other code is called.
    /// </summary>
    public bool IsEnabled { get; private set; }
}
