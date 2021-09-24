using Microsoft.OData.SampleService.Models.TripPin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OData_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            ListPeople();
            SearchForName("Russell");

            //modify data:
            MakeFriends("russellwhyte", "javieralfred");

            //search again to see if modified ok:
            SearchForName("Russell");   //does not show any update which means the service is dummy
        }

        private static void MakeFriends(string user1, string user2)
        {
            DefaultContainer context = GetContext();
            var person1 = context.People.ByKey(userName: user1).GetValue();
            var person2 = context.People.ByKey(userName: user2).GetValue();

            person1.Friends.Add(person2);
            //this does not work either, meaning the service does not update at all: person1.LastName = "Uludamar";
            context.UpdateObject(person1);
            context.SaveChanges();
        }

        private static void SearchForName(string firstName)
        {
            DefaultContainer context = GetContext();
            var people = context.People.Where(c => c.FirstName == firstName);

            Console.WriteLine($"Persons with name {firstName}:");
            foreach (var person in people)
            {
                PrintDetails(person);

                var friends = person.Friends;
                Console.WriteLine("Friends:");
                foreach (var friend in friends)
                {
                    PrintDetails(friend);
                }
            }
        }

        private static void PrintDetails(Person person)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}. Gender: {person.Gender}");
        }

        static void ListPeople()
        {
            DefaultContainer context = GetContext();

            IEnumerable<Person> people = context.People.Execute();
            foreach (var person in people)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName} ({person.UserName})");
            }
        }

        private static DefaultContainer context;

        private static DefaultContainer GetContext()
        {
            if (context != null)
                return context;

            var serviceRoot = "https://services.odata.org/V4/TripPinServiceRW/";
            context = new DefaultContainer(new Uri(serviceRoot));

            return context;
        }
    }
}
