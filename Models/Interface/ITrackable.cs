using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Interface
{
    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        DateTime LastUpdatedAt { get; set; }
    }
}
