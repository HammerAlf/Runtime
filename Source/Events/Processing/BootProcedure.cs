namespace Dolittle.Runtime.Events.Processing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dolittle.Bootstrapping;
    using Dolittle.Collections;
    using Dolittle.Logging;
    using Dolittle.Runtime.Events;
    using Dolittle.Runtime.Events.Processing;
    using Dolittle.Runtime.Events.Store;
    using Dolittle.Runtime.Tenancy;
    using Dolittle.Types;
    using Dolittle.DependencyInversion;
    using Dolittle.Execution;
    using Dolittle.Security;
    using Dolittle.Tenancy;

    /// <summary>
    /// Represents the <see cref="ICanPerformBootProcedure">boot procedure</see> for <see cref="EventProcessors"/>
    /// </summary>
    public class BootProcedure : ICanPerformBootProcedure
    {
        IInstancesOf<IKnowAboutEventProcessors> _systemsThatKnowAboutEventProcessors;
        ITenants _tenants;

        int _canPerformCount = 10;

        IScopedEventProcessingHub _processingHub;
        ILogger _logger;
        private readonly FactoryFor<IEventProcessorOffsetRepository> _getOffsetRepository;
        private readonly FactoryFor<IFetchUnprocessedEvents> _getUnprocessedEventsFetcher;
        private readonly IExecutionContextManager _executionContextManager;

        /// <summary>
        /// Instantiates a new instance of <see cref="BootProcedure" />
        /// </summary>
        /// <param name="systemsThatKnowAboutEventProcessors">Provides <see cref="IEventProcessor">Event Processors</see></param>
        /// <param name="tenants">A collection of all <see cref="ITenants">tenants</see></param>
        /// <param name="processingHub">An instance of <see cref="IScopedEventProcessingHub" /> for processing <see cref="CommittedEventStream">Committed Event Streams</see></param>
        /// <param name="getOffsetRepository">A factory function to return a correctly scoped instance of <see cref="IEventProcessorOffsetRepository" /></param>
        /// <param name="getUnprocessedEventsFetcher">A factory function to return a correctly scoped instance of <see cref="IFetchUnprocessedEvents" /></param>
        /// <param name="executionContextManager">The <see cref="ExecutionContextManager" /> for setting the correct execution context for the Event Processors </param>
        /// <param name="logger">An instance of <see cref="ILogger" /> for logging</param>
        public BootProcedure(IInstancesOf<IKnowAboutEventProcessors> systemsThatKnowAboutEventProcessors, 
                                ITenants tenants, 
                                IScopedEventProcessingHub processingHub, 
                                FactoryFor<IEventProcessorOffsetRepository> getOffsetRepository, 
                                FactoryFor<IFetchUnprocessedEvents> getUnprocessedEventsFetcher, 
                                IExecutionContextManager executionContextManager,                                
                                ILogger logger)
        {
            _processingHub = processingHub;
            _logger = logger;
            _tenants = tenants;
            _systemsThatKnowAboutEventProcessors = systemsThatKnowAboutEventProcessors;
            _getOffsetRepository = getOffsetRepository;
            _getUnprocessedEventsFetcher = getUnprocessedEventsFetcher;
            _executionContextManager = executionContextManager;
            _logger = logger;
        }

        /// <inheritdoc />
        public bool CanPerform()
        {
            var hasAny = _systemsThatKnowAboutEventProcessors.SelectMany(_ => _.ToList()).Any();
            if( hasAny || _canPerformCount-- == 0 ) return true;
            return false;
        }

        /// <inheritdoc />
        public void Perform()
        {
            ProcessInParallel();
            _processingHub.BeginProcessingEvents();
        }

        void ProcessInParallel()
        {
            //_logger.Information("Process")
            Parallel.ForEach(_systemsThatKnowAboutEventProcessors.ToList(), (system) =>
            {
                Parallel.ForEach(system.ToList(), (processor) =>
                {
                    Parallel.ForEach(_tenants.All, (t) =>
                    {
                        if (t != TenantId.System && t != TenantId.Unknown) 
                        {
                            _executionContextManager.CurrentFor(t, CorrelationId.New(), Claims.Empty);
                            _processingHub.Register(new ScopedEventProcessor(t, processor,_getOffsetRepository,_getUnprocessedEventsFetcher, _logger));
                        }
                    });
                });
            });
        }
    }
}