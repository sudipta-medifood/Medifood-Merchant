using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Interface
{
    public interface ITrackable
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset LastUpdatedAt { get; set; }
    }
}
