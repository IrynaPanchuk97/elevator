using LiftSimulator.Models;
using LiftSimulator.ViewModels;

namespace LiftSimulator.Mediators
{
    public class PersonMediator
    {
        public PersonViewModel PersonView { get; private set; }

        public PersonMediator()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            PersonView = new PersonViewModel(this);
        }

        public void SomethingChangesOnUi(PersonColleague colleague)
        {
            if (colleague is PersonModel person)
            {
                var personStatus = person.PersonStatus;
                switch (personStatus)
                {
                    case PersonStatus.WaitingForAnElevator:

                        break;
                }
            }
        }
    }
}
