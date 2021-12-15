using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Strategy {
    class Program {
        static void Main(string[] args)
        {
            IServerRepresentation representation =
                new AwsServerRepresentation("US01", "US East (Northern Virginia)", 4, 8, 250);

            //  Section 2
            //  Start with a default strategy
            CloudServer server = new CloudServer(representation, (r, d) =>
            {
                var basePrice = r.NCores * r.Memory * r.Storage / 100;
                var multiplier = r.Region.StartsWith("US") ? 0.01 : 0.014;
                return basePrice * multiplier;
            });

            server.AddBilling(120);

            //  Client can change the strategy in runtime
            server.SetStrategy((r, d) => {
                var basePrice = r.NCores * r.Memory * r.Storage / 200;
                var multiplier = r.Region.StartsWith("US") ? 0.012 : 0.018;
                return basePrice * multiplier;
            });

            server.AddBilling(140);

            var billAmount = server.GetTotalBill();
            Console.WriteLine($"Total charges: {billAmount}");
        }
    }

    interface IServerRepresentation {
        string AvailabilityZone { get; }
        string Region { get; }
        int NCores { get; }
        int Memory { get; }
        int Storage { get; }
    }

    class AwsServerRepresentation:IServerRepresentation
    {
        public AwsServerRepresentation(string availabilityZone, string region, int nCores, int memory, int storage)
        {
            AvailabilityZone = availabilityZone;
            Region = region;
            NCores = nCores;
            Memory = memory;
            Storage = storage;
        }

        public string AvailabilityZone { get; }
        public string Region { get; }
        public int NCores { get; }
        public int Memory { get; }
        public int Storage { get; }
    }

    class CloudServer
    {
        private readonly IServerRepresentation _representation;

        //  Section 1
        //  The crux is to vary the algorithm independently of the use site
        //  and make it dynamically pluggable
        //  Classic pattern uses the object that encapsulates the algorithm
        private Func<IServerRepresentation, int, double> _billingStrategy;
        private readonly IList<double> _billing = new List<double>();

        //  Base strategy (optional)
        public CloudServer(IServerRepresentation representation, Func<IServerRepresentation, int, double> billingStrategy)
        {
            _representation = representation;
            _billingStrategy = billingStrategy;
        }

        public void AddBilling(int minutes)
        {
            //  delegation to algorithm object
            _billing.Add(_billingStrategy(_representation, minutes));
        }

        //  Algorithm can be changed in runtime
        public void SetStrategy(Func<IServerRepresentation, int, double> strategy)
        {
            _billingStrategy = strategy;
        }

        public double GetTotalBill()
        {
            return _billing.Sum();
        }
    }
}
