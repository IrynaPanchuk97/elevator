using System;
using System.Timers;
using LiftSimulator.AbstractServices;
using LiftSimulator.Models;

namespace LiftSimulator.ConcreteServices
{
    public class PersonGenerator : IPersonGenerator
    {
        private readonly BuildingModel _building;

        public PersonGenerator(BuildingModel building)
        {
            _building = building;
        }

        public void GeneratePerson()
        {
            var aTimer = new Timer(10000);

            aTimer.Elapsed += AddPerson;
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
        }

        private void AddPerson(object source, ElapsedEventArgs e)
        {
            var random = new Random();
            var startFloorIndex = random.Next(0, 4);
            var endFloorIndex = random.Next(0, 4);

            while (endFloorIndex == startFloorIndex)
            {
                endFloorIndex = random.Next(0, 4);
            }

            var personModel = new PersonModel(_building, startFloorIndex, endFloorIndex);
            _building.NeedUiChanges();
        }
    }
}
