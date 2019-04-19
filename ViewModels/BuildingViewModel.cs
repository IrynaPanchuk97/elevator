using System.Collections.Generic;
using LiftSimulator.Mediators;

namespace LiftSimulator.ViewModels
{
    public class BuildingViewModel : BuildingColleague
    {
        public int ExitLocation { get; set; }

        public BuildingViewModel(BuildingMediator mediator) : base(mediator)
        {
            ExitLocation = 677;     
        }
    }
}