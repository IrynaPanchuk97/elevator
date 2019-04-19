using System;
using LiftSimulator.AbstractServices;
using LiftSimulator.Mediators;
using LiftSimulator.Models;

namespace LiftSimulator.ConcreteServices
{
    public class PersonGenerator : BuildingColleague, IPersonGenerator
    {
        private readonly BuildingModel _building;

        public PersonGenerator(BuildingModel building, BuildingMediator mediator) : base(mediator)
        {
            _building = building;
        }

        public PersonModel AddPerson()
        {
            var random = new Random();
            var startFloorIndex = random.Next(0, 4);
            var endFloorIndex = random.Next(0, 4);

            while (endFloorIndex == startFloorIndex)
            {
                endFloorIndex = random.Next(0, 4);
            }

            return new PersonModel(_building, startFloorIndex, endFloorIndex, new PersonMediator());
        }
    }
}
