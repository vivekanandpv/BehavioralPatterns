using System;
using System.Collections;
using System.Collections.Generic;

namespace Iterator {
    internal class Program {
        static void Main(string[] args) {
            //  Section 5
            //  Client creates an aggregate
            ISequence<int> seq = new SequenceList<int>();

            //  For demonstration
            seq.PushData(new List<int> { 1, 4, 7, 8, 5, 2, 3, 6, 9 });

            //  Get the iterator from the aggregate
            IIterator<int> iterator = seq.GetIterator();

            //  Iteration
            while (!iterator.IsDone()) {
                Console.WriteLine(iterator.CurrentItem());
                iterator.Next();
            }


            //  Framework implementation
            IEnumerable<string> cities = new List<string> {
                "Mumbai",
                "Bengaluru",
                "Delhi",
                "Rajkot"
            };

            //  IEnumerator implements the IDisposable
            //  So wrapping in using statement
            using (IEnumerator<string> enumerator = cities.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    Console.WriteLine(enumerator.Current);
                }
            }
        }
    }

    //  Section 1
    //  Provide an independent interface for iteration algorithm
    interface IIterator<out T> {
        void First();
        void Next();
        bool IsDone();
        T CurrentItem();
    }

    //  Section 2
    //  Conventionally called an aggregate
    //  The idea is to separate the sequence from iteration
    //  Client needs to work with both
    //  So, the sequence is coupled with the iteration
    interface ISequence<T> {
        IIterator<T> GetIterator();
        T Get(int index);
        int Size();
        void PushData(IEnumerable<T> collection);   //  for demonstration
    }

    //  Section 3
    //  Concrete iterator implementation
    class ListIterator<T> : IIterator<T> {
        //  holds the delegated object to the sequence
        private readonly ISequence<T> _sequence;

        //  control index
        private int _index = 0;

        //  A sequence passes itself
        public ListIterator(ISequence<T> sequence) {
            _sequence = sequence;
        }

        public void First() {
            _index = 0;
        }

        public void Next() {
            ++_index;
        }

        public bool IsDone() {
            return _index >= _sequence.Size();
        }

        public T CurrentItem() {
            return _sequence.Get(_index);
        }
    }

    //  Section 4
    //  Concrete aggregate implementation
    class SequenceList<T> : ISequence<T> {
        //  For demonstration I am using a standard list
        private readonly List<T> _list = new List<T>();

        //  Chooses its own iterator and that iterator object is coupled with this sequence
        public IIterator<T> GetIterator() {
            return new ListIterator<T>(this);
        }

        //  list access API
        public T Get(int index) {
            return _list[index];
        }

        public int Size() {
            return _list.Count;
        }

        //  Convenience
        public void PushData(IEnumerable<T> collection) {
            _list.AddRange(collection);
        }
    }
}
