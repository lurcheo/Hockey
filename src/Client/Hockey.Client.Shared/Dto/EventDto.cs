using System;

namespace Hockey.Client.Shared.Dto;
public class EventDto : BaseDto
{
    public int EventTypeId { get; set; }

    public TimeSpan StartEventTime { get; set; }
    public TimeSpan EndEventTime { get; set; }
}
