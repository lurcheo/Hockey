using System;

namespace Hockey.Client.Shared.Dto;

public class EventFactoryDto : BaseDto
{
    public int EventTypeId { get; set; }
    public TimeSpan DefaultTimeSpan { get; set; }
    public string AdditionalBindingKey { get; set; }
    public string BindingKey { get; set; }
}
