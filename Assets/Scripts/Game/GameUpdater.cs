using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Game
{
    public class GameUpdater : MonoBehaviour, IServisable
    {
        private readonly List<IGameUpdatable> _updateListeners = new();

        public void AddListener(IGameUpdatable listener)
        {
            _updateListeners.Add(listener);
        }

        public void RemoveListener(IGameUpdatable listener)
        {
            if (!_updateListeners.Contains(listener))
                return;
            
            _updateListeners.Remove(listener);
        }

        public void RemoveAll()
        {
            _updateListeners.Clear();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (var i = 0; i < _updateListeners.Count; i++)
            {
                _updateListeners[i].OnUpdate(deltaTime);
            }
        }
    }
}

