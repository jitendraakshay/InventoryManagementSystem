//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Microsoft.AspNetCore.Html;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace TicketBookingSystem.Helper
//{
//    public static class HtmlHelper
//    {
//        public static HtmlString TimeAgo(DateTime yourDate)
//        {
//            TagBuilder tb = new TagBuilder("span");



//            const int SECOND = 1;
//            const int MINUTE = 60 * SECOND;
//            const int HOUR = 60 * MINUTE;
//            const int DAY = 24 * HOUR;
//            const int MONTH = 30 * DAY;

//            var ts = new TimeSpan(DateTime.Now.Ticks - yourDate.Ticks);
//            string Value = string.Empty;
//            double delta = Math.Abs(ts.TotalSeconds);

//            if (delta < 1 * MINUTE)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
//                }


//            if (delta < 2 * MINUTE)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value = "a minute ago";
//                }

//            if (delta < 45 * MINUTE)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += ts.Minutes + " minutes ago";
//                }

//            if (delta < 90 * MINUTE)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += "an hour ago";
//                }

//            if (delta < 24 * HOUR)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += ts.Hours + " hours ago";
//                }

//            if (delta < 48 * HOUR)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += "yesterday";
//                }

//            if (delta < 30 * DAY)
//                if (string.IsNullOrEmpty(Value))
//                {
//                    Value += ts.Days + " days ago";
//                }

//            if (delta < 12 * MONTH)
//            {
//                if (string.IsNullOrEmpty(Value))
//                {
//                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
//                    Value += months <= 1 ? "one month ago" : months + " months ago";
//                }
//            }
//            else
//            {
//                if (string.IsNullOrEmpty(Value))
//                {
//                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
//                    Value += years <= 1 ? "one year ago" : years + " years ago";
//                }
//            }



//            tb.InnerHtml = Value;
//            return new MvcHtmlString(tb.ToString());
//        }

//    }
//}