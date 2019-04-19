namespace LiftSimulator.Mediators
{
    public class ElevatorColleague
    {
        private readonly ElevatorMediator _mediator;

        public ElevatorColleague(ElevatorMediator mediator)
        {
            _mediator = mediator;
        }

        public void NeedUiChanges()
        {
            _mediator.SomethingChangesOnUi(this);
        }
    }
}
