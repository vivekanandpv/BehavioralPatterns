using System;
using System.Collections.Generic;

namespace Visitor {
    class Program {
        static void Main(string[] args) {
            //  Section 5
            //  Client can either work with Composite or collection of nodes (aka elements)
            IList<IEquipment> equipments = new List<IEquipment>
            {
                new Computer {Name = "Laptop", PowerConsumption = 90, RegularPrice=50000},
                new Printer {Name ="Laser Printer", PowerConsumption = 2500, RegularPrice = 25000},
                new AirConditioner {Name = "Inverter AC", PowerConsumption = 1500, RegularPrice = 35000}
            };

            //  Client chooses the visitor
            //  Please notice the concrete instances of visitors here
            //  This is to retrieve stateful logic
            //  Consider interface here if no stateful logic is employed
            PriceVisitor priceVisitor = new PriceVisitor();
            PowerConsumptionVisitor consumptionVisitor = new PowerConsumptionVisitor();

            //  Applying the visitor to a collection
            //  Composite takes the visitor directly
            foreach (var equipment in equipments) {
                equipment.Accept(priceVisitor);
                equipment.Accept(consumptionVisitor);
            }

            //  Retrieving the state from applied visitors
            Console.WriteLine($"Total price: {priceVisitor.TotalPrice}");
            Console.WriteLine($"Total power consumption: {consumptionVisitor.TotalPowerConsumption}");
        }
    }

    //  Section 1
    //  Conventionally called an Element
    //  Defines the interface for the node in a system
    //  Consider implementing Composite here
    interface IEquipment {
        string Name { get; }
        int PowerConsumption { get; }
        int RegularPrice { get; }
        void Accept(IEquipmentVisitor visitor);
    }

    //  Section 2
    //  Conventionally called a Visitor
    //  Defines the algorithm for each of the concrete node separately
    interface IEquipmentVisitor {
        void VisitComputer(Computer computer);
        void VisitPrinter(Printer printer);
        void VisitAirConditioner(AirConditioner airConditioner);
    }

    //  Section 3
    //  Concrete elements
    //  Nodes are decoupled from the visitor logic
    class Computer : IEquipment {
        public string Name { get; set; }
        public int PowerConsumption { get; set; }
        public int RegularPrice { get; set; }
        public void Accept(IEquipmentVisitor visitor) {
            visitor.VisitComputer(this);
        }
    }

    class Printer : IEquipment {
        public string Name { get; set; }
        public int PowerConsumption { get; set; }
        public int RegularPrice { get; set; }
        public void Accept(IEquipmentVisitor visitor) {
            visitor.VisitPrinter(this);
        }
    }

    class AirConditioner : IEquipment {
        public string Name { get; set; }
        public int PowerConsumption { get; set; }
        public int RegularPrice { get; set; }
        public void Accept(IEquipmentVisitor visitor) {
            visitor.VisitAirConditioner(this);
        }
    }

    //  Section 4
    //  Concrete Visitor
    //  Often stateful; employs the algorithm to run on each node
    class PriceVisitor : IEquipmentVisitor {
        public int TotalPrice { get; private set; } = 0;

        //  Node specific algorithms
        //  Usually the algorithms follow the same theme (price calculation in this example)
        public void VisitComputer(Computer computer) {
            TotalPrice += computer.RegularPrice;
        }

        public void VisitPrinter(Printer printer) {
            TotalPrice += printer.RegularPrice;
        }

        public void VisitAirConditioner(AirConditioner airConditioner) {
            TotalPrice += airConditioner.RegularPrice;
        }
    }

    class PowerConsumptionVisitor : IEquipmentVisitor {
        public int TotalPowerConsumption { get; private set; }

        public void VisitComputer(Computer computer) {
            TotalPowerConsumption += computer.PowerConsumption;
        }

        public void VisitPrinter(Printer printer) {
            TotalPowerConsumption += printer.PowerConsumption;
        }

        public void VisitAirConditioner(AirConditioner airConditioner) {
            TotalPowerConsumption += airConditioner.PowerConsumption;
        }
    }
}
