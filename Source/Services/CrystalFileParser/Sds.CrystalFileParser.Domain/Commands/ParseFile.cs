﻿using MassTransit;
using System;

namespace Sds.CrystalFileParser.Domain.Commands
{
    public interface ParseFile : CorrelatedBy<Guid>
    {
        Guid Id { get; }
        string Bucket { get; }
        Guid BlobId { get; }
        Guid UserId { get; }
    }
}
