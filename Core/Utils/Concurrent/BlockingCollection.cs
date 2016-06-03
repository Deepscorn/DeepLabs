using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Sources.Util.Concurrent
{
    // based on System.Collections.Concurrent.BlockingCollection
    // adopted to project's needs
    public class BlockingCollection<T>
    {
        private readonly Queue<T> underlyingColl = new Queue<T>();
        private readonly object syncRoot = new object();
        private bool isTakingCompleted;

        public void Add(T item)
        {
            lock (syncRoot)
            {
                underlyingColl.Enqueue(item);
                Monitor.Pulse(syncRoot);
            }
        }

        // returns false if not taken (if TakingCompleted)
        public bool TryTake(out T item)
        {
            lock (syncRoot)
            {
                while (underlyingColl.Count == 0 && !isTakingCompleted)
                {
                    // wait for any insertion
                    Monitor.Wait(syncRoot);
                }
                if (isTakingCompleted)
                {
                    item = default(T);
                    return false;
                }
                item = underlyingColl.Dequeue();
                return true;
            }
        }

        public IEnumerable<T> GetConsumingEnumerable()
        {
            T item;
            while (TryTake(out item))
            {
                yield return item;
            }
        }

        public void CompleteTaking()
        {
            isTakingCompleted = true;
            lock (syncRoot)
            {
                Monitor.PulseAll(syncRoot);
            }
        }

        public void StartTakingAgainIfCompleted()
        {
            isTakingCompleted = false;
        }

        public bool IsTakingCompleted()
        {
            return isTakingCompleted;
        }
    }
}
