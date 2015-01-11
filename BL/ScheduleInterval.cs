using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Scheduling
{
    public class ScheduleInterval
    {
        /// <summary>
        /// <remarks>Populated when ComputeFreeIntervals is called</remarks>
        /// </summary>
        protected static List<SchoolDay> SchoolDays;

        protected const int DayStartHour = 7;
        protected const int DayEndHour = 21;

        public DateTime IntervalStart { get; set; }

        public DateTime IntervalEnd { get; set; }

        public TimeSpan Duration { get; private set; }

        public SchoolDay SchoolDay { get; set; }


        public ScheduleInterval(DateTime start, DateTime end, SchoolDay day)
        {
            if (start > end)
            {
                throw new Exception("The interval end cannot be less than the interval start.");
            }

            IntervalStart = start;
            IntervalEnd = end;
            SchoolDay = day;

            Duration = IntervalEnd - IntervalStart;
        }

        /// <summary>
        /// Computes free intervals for each day from Monday to Friday. Note: a work day starts at 7 in the morning and ends at 9 in the evening.
        /// </summary>
        /// <param name="hours">The required amount of hours in the interval.</param>
        /// <param name="_existingIntervals">The existing intervals which the algorithm filters against.</param>
        /// <returns>A list of free intervals.</returns>
        public static List<ScheduleInterval> ComputeFreeIntervals(int hours, List<ScheduleInterval> _existingIntervals)
        {
            List<ScheduleInterval> freeIntervals = new List<ScheduleInterval>();

            Queries query = new Queries();
            SchoolDays = query.GetSchoolDays();

            List<ScheduleInterval> mondayIntervals = _existingIntervals.Where(x => x.SchoolDay.Name == "Monday").OrderBy(x => x.IntervalStart).ToList();
            List<ScheduleInterval> tuesdayIntervals = _existingIntervals.Where(x => x.SchoolDay.Name == "Tuesday").OrderBy(x => x.IntervalStart).ToList();
            List<ScheduleInterval> wednesdayIntervals = _existingIntervals.Where(x => x.SchoolDay.Name == "Wednesday").OrderBy(x => x.IntervalStart).ToList();
            List<ScheduleInterval> thursdayIntervals = _existingIntervals.Where(x => x.SchoolDay.Name == "Thursday").OrderBy(x => x.IntervalStart).ToList();
            List<ScheduleInterval> fridayIntervals = _existingIntervals.Where(x => x.SchoolDay.Name == "Friday").OrderBy(x => x.IntervalStart).ToList();

            ComputeFreeDailyIntervals(hours, mondayIntervals, DayStartHour, SchoolDays.Single(x => x.Name == "Monday"), freeIntervals);
            ComputeFreeDailyIntervals(hours, tuesdayIntervals, DayStartHour, SchoolDays.Single(x => x.Name == "Tuesday"), freeIntervals);
            ComputeFreeDailyIntervals(hours, wednesdayIntervals, DayStartHour, SchoolDays.Single(x => x.Name == "Wednesday"), freeIntervals);
            ComputeFreeDailyIntervals(hours, thursdayIntervals, DayStartHour, SchoolDays.Single(x => x.Name == "Thursday"), freeIntervals);
            ComputeFreeDailyIntervals(hours, fridayIntervals, DayStartHour, SchoolDays.Single(x => x.Name == "Friday"), freeIntervals);

            return freeIntervals;
        }
        
        private static void ComputeFreeDailyIntervals(int hours, List<ScheduleInterval> dailyIntervals, int currentHour, SchoolDay day, List<ScheduleInterval> freeIntervals)
        {
            if ((currentHour == DayEndHour) || (currentHour + hours > DayEndHour))
            {
                return;
            }

            ScheduleInterval blockingInterval = null;

            foreach (ScheduleInterval interval in dailyIntervals)
            {
                if ((currentHour + hours) > interval.IntervalStart.Hour)
                {
                    if (currentHour < interval.IntervalStart.Hour)
                    {
                        blockingInterval = interval;
                        break;
                    }
                    else if (currentHour < interval.IntervalEnd.Hour)
                    {
                        blockingInterval = interval;
                        break;
                    }
                }
            }
            
            if (blockingInterval == null)
            {
                ScheduleInterval interval = new ScheduleInterval(new DateTime(2014, 1, 1, currentHour, 0, 0), new DateTime(2014, 1, 1, currentHour + hours, 0, 0), day);
                freeIntervals.Add(interval);
            }
            currentHour++; 
            ComputeFreeDailyIntervals(hours, dailyIntervals, currentHour, day, freeIntervals);
        }
    }

    /// <summary>
    /// A structure holding all the free classrooms in an interval.
    /// </summary>
    public class ClassroomScheduleInterval
    {
        public ScheduleInterval Interval { get; set; }

        public List<Classroom> Classrooms { get; set; }
    }
}
