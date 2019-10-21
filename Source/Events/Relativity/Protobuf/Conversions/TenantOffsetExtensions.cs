/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using Dolittle.Tenancy;
using Dolittle.Runtime.Protobuf;
using Dolittle.Protobuf;
using Google.Protobuf.Collections;
using grpc = Dolittle.Events.Relativity.Microservice;

namespace Dolittle.Runtime.Events.Relativity.Protobuf.Conversion
{
    /// <summary>
    /// Extensions for converting <see cref="TenantOffset"/> to and from protobuf representations
    /// </summary>
    public static class TenantOffsetExtensions
    {
        /// <summary>
        /// Convert from <see cref="TenantOffset"/> to <see cref="TenantOffset"/>
        /// </summary>
        /// /// <param name="tenantOffset"><see cref="TenantOffset"/> to convert from</param>
        /// <returns>Converted <see cref="grpc.TenantOffset"/></returns>
        public static grpc.TenantOffset ToProtobuf(this TenantOffset tenantOffset)
        {
            var message = new grpc.TenantOffset
            {
                Tenant = tenantOffset.Tenant.ToProtobuf(),
                Offset = tenantOffset.Offset
            };
            return message;
        }

        /// <summary>
        /// Convert from <see cref="grpc.TenantOffset"/> to <see cref="TenantOffset"/>
        /// </summary>
        /// <param name="tenantOffset"><see cref="grpc.TenantOffset"/> to convert from</param>
        /// <returns>Converted <see cref="TenantOffset"/></returns>
        public static TenantOffset ToTenantOffset(this grpc.TenantOffset tenantOffset)
        {
            return new TenantOffset(
                tenantOffset.Tenant.To<TenantId>(),
                tenantOffset.Offset);
        }

        /// <summary>
        /// Convert from collection of <see cref="TenantOffset"/> to collection of <see cref="grpc.TenantOffset"/>
        /// </summary>
        /// <param name="offsets">Collection of <see cref="TenantOffset">Offsets</see> to convert from</param>
        /// <returns>Collection of <see cref="grpc.TenantOffset"/></returns>
        public static RepeatedField<grpc.TenantOffset> ToProtobuf(this IEnumerable<TenantOffset> offsets)
        {
            var protobuf = new RepeatedField<grpc.TenantOffset>
            {
                offsets.Select(_ => _.ToProtobuf())
            };
            return protobuf;
        }

        /// <summary>
        /// Convert from  collection of <see cref="grpc.TenantOffset"/> to collection of <see cref="TenantOffset"/>
        /// </summary>
        /// <param name="offsets"><see cref="grpc.TenantOffset">Offsets</see> to convert from</param>
        /// <returns>Converted <see cref="TenantOffset">offsets</see></returns>
        public static IEnumerable<TenantOffset> ToTenantOffsets(this IEnumerable<grpc.TenantOffset> offsets)
        {
            return offsets.Select(_ => _.ToTenantOffset()).ToArray();
        }
    }
}