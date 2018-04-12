using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;

namespace ICASStacks.Repository
{
    internal class StateOfLocationRepository
    {

        private readonly IIcasRepository<StateOfLocation> _repository;
        private readonly IcasUoWork _uoWork;

        public StateOfLocationRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<StateOfLocation>(_uoWork);
        }
    }
}
