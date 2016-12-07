using System;
using System.Diagnostics;
using System.Linq;

namespace MicroService.Helpers
{
    public static class SystemTime
    {
        public static Func<DateTime> Now
        {
            [DebuggerStepThrough]
            get
            {
                return NowUtc;
            }
        }

        public static Func<DateTime> Local
        {
            [DebuggerStepThrough]
            get
            {
                var utc = DateTime.SpecifyKind(NowUtc(), DateTimeKind.Utc);
                return utc.ToLocalTime;
            }
        }

        /// <summary>
        /// Returns the given date to Default Timezone defined at the config.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToLocalTimeZone(this object date)
        {
            DateTime dt;
            if (!DateTime.TryParse(date.ToString(), out dt))
                return DateTime.UtcNow;

            // TODO: RR: Make Time Zone's configurable through any mechanism
            const string timeZone = "SA Western Standard Time";
            var defaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var userTime = TimeZoneInfo.ConvertTimeFromUtc(dt, defaultTimeZone);

            if (!userTime.IsDaylightSavingTime())
                return userTime;

            return defaultTimeZone.GetAdjustmentRules().
                Aggregate(userTime, (current, rule) => current.Subtract(rule.DaylightDelta));
        }

        /// <summary>
        /// Returns the given date to Default Timezone defined at the config.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToLocalTimeZone(this DateTime? date)
        {
            if (!date.HasValue)
                throw new ArgumentException("Null date values cannot be converted to Local Time Zone dates");

            return date.Value.ToLocalTimeZone();
        }

        #region Members
        private static readonly Func<DateTime> NowUtc = () => DateTime.UtcNow;
        #endregion
    }
}
