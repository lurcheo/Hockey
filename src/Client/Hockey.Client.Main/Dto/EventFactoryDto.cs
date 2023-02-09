using System;

namespace Hockey.Client.Main.Dto;

internal class EventFactoryDto : BaseDto
{
    public int EventTypeId { get; set; }
    public TimeSpan DefaultTimeSpan { get; set; }
}
