using System;
using System.Collections;
using System.Collections.Generic;

namespace Shared.Utils.Listing
{
    public class ActorList<T> : IEnumerable<T>
    {
        #region properties

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _list.Count)
                    throw new IndexOutOfRangeException();
                
                return _list[index];
            }
            set
            {
                if (index < 0 || index >= _list.Count)
                    throw new IndexOutOfRangeException();
                
                _list[index] = value;
            }
        }
        
        public int Count => _list.Count;

        #endregion
        
        #region constructors

        public ActorList()
        {
            _list = new List<T>();
        }
        
        public ActorList(params T[] list)
        {
            _list = new List<T>(list);
        }

        #endregion
        
        #region methodes

        public void UpdateList()
        {
            if (_toAdd.Count > 0)
            {
                foreach (var actor in _toAdd)
                {
                    if (!_list.Contains(actor))
                        _list.Add(actor);
                }
            }

            if (_toRemove.Count > 0)
            {
                foreach (var actor in _toRemove)
                {
                    _list.Remove(actor);
                }
            }
        }

        public void Add(T actor)
        {
            _toAdd.Add(actor);
        }

        public void Remove(T actor)
        {
            _toRemove.Add(actor);
        }
        
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region fields

        private readonly List<T> _list;

        private readonly HashSet<T> _toAdd = new();
        
        private readonly HashSet<T> _toRemove = new();

        #endregion
    }
}