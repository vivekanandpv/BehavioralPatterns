using System;

namespace TemplateMethod {
    class Program {
        static void Main(string[] args) {
            //  Client deals at the high-level interface
            OrderBase regularOrder = new RegularOrder();
            OrderBase premiumOrder = new PremiumOrder();

            //  Template method ensures high-level steps are followed
            //  but giving the freedom of implementation to implementing classes
            regularOrder.Process(); //  template method
            Console.WriteLine("-------------------------");
            premiumOrder.Process(); //  template method
        }
    }

    //  Section 1
    //  Define the interface for high-level steps
    abstract class OrderBase {
        //  High-level steps
        protected abstract void EnsurePayment();
        protected abstract void CheckStock();
        protected abstract void PackItems();
        protected abstract void ShipItems();

        //  Template method
        //  Ensure the execution of high-level steps in a predefined order
        public void Process() {
            EnsurePayment();
            CheckStock();
            PackItems();
            ShipItems();
        }
    }

    //  Section 2
    //  Implementers are forced to implement high-level methods
    //  But are free to choose their own implementation
    class PremiumOrder : OrderBase {
        protected override void CheckStock() {
            Console.WriteLine("PremiumOrder: CheckStock");
        }

        protected override void EnsurePayment() {
            Console.WriteLine("PremiumOrder: EnsurePayment");
        }

        protected override void PackItems() {
            Console.WriteLine("PremiumOrder: PackItems");
        }

        protected override void ShipItems() {
            Console.WriteLine("PremiumOrder: ShipItems");
        }
    }

    class RegularOrder : OrderBase {
        protected override void CheckStock() {
            Console.WriteLine("RegularOrder: CheckStock");
        }

        protected override void EnsurePayment() {
            Console.WriteLine("RegularOrder: EnsurePayment");
        }

        protected override void PackItems() {
            Console.WriteLine("RegularOrder: PackItems");
        }

        protected override void ShipItems() {
            Console.WriteLine("RegularOrder: ShipItems");
        }
    }
}
