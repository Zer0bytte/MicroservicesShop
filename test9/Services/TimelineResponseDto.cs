namespace InstagramDMs.API.Services
{
    public class TimelineResponseDto
    {
        public List<TimelineEventDto> Events { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}