using System;

namespace State {
    class Program {
        static void Main(string[] args)
        {
            //  Section 4
            //  Client works with the context and chooses a default state
            TcpConnection conntection = new TcpConnection(new TcpEstablished());

            //  Context object's behaviour is now defined by TcpEstablished state
            conntection.Open();
            conntection.Acknowledge();
            conntection.Close();

            //  Client can set the state dynamically
            conntection.State = new TcpListen();
            
            //  Context object now behaves as TcpListen
            conntection.Open();
            conntection.Acknowledge();
            conntection.Close();
        }
    }

    //  Section 1
    //  Define the interface for the State
    interface ITcpState
    {
        void Open();
        void Close();
        void Acknowledge();
    }

    //  Section 2
    //  Algorithms are encapsulated as objects
    class TcpEstablished:ITcpState
    {
        public void Open()
        {
            Console.WriteLine("TcpEstablished: Open");
        }

        public void Close()
        {
            Console.WriteLine("TcpEstablished: Close");
        }

        public void Acknowledge()
        {
            Console.WriteLine("TcpEstablished: Acknowledge");
        }
    }

    class TcpListen : ITcpState {
        public void Open() {
            Console.WriteLine("TcpListen: Open");
        }

        public void Close() {
            Console.WriteLine("TcpListen: Close");
        }

        public void Acknowledge() {
            Console.WriteLine("TcpListen: Acknowledge");
        }
    }

    class TcpClosed : ITcpState {
        public void Open() {
            Console.WriteLine("TcpClosed: Open");
        }

        public void Close() {
            Console.WriteLine("TcpClosed: Close");
        }

        public void Acknowledge() {
            Console.WriteLine("TcpClosed: Acknowledge");
        }
    }

    //  Section 3
    //  Conventionally called a Context
    class TcpConnection
    {
        //  Internal state, can be dynamically set
        public ITcpState State { get; set; }

        public TcpConnection(ITcpState state)
        {
            State = state;
        }

        //  Delegate the operations to the underlying state object
        //  Algorithm (logic) becomes state
        //  And the object behaviour is affected by the state
        public void Open()
        {
            State.Open();
        }

        public void Close()
        {
            State.Close();
        }

        public void Acknowledge()
        {
            State.Acknowledge();
        }
    }
}
