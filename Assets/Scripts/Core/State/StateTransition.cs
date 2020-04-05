#region

using System;

#endregion

public class StateTransition
{
    public StateTransition(IState from, IState to, Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }

    public IState From { get; }
    public IState To { get; }
    public Func<bool> Condition { get; }
}