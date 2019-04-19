namespace LiftSimulator.Mediators
{
    public class PersonColleague
    {
        private readonly PersonMediator _mediator;

        public PersonColleague(PersonMediator mediator)
        {
            _mediator = mediator;
        }

        public void NeedUiChanges()
        {
            _mediator.SomethingChangesOnUi(this);
        }
    }
}
