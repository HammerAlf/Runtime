// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using System.Timers;
using Dolittle.Collections;
using Dolittle.Heads.Runtime;
using Dolittle.Logging;
using Dolittle.Protobuf;
using Dolittle.Time;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static Dolittle.Heads.Runtime.Heads;

namespace Dolittle.Runtime.Heads
{
    /// <summary>
    /// Represents an implementation of <see cref="ClientBase"/>.
    /// </summary>
    public class HeadsService : HeadsBase
    {
        readonly IConnectedHeads _connectedHeads;
        readonly ISystemClock _systemClock;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadsService"/> class.
        /// </summary>
        /// <param name="connectedHeads"><see cref="IConnectedHeads"/> for working with connected heads.</param>
        /// <param name="systemClock"><see cref="ISystemClock"/> for time.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public HeadsService(
            IConnectedHeads connectedHeads,
            ISystemClock systemClock,
            ILogger logger)
        {
            _connectedHeads = connectedHeads;
            _systemClock = systemClock;
            _logger = logger;
        }

        /// <summary>
        /// Signals a <see cref="Head"/> client has disconnected.
        /// </summary>
        /// <param name="client"><see cref="Head"/> to disconnect.</param>
        public void ClientDisconnected(Head client)
        {
            _connectedHeads.Disconnect(client.HeadId);
        }

        /// <inheritdoc/>
        public override Task Connect(HeadInfo request, IServerStreamWriter<Empty> responseStream, ServerCallContext context)
        {
            var headId = request.HeadId.To<HeadId>();
            Timer timer = null;
            try
            {
                _logger.Information($"Head connected '{headId}'");
                if (request.ServicesByName.Count == 0) _logger.Information("Not providing any head services");
                else request.ServicesByName.ForEach(_ => _logger.Information($"Providing service {_}"));

                var connectionTime = _systemClock.GetCurrentTime();
                var client = new Head(
                    headId,
                    request.Host,
                    request.Port,
                    request.Runtime,
                    request.ServicesByName,
                    connectionTime);

                _connectedHeads.Connect(client);

                timer = new Timer(1000)
                {
                    Enabled = true
                };
                timer.Elapsed += (s, e) => responseStream.WriteAsync(new Empty());

                context.CancellationToken.ThrowIfCancellationRequested();
                context.CancellationToken.WaitHandle.WaitOne();
            }
            finally
            {
                _connectedHeads.Disconnect(headId);
                timer?.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}