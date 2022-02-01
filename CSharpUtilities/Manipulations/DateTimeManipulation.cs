using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities.Manipulations
{
    public class DateTimeManipulation
    {
        #region ClassData

        #region TimeZones
        private static readonly string _macLinuxTimeZone = "Europe/Sarajevo";
        private static readonly string _windowsTimeZone = "Central European Standard Time";
        #endregion

        #region Enumerations
        public enum DateFormatType
        {
            MonthWithNumbers = 1,
            MonthWithWordsShort,
            MonthWithWordsLong
        }
        public enum TimeFormatType
        {
            ShortTime = 1,
            LongTime
        }

        public enum Period
        {
            Daily=1, 
            Weekly, 
            Monthly, 
            Yearly
        }

        public enum TimeFrameType
        {
            Both = 1, 
            Current, 
            Previous
        }

        #endregion

        #region MonthLists
        private static readonly List<string> _shortMonths = new List<string>()
        {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec",
        };

        private static readonly List<string> _longMonths = new List<string>()
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "Jun",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        };
        #endregion

        #region TimeFrame
        /// <summary>
        /// Defines a model that will contain dates of a specific timeframe, based on a period
        /// </summary>
        public class TimeFrame
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public DateTime PreviousFrom { get; set; }
            public DateTime PreviousTo { get; set; }
        }

        #endregion
        #endregion

        /// <summary>
        /// Will return the current system date and time, of a specific time zone.
        /// Time zones are stored differently on different operating systems.
        /// This function will first try Mac/Linux, then Windows.
        /// Define the time zones above, inside the class data.
        /// </summary>
        /// <returns>
        /// DateTime - current system date and time
        /// </returns>
        public static DateTime GetCurrentDate()
        {
            try
            {
                return System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(_macLinuxTimeZone));
            }
            catch
            {
                return System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(_windowsTimeZone));
            }
        }

        /// <summary>
        /// Will return a formatted date, based on the specified input parameters, that are set to some default behaviours.
        /// </summary>
        /// <param name="date" type="DateTime">the date that needs to be formatted</param>
        /// <param name="dateFormatType" type="DateFormatType">the dates month format type</param>
        /// <param name="includeTime" type="bool">defines if time will be included in the format</param>
        /// <param name="timeFormatType" type="TimeFormatType">will format the time as well, if the time gets included</param>
        /// <returns>string - formatted date and time</returns>
        public static string FormatDate(DateTime date, DateFormatType dateFormatType = DateFormatType.MonthWithNumbers, bool includeTime = false, TimeFormatType timeFormatType = TimeFormatType.ShortTime)
        {
            string formatedDate;
            switch (dateFormatType)
            {
                case DateFormatType.MonthWithNumbers:
                    formatedDate = date.Day + "." + date.Month + "." + date.Year;
                    break;
                case DateFormatType.MonthWithWordsShort:
                    formatedDate = date.Day + " " + _shortMonths[0] + " " + date.Year;
                    break;
                case DateFormatType.MonthWithWordsLong:
                    formatedDate = date.Day + " " + _longMonths[0] + " " + date.Year;
                    break;
                default:
                    return FormatDate(date, DateFormatType.MonthWithNumbers, true, TimeFormatType.ShortTime);
            }
            if (includeTime)
            {
                if (timeFormatType == TimeFormatType.ShortTime) formatedDate += " | " + date.Hour + ":" + date.Minute;
                else formatedDate += " | " + date.Hour + ":" + date.Minute + ":" + date.Second;
            }
            return formatedDate;
        }

        /// <summary>
        /// If no date is provided, or gets left empty, the date object will fall back to an empty date.
        /// Will return that default empty date for comparison purposes.
        /// </summary>
        /// <returns>DatetTime - empty date</returns>
        public static DateTime EmptyDate()
        {
            return new DateTime(0001, 1, 1);
        }


        /// <summary>
        /// Will return the current day of the week.
        /// </summary>
        /// <returns>DayOfWeek - current day of the week</returns>
        public static DayOfWeek GetCurrentDay()
        {
            return GetCurrentDate().DayOfWeek;
        }

        /// <summary>
        /// Checks if a date falls into specific day.
        /// </summary>
        /// <param name="dateToCheck" type="DateTime">the date that needs to be checked</param>
        /// <param name="dayOfWeek" type="DayOfWeek">the day of week in which the date is/should fall into</param>
        /// <returns>bool - if the date falls into a day</returns>
        public static bool DateIsDay(DateTime dateToCheck, DayOfWeek dayOfWeek)
        {
            return dateToCheck.DayOfWeek == dayOfWeek;
        }

        /// <summary>
        /// Returns dates inside a specific timeframe.
        /// A timeframe is defined like the beggining and end of a period, either today, this month, week or year.
        /// Includes current and previous optional date timestamps, calculated based on the period.
        /// </summary>
        /// <param name="period" type="Period">defines the desired period for date calculation</param>
        /// <param name="timeFrameType" type="TimeFrameType">defines the timeframe type that should ne returned</param>
        /// <returns>TimeFrame - calculated timeframes based on a period</returns>
        public static TimeFrame GetTimeFrame(Period period, TimeFrameType timeFrameType = TimeFrameType.Both)
        {
            var today = GetCurrentDate();

            switch (period)
            {
                case Period.Daily:
                    return new TimeFrame() { 
                        From = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? today : EmptyDate(),
                        To = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? today : EmptyDate(), 
                        PreviousFrom = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? today.AddDays(-1) : EmptyDate(), 
                        PreviousTo = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? today.AddDays(-1) : EmptyDate()
                    };
                case Period.Weekly:
                    return new TimeFrame() { 
                        From = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? today.AddDays(-(int)today.DayOfWeek + 1) : EmptyDate(),
                        To = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? today.AddDays(-(int)today.DayOfWeek).AddDays(7).AddSeconds(-1) : EmptyDate(),
                        PreviousFrom = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? today.AddDays(-(int)today.DayOfWeek + 1).AddDays(-7) : EmptyDate(),
                        PreviousTo = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? today.AddDays(-(int)today.DayOfWeek).AddDays(7).AddSeconds(-1).AddDays(-7) : EmptyDate()
                    };
                case Period.Monthly:
                    return new TimeFrame() { 
                        From = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, today.Month, 1) : EmptyDate(),
                        To = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1) : EmptyDate(),
                        PreviousFrom = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, today.Month, 1).AddMonths(-1) : EmptyDate(),
                        PreviousTo = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, today.Month, 1).AddDays(-1) : EmptyDate()
                    };
                case Period.Yearly:
                    return new TimeFrame() { 
                        From = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, 1, 1) : EmptyDate(),
                        To = (timeFrameType == TimeFrameType.Current || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year, 12, 31) : EmptyDate(),
                        PreviousFrom = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year - 1, 1, 1) : EmptyDate(),
                        PreviousTo = (timeFrameType == TimeFrameType.Previous || timeFrameType == TimeFrameType.Both) ? new DateTime(today.Year - 1, 12, 31) : EmptyDate()
                    };
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns a unique timestamp identificator, based on the current time.
        /// </summary>
        /// <returns>string - a unique timestamp id</returns>
        public static string GetUniqueTimestampId()
        {
            DateTime timestamp = GetCurrentDate();

            return timestamp.Month.ToString() +
                   timestamp.Day.ToString() +
                   timestamp.Hour.ToString() +
                   timestamp.Minute.ToString() +
                   timestamp.Second.ToString() +
                   timestamp.Millisecond.ToString();
        }
    }
}
