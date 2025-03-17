namespace Rise.Shared.Events;

public interface IEventService
{
    Task<List<EventDto>> GetEventAsync(FiltersDataDto filtersData);

    Task<EventDto> GetEventByIdAsync(int id);

}