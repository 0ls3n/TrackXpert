using System;

namespace TrackXpert_ClassLibrary.Models;

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
