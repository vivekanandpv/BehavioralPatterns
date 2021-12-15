using System;
using System.Collections.Generic;

namespace Observer {
    class Program {
        static void Main(string[] args) {
            //  Section 5
            //  First the client creates the subject (publisher)
            IPublisher<double> pos = new PointOfSale();

            //  Subsequent observers (subscribers) are instantiated with the subject created
            //  Subscribers (observers) take care of registering themselves
            ISubscriber<double> fd = new FraudDetection(pos);
            ISubscriber<double> rc = new RegulatoryCompliance(pos);

            //  Business goes on with the publisher
            //  Subscribers get a delegated notification in a decoupled way
            pos.SetState(124.25);
            pos.SetState(4741.25);
        }
    }

    //  Section 1
    //  Define the Subject interface (aka publisher)
    //  Can also be defined as an abstract class
    interface IPublisher<T> {
        void Attach(ISubscriber<T> observer);
        void Detach(ISubscriber<T> observer);
        void Notify();
        T GetState();
        void SetState(T state);
    }

    //  Section 2
    //  Define the observer interface (aka subscriber)
    //  Can also be defined as an abstract class
    interface ISubscriber<T> {
        void Update();
    }

    //  Section 3
    //  Concrete publisher
    class PointOfSale : IPublisher<double> {
        //  Invocation list
        private readonly IList<ISubscriber<double>> _subscribers = new List<ISubscriber<double>>();

        //  The idea is to notify the subscribers on state change
        private double _state = 0;

        //  Observers register themselves here
        public void Attach(ISubscriber<double> observer) {
            _subscribers.Add(observer);
        }

        //  Observers can deregister
        public void Detach(ISubscriber<double> observer) {
            _subscribers.Remove(observer);
        }

        //  Kick off the notification process
        public void Notify() {
            //  iterate over the invocation list and invoke the delegates on each observer
            foreach (var subscriber in _subscribers) {
                subscriber.Update();
            }
        }

        //  Observers use this to know what the current state is
        public double GetState() {
            return _state;
        }

        //  Exposed API for state change
        public void SetState(double state) {
            _state = state;
            Notify();   //  state change kicks off the notification process
        }
    }

    //  Section 4
    //  Concrete observer (aka subscriber)
    class FraudDetection : ISubscriber<double> {
        //  To register and to get the state, observer maintains the dependency to the publisher (subject)
        private readonly IPublisher<double> _publisher;

        //  dependency inversion
        public FraudDetection(IPublisher<double> publisher) {
            _publisher = publisher;
            _publisher.Attach(this);
        }

        //  This is the delegated method
        //  called by subject or publisher on notify
        public void Update() {
            //  There are variants where the update is called with the new state
            //  Event handlers are implemented that way
            //  This is the classic approach of GoF
            Console.WriteLine($"FraudDetection Notified: State -> {_publisher.GetState()}");
        }
    }

    class RegulatoryCompliance : ISubscriber<double> {
        private readonly IPublisher<double> _publisher;

        public RegulatoryCompliance(IPublisher<double> publisher) {
            _publisher = publisher;
            _publisher.Attach(this);
        }

        public void Update() {
            Console.WriteLine($"RegulatoryCompliance Notified: State -> {_publisher.GetState()}");
        }
    }
}
