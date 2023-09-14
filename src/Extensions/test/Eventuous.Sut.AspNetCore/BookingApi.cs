// Copyright (C) Ubiquitous AS. All rights reserved
// Licensed under the Apache License, Version 2.0.

using Eventuous.AspNetCore.Web;
using Eventuous.Sut.App;
using Eventuous.Sut.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Eventuous.Sut.AspNetCore;

public record BookingResult : Result {
    public new BookingState? State { get; init; }
}

public class BookingApi(ICommandService<Booking> service, MessageMap? commandMap = null) : CommandHttpApiBase<Booking, BookingResult>(service, commandMap) {
    [HttpPost("v2/pay")]
    public Task<ActionResult<BookingResult>> RegisterPayment([FromBody] RegisterPaymentHttp cmd, CancellationToken cancellationToken)
        => Handle<RegisterPaymentHttp, Commands.RecordPayment>(cmd, cancellationToken);

    public record RegisterPaymentHttp(
        string         BookingId,
        string         PaymentId,
        float          Amount,
        DateTimeOffset PaidAt
    );
}
