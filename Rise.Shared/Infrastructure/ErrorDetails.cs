﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rise.Shared.Infrastructure;

/// <summary>
/// Error returned by the <see cref="ExceptionMiddleware"/> when an <see cref="Exception"/> is thrown.
/// This class is used to not show the entire stacktrace to the client.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// The HTTP statuscode
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The custom message of the error, most of the time the one from the exception.
    /// </summary>
    public string? Message { get; set; }

    public ErrorDetails()
    {

    }

    public ErrorDetails(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        StatusCode = (int)statusCode;
        Message = message;
    }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
