namespace LiftSimulator.Custom_classes
{
    public abstract class Colleague
    {
        protected Mediator Mediator;

        protected Colleague(Mediator mediator)
        {
            Mediator = mediator;
        }
    }
}
