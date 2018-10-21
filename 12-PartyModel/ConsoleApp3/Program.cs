using System;
using System.Collections.Generic;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var parties = new List<Party>();
            parties.Add(new Company { Id = 1, Name = "Delfi", RegNo = "121312" });
            parties.Add(new Person { Id = 2, FirstName = "Jüri", LastName = "Nüri", RegNo = "457272727" });
            parties.Add(new Person { Id = 3, FirstName = "Jüri", LastName = "Nüri", RegNo = "565636726" });

            foreach (var party in parties)
            {
                Console.WriteLine(party.DisplayName + " (" + party.RegNo + ")");
            }

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        public abstract class Party
        {
            public int Id { get; set; }
            
            public string RegNo { get; set; }

            public abstract string DisplayName { get; }
        }

        public class Person : Party
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public override string DisplayName { get { return LastName + ", " + FirstName; } }
        }

        public class Company : Party
        {
            public string Name { get; set; }

            public override string DisplayName { get { return Name; } }
        }
    }
}
