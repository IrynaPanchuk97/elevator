using System.Collections.Generic;
using LiftSimulator.Mediators;

namespace LiftSimulator.ViewModels
{
    public class BuildingViewModel : BuildingColleague
    {
        public int ExitLocation { get; set; }
        public List<ElevatorViewModel> Elevators { get; set; }
        public List<PersonViewModel> ListOfAllPeopleWhoNeedAnimation { get; set; }

        public BuildingViewModel(BuildingMediator mediator) : base(mediator)
        {
            ExitLocation = 677;
            Elevators = new List<ElevatorViewModel>();
            for (var i = 0; i < 3; i++)
            {
                Elevators.Add(new ElevatorViewModel(120 + (85 + 2 * i) * i, 373));
            }

            ListOfAllPeopleWhoNeedAnimation = new List<PersonViewModel>();
        }

        public void AddPersonToListOfWhoNeedAnimation()
        {
            ListOfAllPeopleWhoNeedAnimation.Add(new PersonViewModel());
        }
    }
}