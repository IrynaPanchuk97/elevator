namespace LiftSimulator.Mediators
{
    public class BuildingColleague
    {
        private readonly BuildingMediator _mediator;

        public BuildingColleague(BuildingMediator mediator)
        {
            _mediator = mediator;
        }

        public void NeedUiChanges()
        {
            _mediator.SomethingChangesOnUi(this);
        }
    }
}
