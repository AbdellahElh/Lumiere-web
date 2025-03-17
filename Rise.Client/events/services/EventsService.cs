using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rise.Shared.Events;
using System;
using AngleSharp.Dom.Events;

namespace Rise.Client.events.services
{
    public class EventService : IEventService
    {
        private readonly HttpClient httpClient;

        public EventService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<EventDto>> GetEventAsync(FiltersDataDto args)
        {
            string url = "Event";
            int i = 0;
           
            if (args.SelectedCinemas != null && args.SelectedCinemas.Any())
            {

                foreach (var cinema in args.SelectedCinemas)
                {
                    if(i == 0)
                    {
                        url += $"?cinema={cinema}";
                        i++;
                    }
                    else
                    {
                        url += $"&cinema={cinema}";
                    }
                    
                }
            }

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<EventDto>>();
                return result!;
           
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
          
                var response = await httpClient.GetAsync($"Event/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<EventDto>();
                return result!;
           
          

        }
    }
}