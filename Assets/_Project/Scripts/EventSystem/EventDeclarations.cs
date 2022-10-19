//(c) copyright by Martin M. Klöckener
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Wichtel;

namespace Doodlenite{
public class Event
{
    private Action eventAction = delegate {  };
    private string eventName;
    
    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action function) => eventAction += function;
    public void Unsubscribe(Action function) => eventAction -= function;
    public void Invoke() => Invoke(false);
    public void Invoke(bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke();

        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{});
    }
}

public class Event<T1>
{
    private Action<T1> eventAction = delegate(T1 _obj1) {  };
    private string eventName;

    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action<T1> function) => eventAction += function;
    public void Unsubscribe(Action<T1> function) => eventAction -= function;
    public void Invoke(T1 _param1) => Invoke(_param1, false);
    public void Invoke(T1 _param1, bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke(_param1);
        
        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{_param1});
    }
}

public class Event<T1, T2>
{
    private Action<T1, T2> eventAction = delegate(T1 _obj1, T2 _obj2) {  };
    private string eventName;

    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action<T1, T2> function) => eventAction += function;
    public void Unsubscribe(Action<T1, T2> function) => eventAction -= function;
    public void Invoke(T1 _param1, T2 _param2) => Invoke(_param1, _param2, false);
    public void Invoke(T1 _param1, T2 _param2, bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke(_param1, _param2);
        
        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{_param1, _param2});
    }
}

public class Event<T1, T2, T3>
{
    private Action<T1, T2, T3> eventAction = delegate(T1 _obj1, T2 _obj2, T3 _obj3) {  };
    private string eventName;

    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action<T1, T2, T3> function) => eventAction += function;
    public void Unsubscribe(Action<T1, T2, T3> function) => eventAction -= function;
    public void Invoke(T1 _param1, T2 _param2, T3 _param3) => Invoke(_param1, _param2, _param3, false);
    public void Invoke(T1 _param1, T2 _param2, T3 _param3, bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke(_param1, _param2, _param3);
        
        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{_param1, _param2, _param3});
    }
}

public class Event<T1, T2, T3, T4>
{
    private Action<T1, T2, T3, T4> eventAction = delegate(T1 _obj1, T2 _obj2, T3 _obj3, T4 _obj4) {  };
    private string eventName;

    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action<T1, T2, T3, T4> function) => eventAction += function;
    public void Unsubscribe(Action<T1, T2, T3, T4> function) => eventAction -= function;
    public void Invoke(T1 _param1, T2 _param2, T3 _param3, T4 _param4) => Invoke(_param1, _param2, _param3, _param4, false);
    public void Invoke(T1 _param1, T2 _param2, T3 _param3, T4 _param4, bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke(_param1, _param2, _param3, _param4);
        
        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{_param1, _param2, _param3, _param4});
    }
}

public class Event<T1, T2, T3, T4, T5>
{
    private Action<T1, T2, T3, T4, T5> eventAction = delegate(T1 _obj1, T2 _obj2, T3 _obj3, T4 _obj4, T5 _obj5) {  };
    private string eventName;

    public Event(string _eventName) => eventName = _eventName;
    public void Subscribe(Action<T1, T2, T3, T4, T5> function) => eventAction += function;
    public void Unsubscribe(Action<T1, T2, T3, T4, T5> function) => eventAction -= function;
    public void Invoke(T1 _param1, T2 _param2, T3 _param3, T4 _param4, T5 _param5) => Invoke(_param1, _param2, _param3, _param4, _param5, false);
    public void Invoke(T1 _param1, T2 _param2, T3 _param3, T4 _param4, T5 _param5, bool invokeNetworkEvent = false)
    {
        //invoke local event
        eventAction?.Invoke(_param1, _param2, _param3, _param4, _param5);
        
        if (!invokeNetworkEvent) return;
        //invoke network event
        NetworkEvents.Invoke(eventName, new object[]{_param1, _param2, _param3, _param4, _param5});
    }
}

// need more generic parameters? Just copy paste the last class and add more...

}