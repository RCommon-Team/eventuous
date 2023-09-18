// Copyright (C) Ubiquitous AS. All rights reserved
// Licensed under the Apache License, Version 2.0.

using Eventuous.AspNetCore.Web;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing;

using Builder;
using Http;

public delegate TCommand ConvertAndEnrichCommand<in TContract, out TCommand>(TContract command, HttpContext httpContext);

public static partial class RouteBuilderExtensions {
    /// <summary>
    /// Map command to HTTP POST endpoint.
    /// The HTTP command type should be annotated with <seealso cref="HttpCommandAttribute"/> attribute.
    /// </summary>
    /// <param name="builder">Endpoint route builder instance</param>
    /// <param name="convert">Function to convert HTTP command to domain command</param>
    /// <typeparam name="TContract">HTTP command type</typeparam>
    /// <typeparam name="TCommand">Domain command type</typeparam>
    /// <typeparam name="TAggregate">Aggregate type</typeparam>
    /// <typeparam name="TResult">Result type that will be returned</typeparam>
    /// <returns></returns>
    [PublicAPI]
    public static RouteHandlerBuilder MapCommand<TContract, TCommand, TAggregate, TResult>(
        this IEndpointRouteBuilder                   builder,
        ConvertAndEnrichCommand<TContract, TCommand> convert
    ) where TAggregate : Aggregate where TCommand : class where TContract : class where TResult : class, new() {
        var attr = typeof(TContract).GetAttribute<HttpCommandAttribute>();
        return Map<TAggregate, TContract, TCommand, TResult>(builder, attr?.Route, convert, attr?.PolicyName);
    }

    /// <summary>
    /// Map command to HTTP POST endpoint
    /// </summary>
    /// <param name="builder">Endpoint route builder instance</param>
    /// <param name="route">API route for the POST endpoint</param>
    /// <param name="convert">Function to convert HTTP command to domain command</param>
    /// <param name="policyName">Optional authorization policy name</param>
    /// <typeparam name="TContract">HTTP command type</typeparam>
    /// <typeparam name="TCommand">Domain command type</typeparam>
    /// <typeparam name="TAggregate">Aggregate type</typeparam>
    /// <typeparam name="TResult">Result type that will be returned</typeparam>
    /// <returns></returns>
    [PublicAPI]
    public static RouteHandlerBuilder MapCommand<TContract, TCommand, TAggregate, TResult>(
        this IEndpointRouteBuilder                   builder,
        string?                                      route,
        ConvertAndEnrichCommand<TContract, TCommand> convert,
        string?                                      policyName = null
    ) where TAggregate : Aggregate where TCommand : class where TContract : class where TResult : class, new()
        => Map<TAggregate, TContract, TCommand, TResult>(builder, route, convert, policyName);
}
