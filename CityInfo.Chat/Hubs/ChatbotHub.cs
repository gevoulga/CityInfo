using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CityInfo.Parking.Hubs
{
    public class ChatbotHub : Hub
    {
        private int counter = 0;
        private readonly IDictionary<int, IClientProxy> users = new ConcurrentDictionary<int, IClientProxy>();

        public async Task<Unit> Send(int userId, string message)
        {
            return await
                //Send the message to the recipient
                Observable.FromAsync((token) =>
                        users[userId].SendAsync("ReceiveMessage", message, token))
                    //And then...
                    .Concat(
                        //Notify about message sent
                        Observable.FromAsync((token) =>
                                Clients.Caller.SendAsync("Sent"))
                            //Don't bother if we failed to deliver a sent receipt - swallow exception
                            .Catch<Unit, Exception>(exception => Observable.Empty<Unit>())
                    )
                    //Failed to send message to recipient
                    .Catch<Unit, Exception>(exception =>
                        //Notify about message not sent
                        Observable.FromAsync((token) =>
                                Clients.Caller.SendAsync("Failed", exception))
                            //Don't bother if we failed to deliver a sent receipt - swallow exception
                            .Catch<Unit, Exception>(exception => Observable.Empty<Unit>())
                    )
                    .ToTask();
        }

        public override Task OnConnectedAsync()
        {
            users[Interlocked.Increment(ref counter)] = Clients.Caller;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var item in users.Where(kvp => kvp.Value == Clients.Caller))
            {
                users.Remove(item.Key);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}