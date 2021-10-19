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
    }
}
