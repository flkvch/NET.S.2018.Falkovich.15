using System;
using System.Collections;
using System.Collections.Generic;

namespace Collections
{
    public class Queue<T> : IEnumerable
    {
        private const int DEFAULT_SIZE = 4;
        private T[] array;
        private int head;
        private int version;

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// </summary>
        public Queue()
        {
            array = new T[DEFAULT_SIZE];
            Count = 0;
            version = 1;
            head = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException">capacity</exception>
        public Queue(int capacity) : this()
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(capacity)}  can't be less than null");
            }

            array = new T[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Queue(IEnumerable<T> collection) 
        {
            if (collection == null)
            {
                throw new ArgumentNullException($"Can't create a queue based on null {collection}.");
            }

            array = new T[DEFAULT_SIZE];
            IEnumerator enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Enqueue((T)enumerator.Current);
            }

            version = 1;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Enqueues the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Enqueue(T element)
        {
            if (Count == array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }

            T[] newArray = new T[array.Length];
            newArray[0] = element;

            for (int i = 0; i < Count; i++)
            {
                newArray[i + 1] = array[i];
            }

            array = newArray;
            head = Count;
            Count++;
            version++;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The queue is empty. Can't dequeue from empty queue.</exception>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty. Can't dequeue from empty queue.");
            }

            T element = array[head];
            array[head] = default(T);
            head--;
            version++;
            return element;
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The queue is empty. Can't dequeue from empty queue.</exception>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty. Can't dequeue from empty queue.");
            }

            return array[head];
        }

        /// <summary>
        /// Determines whether [contains] [the specified element].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified element]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T element)
        {
            for (int i = 0; i <= head; i++)
            {
                if (array[i].Equals(element))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Trims to actual size.
        /// </summary>
        public void TrimToActualSize()
        {
            Array.Resize(ref array, head + 1);
            Count = head + 1;
            version++;
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            //TrimToActualSize();
            return array;
        }

        #endregion

        #region Enumerator        
        /// <summary>
        /// Enumerator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="System.Collections.IEnumerable" />
        public class QueueEnumerator<T> : IEnumerator
        {
            private Queue<T> queue;
            private int currentIndex;
            private int version;
            private int head;

            internal QueueEnumerator(Queue<T> queue)
            {
                if (queue.head == 0)
                {
                    currentIndex = -1;
                }

                version = queue.version;
                currentIndex = -1; 
                this.queue = queue;
                head = queue.head;
            }

            public object Current
            {
                get
                {
                    if (currentIndex == -1 || currentIndex == queue.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return queue.array[currentIndex];
                }
            }

            public bool MoveNext()
            {
                if (queue.version != version)
                {
                    throw new InvalidOperationException();
                }

                if (currentIndex == -1)
                {
                    return false;
                }

                return ++currentIndex < queue.Count;
            }

            public void Reset()
            {
                if (queue.version != version)
                {
                    throw new InvalidOperationException();
                }

                currentIndex = -1;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new QueueEnumerator<T>(this);
        }
        #endregion
    }
}
