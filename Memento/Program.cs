using System;
using System.Collections.Generic;

namespace Memento {
    class Program {
        static void Main(string[] args) {
            //  Section 4
            //  Client instantiates the caretaker
            Caretaker caretaker = new Caretaker();

            //  Carries on with the originator
            DocumentOriginator doc = new DocumentOriginator { Title = "Initial Title", Body = "Initial Body" };

            //  To save a snapshot of the state
            caretaker.Save(doc);

            caretaker.Undo(doc); // No effect
            Console.WriteLine(doc);

            doc.Title = "New Title 1";
            caretaker.Save(doc);

            Console.WriteLine(doc); //  New Title 1

            doc.Body = "New body 1";
            Console.WriteLine(doc);

            //  Restore to previously saved state
            caretaker.Undo(doc);  //  New Title 1

            Console.WriteLine(doc);
        }
    }

    //  Section 1
    //  Originator
    //  Can be abstracted as an interface
    class DocumentOriginator {
        public string Title { get; set; }
        public string Body { get; set; }

        //  Creates the memento of current state
        public DocumentMemento CreateMemento() {
            return new DocumentMemento(Title, Body);
        }

        //  Restores the state from memento
        public void SetMemento(DocumentMemento memento) {
            Title = memento.Title;
            Body = memento.Body;
        }

        public override string ToString() {
            return $"Title: {Title} {Environment.NewLine}Body: {Body} {Environment.NewLine}-------------------------------------";
        }
    }

    //  Section 2
    //  Memento
    //  Encapsulates the requisite state of the originator
    class DocumentMemento {
        public string Title { get; }
        public string Body { get; }

        public DocumentMemento(string title, string body) {
            Title = title;
            Body = body;
        }
    }

    //  Section 3
    //  Caretaker as a registry of state snapshots of mementos of originator
    class Caretaker {
        //  Stack is a natural choice for providing undo operation
        private readonly Stack<DocumentMemento> _history = new Stack<DocumentMemento>();

        public void Save(DocumentOriginator document) {
            _history.Push(document.CreateMemento());
        }

        public void Undo(DocumentOriginator document) {
            if (_history.Count > 0) {
                document.SetMemento(_history.Pop());
            }
        }
    }
}
