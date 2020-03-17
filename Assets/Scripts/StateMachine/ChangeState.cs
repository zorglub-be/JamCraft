using System;

public class ChangeState
{
    public readonly IState From;
    public readonly IState To;
    public readonly Func<bool> Condition;
 
    public ChangeState(IState from, IState to,Func<bool> condition)
    {
        From = @from;
        To = to;
        Condition = condition;
    }
}