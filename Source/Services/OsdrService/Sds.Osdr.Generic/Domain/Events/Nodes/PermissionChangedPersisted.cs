﻿using CQRSlite.Events;
using Sds.Osdr.Generic.Domain.ValueObjects;
using System;

namespace Sds.Osdr.Generic.Domain.Events.Nodes
{
    public interface PermissionChangedPersisted : IEvent
    {
        Guid UserId { get; set; }
        Guid ParentId { get; set; }
        AccessPermissions AccessPermissions { get; set; }
    }
}
