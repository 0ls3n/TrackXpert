using System;

namespace TrackXpert_API.Models;

public enum PStatus
{
    Uploaded,
    Processing,
    Ready,
    Error,
    Queued,
    Rejected,
    PreviewAvailable,
    PendingApproval,
    Archived,
    Deleted
}
