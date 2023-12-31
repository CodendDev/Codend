﻿using Codend.Domain.Core.Primitives;

namespace Codend.Domain.Entities;

public sealed record UserId : EntityId<Guid>
{
    public UserId()
    {
    }

    public UserId(Guid value) : base(value)
    {
    }
}