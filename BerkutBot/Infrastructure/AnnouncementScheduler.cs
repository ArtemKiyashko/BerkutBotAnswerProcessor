using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BerkutBot.Models;
using BerkutBot.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BerkutBot.Infrastructure
{
	public class AnnouncementScheduler : IAnnouncementScheduler
	{
        private readonly HttpClient _httpClient;
        private readonly SchedulerOptions _options;
        private readonly string _query = "?api-version={0}&sp={1}&sv={2}&sig={3}";

        public AnnouncementScheduler(HttpClient httpClient, IOptions<SchedulerOptions> options)
		{
            _httpClient = httpClient;
            _options = options.Value;
            _query = string.Format(_query, _options.ApiVersion, _options.Sp, _options.Sv, _options.Sig);
        }

        public async Task ScheduleAnnouncement(AnnouncementRequest announcementRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(announcementRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_query, content);
        }
    }
}

