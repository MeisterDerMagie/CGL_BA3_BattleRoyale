//(c) copyright by Martin M. Klöckener

namespace Doodlenite {
public interface INetworkEventHandler
{
    public void Send(string _serializedEvent);

    public void Receive(string _serializedEvent);
}
}