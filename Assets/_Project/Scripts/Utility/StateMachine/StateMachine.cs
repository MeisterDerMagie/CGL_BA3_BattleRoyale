//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Wichtel.TimeControl;

//https://www.youtube.com/watch?v=V75hgcsCGOM
//(https://forum.unity.com/threads/c-proper-state-machine.380612/)
namespace Wichtel.StateMachine {
public class StateMachine
{
    public IState currentState { get; private set; }
    public string currentStateName => currentState == null ? "NONE" : currentState.GetType().Name;
   
    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type,List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();
   
    private static List<Transition> emptyTransitions = new List<Transition>(0);

    private bool pauseWithGameTime;

    public StateMachine(bool _pauseWithGameTime = false)
    {
        pauseWithGameTime = _pauseWithGameTime;
        
        //run state machine
        //Timing.RunCoroutine(Tick());
    }

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null) SetState(transition.To);

        if (pauseWithGameTime && GameTime.GameIsPaused) return; //don't tick if game is paused
        currentState?.Tick();
    }

    public void SetState(IState _state)
    {
        if (_state == currentState)
            return;
      
        currentState?.OnExit();
        currentState = _state;
      
        transitions.TryGetValue(currentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = emptyTransitions;
      
        currentState.OnEnter();
    }

    public void AddTransition(IState _from, IState _to, Func<bool> _predicate)
    {
        if (this.transitions.TryGetValue(_from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            this.transitions[_from.GetType()] = transitions;
        }
      
        transitions.Add(new Transition(_to, _predicate));
    }
    
    public void AddTransition(IState _from, IState _to, ref Action _trigger)
    {
        if (this.transitions.TryGetValue(_from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            this.transitions[_from.GetType()] = transitions;
        }
        
        transitions.Add(new Transition(_to, ref _trigger));
    }

    public void AddAnyTransition(IState _state, Func<bool> _predicate)
    {
        anyTransitions.Add(new Transition(_state, _predicate));
    }
    
    public void AddAnyTransition(IState _state, ref Action _trigger)
    {
        anyTransitions.Add(new Transition(_state, ref _trigger));
    }

    private class Transition
    {
        public Func<bool> Condition {get; }
        public IState To { get; }

        private bool Triggered() => triggered;
        private bool triggered = false;
        public void ResetTrigger() => triggered = false;
        public Transition(IState _to, Func<bool> _condition)
        {
            To = _to;
            Condition = _condition;
        }
        
        public Transition(IState _to, ref Action _trigger)
        {
            To = _to;
            _trigger += onTrigger;
            Condition = Triggered;
        }

        private void onTrigger()
        {
            triggered = true;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.Condition())
            {
                transition.ResetTrigger();
                return transition;
            }
        }
        
        foreach (var transition in currentTransitions)
        {
            if (transition.Condition())
            {
                transition.ResetTrigger();
                return transition;
            }
        }

        return null;
    }
}
}