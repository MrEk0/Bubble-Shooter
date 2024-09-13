using System.Collections.Generic;
using Interfaces;

namespace Game
{
    public class GameSubscriber
    {
        private readonly List<ISubscribable> _subscribers = new();

        public void AddListener(ISubscribable subscriber)
        {
            subscriber.Subscribe();
            _subscribers.Add(subscriber);
        }

        public void RemoveListener(ISubscribable subscriber)
        {
            subscriber.Unsubscribe();
            _subscribers.Remove(subscriber);
        }

        public void RemoveAll()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Unsubscribe();
            }

            _subscribers.Clear();
        }
    }
}