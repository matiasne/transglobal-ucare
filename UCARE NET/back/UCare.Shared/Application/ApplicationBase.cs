using GQ.Architecture.DDD.Application;
using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Shared.Application
{
    public abstract class ApplicationBase : IApplication
    {
        public string Name => this.GetType().Name;

        protected IAuthUserRepository authUser;
        protected IEventBus eventBus;
        protected ICacheService cacheService;
        protected ApplicationBase(IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService)
        {
            this.authUser = authUser;
            this.eventBus = eventBus;
            this.cacheService = cacheService;
        }

    }
}
