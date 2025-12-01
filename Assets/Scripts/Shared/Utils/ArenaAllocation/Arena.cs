using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Shared.Utils.ArenaAllocation
{
    public unsafe class Arena : IDisposable
    {
        #region constructors

        public Arena(int byteSize)
        {
            _buffer = (byte*)UnsafeUtility.Malloc(byteSize, 16, Allocator.Persistent);
            _offset = 0;
            _capacity = byteSize;
        }

        #endregion

        #region methodes

        public T* Alloc<T>(int count = 1) where T : unmanaged
        {
            var size = UnsafeUtility.SizeOf<T>() * count;
            if (_offset + size > _capacity)
                throw new Exception("Allocation overflow");
            
            var ptr = (T*)(_buffer + _offset);
            _offset += size;
            return ptr;
        }

        public void Reset() => _offset = 0;
        
        public void Dispose()
        {
            if (_buffer is null)
                return;
            
            UnsafeUtility.Free(_buffer, Allocator.Persistent);
            _buffer = null;
        }
        
        #endregion

        #region fields

        private byte* _buffer;

        private int _offset;
        
        private readonly int _capacity;

        #endregion
    }
}