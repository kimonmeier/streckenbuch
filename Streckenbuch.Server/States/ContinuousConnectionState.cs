using Google.Protobuf;
using MediatR;
using Streckenbuch.Shared.Services;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Streckenbuch.Server.States;

public class ContinuousConnectionState
{
    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<IRequest<Unit>>> _messageQeue = new();
    private readonly ConcurrentDictionary<Guid, int> _registeredTrains = new();

    public IReadOnlyList<Guid> GetRegisteredClients()
    {
        return _messageQeue.Keys.ToList().AsReadOnly();
    } 
    
    public ReadOnlyDictionary<Guid, int> GetRegisteredTrainOperator()
    {
        return _registeredTrains.AsReadOnly();
    }
    
    public void RegisterTrain(Guid clientId, int trainNumber)
    {
        if (_registeredTrains.TryAdd(clientId, trainNumber))
        {
            return;
        }

        throw new Exception("Something went wrong");
    }

    public void UnregisterTrain(Guid clientId)
    {
        if (_registeredTrains.TryRemove(clientId, out _))
        {
            return;
        }

        throw new Exception("Something went wrong");
    }

    public void EnqueueMessageTrain(int trainNumber, IRequest<Unit> request)
    {
        foreach (Guid clientId in _registeredTrains.Where(x => x.Value == trainNumber).Select(x => x.Key))
        {
            EnqueueMessageClient(clientId, request);
        }
    }

    public void EnqueueMessageClient(Guid clientId, IRequest<Unit> request)
    {
        _messageQeue[clientId].Enqueue(request);
    }
    
    public List<Message> GetMessagesInQueue(Guid clientId)
    {
        if (!_messageQeue.ContainsKey(clientId))
        {
            if (!_messageQeue.TryAdd(clientId, new ConcurrentQueue<IRequest<Unit>>()))
            {
                throw new Exception("Something went wrong");
            }
        }
        _messageQeue.TryGetValue(clientId, out ConcurrentQueue<IRequest<Unit>>? queue);
        

        List<Message> messages = new();

        while (queue!.TryDequeue(out IRequest<Unit>? request))
        {
            if (request is null)
            {
                throw new Exception("Something went wrong");
            }

            Message message = new Message()
            {
                Type = request.GetType().FullName, Data = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes(request))
            };

            messages.Add(message);
        }

        return messages;
    }
}