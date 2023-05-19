﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Azure.IIoT.OpcUa.Publisher.Module.Controllers
{
    using Azure.IIoT.OpcUa.Publisher.Module.Filters;
    using Azure.IIoT.OpcUa.Publisher.Models;
    using Furly.Tunnel.Router;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Publisher methods controller
    /// </summary>
    [Version("_V2")]
    [Version("_V1")]
    [Version("")]
    [RouterExceptionFilter]
    [ControllerExceptionFilter]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/configuration")]
    [ApiController]
    public class PublisherMethodsController : ControllerBase, IMethodController
    {
        /// <summary>
        /// Create controller
        /// </summary>
        /// <param name="configServices"></param>
        public PublisherMethodsController(IConfigurationServices configServices)
        {
            _configServices = configServices ??
                throw new ArgumentNullException(nameof(configServices));
        }

        /// <summary>
        /// Start publishing values from a node
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("start")]
        public async Task<PublishStartResponseModel> PublishStartAsync(
            RequestEnvelope<PublishStartRequestModel> request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Connection);
            ArgumentNullException.ThrowIfNull(request.Request);
            return await _configServices.PublishStartAsync(request.Connection,
                request.Request).ConfigureAwait(false);
        }

        /// <summary>
        /// Stop publishing values from a node
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("stop")]
        public async Task<PublishStopResponseModel> PublishStopAsync(
            RequestEnvelope<PublishStopRequestModel> request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Connection);
            ArgumentNullException.ThrowIfNull(request.Request);
            return await _configServices.PublishStopAsync(request.Connection,
                request.Request).ConfigureAwait(false);
        }

        /// <summary>
        /// Configure node values to publish and unpublish in bulk
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("bulk")]
        public async Task<PublishBulkResponseModel> PublishBulkAsync(
            RequestEnvelope<PublishBulkRequestModel> request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Connection);
            ArgumentNullException.ThrowIfNull(request.Request);
            return await _configServices.PublishBulkAsync(request.Connection,
                request.Request).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all published nodes for a server endpoint.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("list")]
        public async Task<PublishedItemListResponseModel> PublishListAsync(
            RequestEnvelope<PublishedItemListRequestModel> request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Connection);
            ArgumentNullException.ThrowIfNull(request.Request);
            return await _configServices.PublishListAsync(request.Connection,
                request.Request).ConfigureAwait(false);
        }

        /// <summary>
        /// Handler for PublishNodes direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("nodes")]
        public async Task<PublishedNodesResponseModel> PublishNodesAsync(
            PublishedNodesEntryModel request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _configServices.PublishNodesAsync(request, ct).ConfigureAwait(false);
            return new PublishedNodesResponseModel();
        }

        /// <summary>
        /// Handler for UnpublishNodes direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("nodes/unpublish")]
        public async Task<PublishedNodesResponseModel> UnpublishNodesAsync(
            PublishedNodesEntryModel request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _configServices.UnpublishNodesAsync(request, ct).ConfigureAwait(false);
            return new PublishedNodesResponseModel();
        }

        /// <summary>
        /// Handler for UnpublishAllNodes direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("nodes/unpublish/all")]
        public async Task<PublishedNodesResponseModel> UnpublishAllNodesAsync(
            PublishedNodesEntryModel request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _configServices.UnpublishAllNodesAsync(request, ct).ConfigureAwait(false);
            return new PublishedNodesResponseModel();
        }

        /// <summary>
        /// Handler for AddOrUpdateEndpoints direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPatch]
        public async Task<PublishedNodesResponseModel> AddOrUpdateEndpointsAsync(
            List<PublishedNodesEntryModel> request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var endpoints = request.ConvertAll(e => e);
            await _configServices.AddOrUpdateEndpointsAsync(endpoints, ct).ConfigureAwait(false);
            return new PublishedNodesResponseModel();
        }

        /// <summary>
        /// Handler for GetConfiguredEndpoints direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        [HttpGet]
        public async Task<GetConfiguredEndpointsResponseModel> GetConfiguredEndpointsAsync(
            [FromQuery] GetConfiguredEndpointsRequestModel? request = null, CancellationToken ct = default)
        {
            var response = await _configServices.GetConfiguredEndpointsAsync(
                request?.IncludeNodes ?? false, ct).ConfigureAwait(false);
            return new GetConfiguredEndpointsResponseModel
            {
                Endpoints = response
            };
        }

        /// <summary>
        /// Handler for SetConfiguredEndpoints direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPut]
        public async Task SetConfiguredEndpointsAsync(
            SetConfiguredEndpointsRequestModel request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _configServices.SetConfiguredEndpointsAsync(new List<PublishedNodesEntryModel>(
                request.Endpoints ?? Enumerable.Empty<PublishedNodesEntryModel>()),
                ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Handler for GetConfiguredNodesOnEndpoint direct method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/>
        /// is <c>null</c>.</exception>
        [HttpPost("endpoints/list/nodes")]
        public async Task<GetConfiguredNodesOnEndpointResponseModel> GetConfiguredNodesOnEndpointAsync(
            PublishedNodesEntryModel request, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var response = await _configServices.GetConfiguredNodesOnEndpointAsync(
                request, ct).ConfigureAwait(false);
            return new GetConfiguredNodesOnEndpointResponseModel
            {
                OpcNodes = response
            };
        }

        /// <summary>
        /// Handler for GetDiagnosticInfo direct method
        /// </summary>
        /// <param name="ct"></param>
        [HttpPost("diagnostics")]
        public async Task<List<PublishDiagnosticInfoModel>> GetDiagnosticInfoAsync(
            CancellationToken ct = default)
        {
            return await _configServices.GetDiagnosticInfoAsync(ct).ConfigureAwait(false);
        }

        private readonly IConfigurationServices _configServices;
    }
}