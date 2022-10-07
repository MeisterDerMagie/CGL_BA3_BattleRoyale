//(c) copyright by Martin M. Klöckener

using System;

namespace Doodlenite {
public abstract class Command<T>
{
    protected T target;

    protected Command(T target)
    {
        this.target = target;
    }

    public abstract void Execute();
}
}