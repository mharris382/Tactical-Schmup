#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class StateMachine
{
    private readonly List<StateTransition> _anyStateTransitions = new List<StateTransition>();

    private IState _currentState;
    private readonly List<StateTransition> _stateTransitions = new List<StateTransition>();

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        var transition = new StateTransition(from, to, condition);
        _stateTransitions.Add(transition);
    }


    public void AddTransitions(IState to, Func<bool> condition, params IState[] states)
    {
        foreach (var state in states)
        {
            AddTransition(state, to, condition);
        }
    }

    public void AddAnyTransition(IState to, Func<bool> condition)
    {
        var transition = new StateTransition(null, to, condition);
        _anyStateTransitions.Add(transition);
    }

    public void Tick()
    {
        var transition = CheckForTransitions();

        if (transition != null)
        {
            SetState(transition.To);
        }
        
        _currentState.Tick();
    }

    public void FixedTick()
    {
        _currentState?.FixedTick();
    }

    public void SetState(IState state)
    {
        if (_currentState == state) return;
        
        _currentState?.OnStateExit();

        _currentState = state;
        
        Debug.LogWarning($"Current State: {_currentState}");
        
        _currentState.OnStateEnter();
    }

    private StateTransition CheckForTransitions()
    {
        foreach (var anyTransition in _anyStateTransitions)
        {
            if (anyTransition.Condition())
                return anyTransition;
        }

        foreach (var transition in _stateTransitions)
        {
            if (transition.From == _currentState && transition.Condition())
                return transition;
        }

        return null;
    }
}
