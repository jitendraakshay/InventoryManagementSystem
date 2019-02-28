using Dapper;
using DomainEntities;
using DomainInterface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace DomainRepository
{
    public class SettingsRepo : ISettingsRepo
    {
        IOptions<ReadConfig> connectionString;
        public SettingsRepo(IOptions<ReadConfig> _connectionString)
        {
            connectionString = _connectionString;
        }
        
        public bool UpdateSettings(string SettingsId, string DefaultValue)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SettingsId", SettingsId);
                    param.Add("@DefaultValue", DefaultValue);
                    //con = DBHelper.connection();
                    con.Open();
                    con.Execute("usp_SettingsUpdate", param, commandType: CommandType.StoredProcedure);
                    con.Close();


                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public Settings GetSettingsBySettingsId(string SettingsId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SettingsId", SettingsId);
                    //  con = DBHelper.connection();
                    con.Open();

                    var settings = con.Query<Settings>("usp_GetSettingsById", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
                    con.Close();

                    return settings;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Settings> GetSettings()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    con.Open();
                var settings = SqlMapper.Query<Settings>(
                                   con, "usp_GetSettings").OrderBy(x => x.OfGroup).OrderBy(x => x.SortOrder).ToList();
                con.Close();


                var sb = new StringBuilder();// for TAB 

                foreach (var group in settings.GroupBy(x => x.OfGroup))
                {

                    var tabName = group.Key;
                    if (group.First().SettingsId == settings.First().SettingsId)
                    {
                        sb.Append(GetTab(settings));

                        sb.Append("<div class='col-lg-9 col-md-9 setting-tabcontent'><div class='tabcontent' >");
                    }

                    foreach (var setting in group.OrderBy(x => x.SortOrder))
                    {

                        if (group.First().SettingsId == setting.SettingsId)
                        {

                            sb.Append("<div id='");
                            sb.Append(tabName);
                            sb.Append("' class='tab-pane fade'>");

                            sb.Append("<div class='col-md-6'>"); //id='");
                            //sb.Append(tabName);
                            //sb.Append("'>");
                            // sb.Append("<div class='row'>");




                        }
                        sb.Append("<label for='" + setting.SettingsKey + "'>");
                        sb.Append(setting.SettingsKey);
                        if (setting.Required == true)
                        {
                            sb.Append("<span style='color: red'> *</span>");
                        }
                        sb.Append("</label>");
                        if (setting.Type == "Dropdown")
                        {
                            sb.Append("<div class='form-group input-group mdl-selectfield mdl-position'>");
                        }
                        else
                        {
                            sb.Append("<div class='form-group input-group'>");
                        }
                        if (!String.IsNullOrEmpty(setting.Prefix))
                        {
                            sb.Append(" " + setting.Prefix);
                        }
                        var defaultVal = setting.SettingsValue;
                        //if (!String.IsNullOrEmpty(setting.DefaultValue))
                        //{
                        //    defaultVal = setting.DefaultValue;
                        //}
                        if (setting.Type == "TextBox")
                        {
                            if (setting.SettingsKey.ToLower().Contains("password") && setting.SettingsId!="1022")
                            {
                                sb.Append(LoadTextBoxes(setting.SettingsId, defaultVal, setting.Required, setting.Suffix, setting.ReadOnly, setting.Regex, true));
                            }
                            else
                            {
                                sb.Append(LoadTextBoxes(setting.SettingsId, defaultVal, setting.Required, setting.Suffix, setting.ReadOnly, setting.Regex, false));
                            }
                            //if (!String.IsNullOrEmpty(setting.Suffix))
                            //{
                            //    sb.Append(" " +setting.Suffix );
                            //}
                            sb.Append("</div>");//closing of formgroup

                        }
                        if (setting.Type == "TextArea")
                        {

                            sb.Append(LoadTextAreas(setting.SettingsId, defaultVal, setting.Required));
                            if (!String.IsNullOrEmpty(setting.Suffix))
                            {
                                sb.Append(" " + setting.Suffix);
                            }
                            sb.Append("</div>");//closing of formgroup

                        }
                        if (setting.Type == "RadioButton")
                        {

                            sb.Append(LoadRadioButtons(setting.Options, setting.SettingsId, defaultVal, setting.Required));
                            if (!String.IsNullOrEmpty(setting.Suffix))
                            {
                                sb.Append(" " + setting.Suffix);
                            }
                            sb.Append("</div>");//closing of formgroup

                        }
                        if (setting.Type == "Dropdown")
                        {

                            sb.Append(LoadDropDown(setting.Options, setting.SettingsId, defaultVal, setting.Required));
                            if (!String.IsNullOrEmpty(setting.Suffix))
                            {
                                sb.Append(" " + setting.Suffix);
                            }
                            sb.Append("</div>");//closing of formgroup

                        }
                        if (setting.Type == "CheckBox")
                        {

                            sb.Append(LoadCheckBoxes(setting.Options, setting.SettingsId, defaultVal));
                            if (!String.IsNullOrEmpty(setting.Suffix))
                            {
                                sb.Append(" " + setting.Suffix);
                            }
                            sb.Append("</div>");//closing of formgroup


                        }
                        if (setting.Type == "Password")
                        {

                            sb.Append(LoadPasswordBoxes(setting.SettingsId, defaultVal, setting.Required, setting.Suffix, setting.ReadOnly, setting.Regex));
                            //if (!String.IsNullOrEmpty(setting.Suffix))
                            //{
                            //    sb.Append(" " +setting.Suffix );
                            //}
                            sb.Append("</div>");//closing of formgroup

                        }

                        if (group.Last().SettingsId == setting.SettingsId)
                        {
                            sb.Append("</div></div>");//closing of tab pane

                        }
                        //if (settings.Last().SettingsId == setting.SettingsId)
                        //{
                        //    // sb.Append("</div></div>");//closing of tab content
                        //    sb.Append("</div>");

                        //}
                        //if (group.Last().SettingsId == setting.SettingsId)
                        //{

                        //    sb.Append("</div>");
                        //    //sb.Append("</div>");

                        //}
                        setting.Options = sb.ToString();
                        sb.Clear();

                    }

                }

                return settings.ToList();
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String LoadDropDown(string options, string SettingsId, string DefaultValue, bool Required)
        {

            var optionsList = JArray.Parse(options);
            var sb = new StringBuilder();

            sb.Append("<select class='form-control mdl-selectfield mdl-position' name='");
            sb.Append(SettingsId);

            if (Required == true)
            {
                sb.Append("' required />");
            }
            else
            {
                sb.Append("'>");
            }
            for (int n = 0; n < optionsList.Count; n++)
            {
                var option = JObject.Parse(optionsList[n].ToString());
                sb.Append("<option value='");
                sb.Append(option["Value"]);
                if (option["Key"].ToString().Equals(DefaultValue))
                {
                    sb.Append("' selected>");
                }
                else
                {
                    sb.Append("'>");
                }
                sb.Append(option["Value"]);
                sb.Append("</option>");
            }

            sb.Append("</select><span class='input-group-addon' id='basic-addon3'></span>");

            return sb.ToString();
        }

        public String LoadRadioButtons(string options, string SettingsId, string DefaultValue, bool Required)
        {
            var sb = new StringBuilder();

            var optionsList = JArray.Parse(options);

            for (int n = 0; n < optionsList.Count; n++)
            {
                var option = JObject.Parse(optionsList[n].ToString());

                sb.Append("<div class='radio'>");
                sb.Append("<label>");
                sb.Append("<input type='radio' name='");
                sb.Append(SettingsId);
                sb.Append("' value='");
                sb.Append(option["Value"]);
                if (option["Key"].ToString().Equals(DefaultValue))
                {
                    sb.Append("' checked>");
                }
                else
                {
                    sb.Append("'>");
                }
                sb.Append(option["Value"]);
                sb.Append("</label>");
                sb.Append("</div>");
            }

            return sb.ToString();

        }

        public String LoadCheckBoxes(string options, string SettingsId, string DefaultValue)
        {

            var sb = new StringBuilder();

            var optionsList = JArray.Parse(options);
            var checkedVal = false;
            for (int n = 0; n < optionsList.Count; n++)
            {
                var option = JObject.Parse(optionsList[n].ToString());
                sb.Append("<div class='form-group'>");
                sb.Append("<label class='check-box-type'>");
                sb.Append("<input type='checkbox' name='");
                sb.Append(SettingsId);

                sb.Append("' value='");
                sb.Append(option["Key"]);

                if (!String.IsNullOrEmpty(DefaultValue))
                {
                    var defaultChkVal = JArray.Parse(DefaultValue);
                    //var defaultChkVal = optionsList;
                    for (int i = 0; i < defaultChkVal.Count; i++)
                    {
                        var DefaultValue_FromDB = JObject.Parse(defaultChkVal[i].ToString());

                        if (option["Key"].ToString().Equals(DefaultValue_FromDB["Key"].ToString()))
                        {
                            checkedVal = true;
                        }
                    }
                    if (checkedVal == true)
                    {
                        sb.Append("' checked><span class='checkmark'></span>");
                    }
                    else
                    {
                        sb.Append("'><span class='checkmark'></span>");
                    }
                }
                else
                {
                    sb.Append("'><span class='checkmark'></span>");
                }

                sb.Append("</label>");
                sb.Append("<label><label>" + option["Value"] + "</label></label>");
                sb.Append("</div>");
                checkedVal = false;
            }

            //string chkValue = DefaultValue == "1" ? "checked='checked' value='true'" : "value='false'";

            //sb.AppendFormat(string.Format(@"

            //                            <label class='check-box-type'>
            //                                <input type='checkbox' name='{0}' {1}/>
            //                                <span class='checkmark'></span>
            //                            </label>
            //                         ",SettingsId,chkValue)); 
            return sb.ToString();

        }

        public String LoadTextBoxes(string SettingsId, string DefaultValue, bool Required, string suffix, bool readonlys, 
            string regex, bool PasswordType)
        {


            //                        < label for= "basic-url" > Your vanity URL </ label >< span style = "color: red" > *</ span >
            // < div class="form-group input-group">
            //  <input type = "text" value="30" class="form-control" id="basic-url" aria-describedby="basic-addon3">
            //  <span class="input-group-addon" id="basic-addon3">minutes</span>
            //</div>

            var sb = new StringBuilder();
            if (PasswordType)
            {
                sb.Append("<input type='password' ");
            }
            else
            {
                sb.Append("<input type='text' ");
            }
            sb.Append("name='");
            sb.Append(SettingsId);

            if (string.IsNullOrEmpty(DefaultValue))
            {
                sb.Append("' value=' '");
            }
            else
            {
                sb.Append("' value='");
                sb.Append(DefaultValue);
                sb.Append("' ");
            }
            sb.Append("class='form-control' ");
            if (!string.IsNullOrEmpty(regex))
            {
                sb.Append("pattern='" + regex + "' ");

            }
            if (Required == true)
            {
                sb.Append("required ");
            }
            if (readonlys == true)
            {
                sb.Append("readonly />");

            }
            else
            {
                sb.Append("/>");
            }
            sb.Append("<span class='input-group-addon' id='basic-addon3'>");
            if (suffix != null)
            {
                sb.Append(suffix);
            }
            sb.Append("</span>");

            return sb.ToString();
        }

        public String LoadPasswordBoxes(string SettingsId, string DefaultValue, bool Required, string suffix, bool readonlys, string regex)
        {


            //                        < label for= "basic-url" > Your vanity URL </ label >< span style = "color: red" > *</ span >
            // < div class="form-group input-group">
            //  <input type = "text" value="30" class="form-control" id="basic-url" aria-describedby="basic-addon3">
            //  <span class="input-group-addon" id="basic-addon3">minutes</span>
            //</div>

            var sb = new StringBuilder();
            sb.Append("<input type='password' ");
            sb.Append("name='");
            sb.Append(SettingsId);
            sb.Append("' value='");
            sb.Append(DefaultValue);
            sb.Append("' ");
            sb.Append("class='form-control' ");
            if (!string.IsNullOrEmpty(regex))
            {
                sb.Append("pattern='" + regex + "' ");

            }
            if (Required == true)
            {
                sb.Append("required ");
            }
            if (readonlys == true)
            {
                sb.Append("readonly />");

            }
            else
            {
                sb.Append("/>");
            }
            sb.Append("<span class='input-group-addon' id='basic-addon3'>");
            if (suffix != null)
            {
                sb.Append(suffix);
            }
            sb.Append("</span>");

            return sb.ToString();
        }


        public String LoadTextAreas(string SettingsId, string DefaultValue, bool Required)
        {
            var sb = new StringBuilder();
            sb.Append("<textarea ");
            sb.Append("name='");
            sb.Append(SettingsId);
            sb.Append("' ");
            sb.Append("class='form-control'  cols='120' ");
            if (Required == true)
            {
                sb.Append(" required />");
            }
            else
            {
                sb.Append(" />");
            }
            sb.Append(DefaultValue);
            sb.Append("</textarea>");
            return sb.ToString();
        }

        public String GetTab(IEnumerable<Settings> settings)
        {
            var sb = new StringBuilder();// for TAB 
            sb.Append("<div class='col-lg-3 col-md-3 nav-setting-tab'><ul class='nav nav-tabs'>");
            foreach (var group in settings.GroupBy(x => x.OfGroup))
            {
                var tabName = group.Key;

                sb.Append("<li class>");
                sb.Append("<a href='#");
                sb.Append(tabName);
                sb.Append("' data-toggle='tab'>");
                sb.Append(tabName);
                sb.Append("</a>");
                sb.Append("</li>");

            }
            sb.Append("</ul></div>");
            return sb.ToString();
        }

        public int GetSessionTimeout()
        {

            return 1;
        }

        public string GetSettingByIDandGroup(string SettingsID, string OfGroups)
        {


            try
            {
                using (SqlConnection con = new SqlConnection(connectionString.Value.DefaultConnection))
                {
                    DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@SettingsID", SettingsID);
                parameters.Add("@OfGroups", OfGroups);

                Settings setting = SqlMapper.Query<Settings>(con, "usp_SettingsByIdandGroup", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                string SettingValue = "";
                if (!String.IsNullOrEmpty(setting.SettingsValue))
                {
                    SettingValue = setting.SettingsValue;
                }
                else if (!String.IsNullOrEmpty(setting.DefaultValue))
                {
                    SettingValue = setting.DefaultValue;
                }

                return SettingValue;
            }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
