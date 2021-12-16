using System;
using System.Collections;
using System.Collections.Generic;

namespace Mediator {
    class Program {
        static void Main(string[] args) {
            //  Section 5
            //  Client works with the director
            WidgetDirectorBase mediator = new DialogDirector();

            //  Director sets up the colleagues
            mediator.CreateWidget();

            //  Demonstration of inter-colleague coordination
            mediator.DoAction();
        }
    }

    //  Section 1
    //  Defining the mediator interface
    //  Using an abstract class as the interface is stateful
    abstract class WidgetDirectorBase {
        //  Director maintains the internal state of colleagues
        protected Button _button;
        protected Dropdown _dropdown;

        //  Listener for colleague's changed event
        public abstract void WidgetChanged(WidgetBase widget);

        //  Director is responsible for housing the colleagues
        public void CreateWidget() {
            _button = new Button(this) { Text = "Default Button", IsDisabled = false };
            _dropdown = new Dropdown(this) { Text = "Default Dropdown" };
        }

        //  For demonstration
        public abstract void DoAction();
    }

    //  Section 2
    //  Concrete mediator
    class DialogDirector : WidgetDirectorBase {
        //  The crux: the listener mediates the object coordination
        //  The WidgetDirectorBase is stateful precisely because of this reason
        //  Consider how we toggle the button's disabled status based on dropdown selection
        public override void WidgetChanged(WidgetBase widget) {
            if (widget == _button) {
                Console.WriteLine($"Director: button -> {widget}");
            } else if (widget == _dropdown) {
                Console.WriteLine($"Director: Dropdown -> {widget}");
                _button.IsDisabled = _dropdown.SelectedIndex % 2 == 0;
            }
        }

        //  Some typical interactions
        public override void DoAction() {
            _button.Text = "Changing the button text";
            _dropdown.SelectedIndex = 2;
            _dropdown.SelectedIndex = 3;
        }
    }

    //  Section 3
    //  Defining the interface for colleagues
    //  Using an abstract class as the interface is stateful
    abstract class WidgetBase {
        //  Colleagues use the mediator for intercommunication
        private readonly WidgetDirectorBase _mediator;

        protected WidgetBase(WidgetDirectorBase mediator) {
            _mediator = mediator;
        }

        //  Colleagues in a declarative manner notify the mediator of their
        //  specific state change
        public void Changed() {
            //  Let the mediator handle the coordination if any
            _mediator.WidgetChanged(this);
        }
    }

    //  Section 4
    //  A concrete colleague
    class Dropdown : WidgetBase {
        //  Specific state of the colleague
        private string _text = "";
        private int _selectedIndex = 0;
        private IList<string> _items = new List<string>();
        public string Text {
            get => _text;
            set {
                _text = value;
                Changed();  //  notify the state change
            }
        }

        public int SelectedIndex {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                Changed();
            }
        }

        public IList<string> Items {
            get => _items;
            set {
                _items = value;
                Changed();
            }
        }

        public Dropdown(WidgetDirectorBase mediator) : base(mediator) {
        }

        //  For ease of demonstration
        public override string ToString() {
            return $"Dropdown: {SelectedIndex} -> {Text}";
        }
    }

    class Button : WidgetBase {
        private string _text = "";
        private bool _isDisabled = false;
        public string Text {
            get => _text;
            set {
                _text = value;
                Changed();
            }
        }


        public bool IsDisabled {
            get => _isDisabled;
            set {
                _isDisabled = value;
                Changed();
            }
        }

        public Button(WidgetDirectorBase mediator) : base(mediator) {
        }

        public override string ToString() {
            return $"Button: {Text} -> Disabled: {IsDisabled}";
        }
    }
}
