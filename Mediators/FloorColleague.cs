namespace LiftSimulator.Mediators
{
    public class FloorColleague
    {
        private readonly FloorMediator _mediator;

        public FloorColleague(FloorMediator mediator)
        {
            _mediator = mediator;
        }

        public void NeedUiChanges()
        {
            _mediator.SomethingChangesOnUi(this);
        }
    }
}
