using System;

namespace Command {
    class Program {
        static void Main(string[] args)
        {
            //  Section 6
            //  Client (user code) creates the commands
            ICommand approve = new ApproveCommand(new ManagerActionReceiver());
            ICommand reject = new RejectCommand(new ManagerActionReceiver());

            //  These commands are then plugged in for the invoker which is the framework code
            ApplicationInvoker invoker = new ApplicationInvoker(approve, reject);

            //  Emulating end-user interaction with the framework
            invoker.PressButton1();
            invoker.PressButton2();
        }
    }

    //  Section 1
    //  User code side
    //  Actions that the user code is capable of handling
    interface IManagerActions {
        void Approve();
        void Reject();
    }

    //  Section 2
    //  Action implementation
    //  Conventionally called a receiver
    class ManagerActionReceiver : IManagerActions {
        public void Approve() {
            Console.WriteLine("Manager: Application is approved");
        }

        public void Reject() {
            Console.WriteLine("Manager: Application is rejected");
        }
    }

    //  Section 3
    //  Command interface
    //  Client (framework code) uses this
    //  Framework doesn't know the actual handler of this request
    interface ICommand {
        void Execute();
    }

    //  Section 4
    //  Concrete command implementations
    //  Prepared by the user code and used by the client to plug in invoker
    class ApproveCommand : ICommand {
        private readonly IManagerActions _receiver;

        public ApproveCommand(IManagerActions receiver) {
            _receiver = receiver;
        }

        //  Delegate to known action of the receiver
        public void Execute() {
            _receiver.Approve();
        }
    }

    class RejectCommand : ICommand {
        private readonly IManagerActions _receiver;

        public RejectCommand(IManagerActions receiver) {
            _receiver = receiver;
        }

        public void Execute() {
            _receiver.Reject();
        }
    }

    //  Section 5
    //  This is the framework code
    //  Conventionally called invoker as it invokes the commands
    //  It doesn't handle the requests but delegates to the commands
    class ApplicationInvoker {
        private readonly ICommand _button1Command;
        private readonly ICommand _button2Command;

        public ApplicationInvoker(ICommand button1Command, ICommand button2Command) {
            _button1Command = button1Command;
            _button2Command = button2Command;
        }

        // delegation
        public void PressButton1() {
            _button1Command.Execute();
        }

        // delegation
        public void PressButton2() {
            _button2Command.Execute();
        }
    }
}
