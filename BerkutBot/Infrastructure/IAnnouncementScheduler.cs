using System;
using System.Threading.Tasks;
using BerkutBot.Models;

namespace BerkutBot.Infrastructure
{
	public interface IAnnouncementScheduler
	{
		Task ScheduleAnnouncement(AnnouncementRequest announcementRequest);
	}
}

