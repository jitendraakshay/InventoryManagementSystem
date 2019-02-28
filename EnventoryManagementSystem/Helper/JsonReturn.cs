using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketBookingSystem
{
    public class JsonReturn
    {
        #region Properties

        #endregion

        #region Methods
        #region UnAuthorized
        public static string UnAuthorized()
        {
            dynamic result = new JObject();
            result.result = true;
            result.authentication = false;
            result.message = "You don't have access.";
            return result.ToString(Formatting.None);
        }
        #endregion
        #region True
        public static Dictionary<object, object> True(object key, object value)
        {
            Dictionary<object, object> result = new Dictionary<object, object> {
            { "result", true},
            { key, value },
            { "message", string.Empty } };
            return result;
        }
        public static dynamic True()
        {
            dynamic result = new JObject();
            result.result = true;
            result.message = string.Empty;
            return result;
        }
        public static Dictionary<object, object> True(Dictionary<object, object> param)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            result.Add("result", true);
            foreach (KeyValuePair<object, object> obj in param)
            {
                result.Add(obj.Key, obj.Value);
            }
            result.Add("message", string.Empty);
            return result;
        }
        public static dynamic True(IEnumerable<Object> a)
        {
            dynamic result = new JObject();
            result.result = true;
            result.items = JArray.Parse(JsonConvert.SerializeObject(a));
            result.message = string.Empty;
            return result;
        }
        public static object True(Object a)
        {
            a.GetType().GetProperties();

            dynamic result = new JObject();
            result.result = true;
            result.item = JObject.Parse(JsonConvert.SerializeObject(a));
            result.message = string.Empty;
            return result;
        }
        #endregion
        #region False
        public static dynamic False(string message)
        {
            dynamic result = new JObject();
            result.result = false;
            result.message = message;
            return result;
        }
        public static dynamic False()
        {
            dynamic result = new JObject();
            result.result = false;
            result.message = "Some error Occured.";
            return result;
        }
        #endregion 
        #endregion
    }
}