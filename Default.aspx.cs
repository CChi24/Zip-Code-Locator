using System;
using System.Collections.Generic;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Zip_Code_Finder
{

    public partial class Default : System.Web.UI.Page
    {
        //HttpClient is declared as a global variable so we can have access to it in all our functions because
        //we do not want to keep making a new HttpClient object due prevent overworking the server.
        HttpClient httpClient = new HttpClient();
        Dictionary<string, string> City_Search_List = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs eventArgs)
        {
            //!IsPostBack is to check to see if the page is being loaded for the first time.
            if (!IsPostBack)
            {
                ViewState["City_List"] = null;
                div_city_info.Style.Add("visibility", "hidden");
                btn_add_to_list.Visible = false;
                lbl_city_list.Visible = false;

                Print_List();
            }
            else
            {
                //making sure that the City_Search_List is set to the most recent version of the list after any additions to the list.
                if (ViewState["City_List"] != null)
                {
                    City_Search_List = (Dictionary<string, string>)ViewState["City_List"];
                }
            }
        }

        /// <summary>
        /// The event function for when the user clicks the Submit Button.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void btn_Submit_ZipCode(object sender, EventArgs e)
        {
            if (Check_ZipCode_TB())
            {
                lbl_all_alerts.Visible = false;

                //using the HttpClient object, we begin making a connection to the Zippotam.us API by using the URL and setting up the API call to get all searched data in a JSON format.
                httpClient.BaseAddress = new Uri("https://api.zippopotam.us/us/" + tb_enterZip.Text);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //When we send a ping to the API, we check to make sure data, based on what zip code was entered in the textbox, was successfully returned.
                HttpResponseMessage apiResponse = httpClient.GetAsync("https://api.zippopotam.us/us/" + tb_enterZip.Text).Result;
                if (apiResponse.IsSuccessStatusCode)
                {
                    //We analyze the JSON data returned from the API and begin to break down the JSON into accessible pieces of data through the index name featured on the API.
                    Task<string> cityInfoResults = apiResponse.Content.ReadAsStringAsync();
                    JsonValue cityInfo_Json = JsonValue.Parse(cityInfoResults.Result);

                    //Assiging text values to the Label control using the JSON indexes.
                    lbl_zip.Text = CleanUp_JSON(cityInfo_Json["post code"].ToString());
                    lbl_city.Text = CleanUp_JSON(cityInfo_Json["places"][0]["place name"].ToString()) + ", " + CleanUp_JSON(cityInfo_Json["places"][0]["state"].ToString());
                    lbl_long_and_lat.Text = String.Format("Longitude: {0}<br/>Latitude: {1}", CleanUp_JSON(cityInfo_Json["places"][0]["longitude"]), CleanUp_JSON(cityInfo_Json["places"][0]["latitude"]));

                    //showing the return information by unhiding the content.
                    div_city_info.Style.Remove("visibility:hidden");
                    div_city_info.Style.Add("visibility", "visible");

                    btn_add_to_list.Visible = true;
                    tb_enterZip.Text = "";
                }
                else
                {
                    //if there is no sort of data returned from the API, we reveal an alert indicating nothing was found.
                    lbl_all_alerts.Text = "No ZIP code data is available! Please try another ZIP code.";
                    lbl_all_alerts.Visible = true;
                    btn_add_to_list.Visible = false;
                    div_city_info.Style.Add("visibility", "hidden");
                }
            }
        }


        /// <summary>
        /// The event function for the Add button to add searched zip codes into a list.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void btn_Add_To_List_Click(object sender, EventArgs e)
        {
            //With ViewState, we are going to save all the data that was added to the ZIP Code search list and make sure the list is the same throughout page reloads and/or user session.
            if (ViewState["City_List"] != null)
            {
                City_Search_List = (Dictionary<string, string>)ViewState["City_List"];
            }

            //checks the list to make sure the searched zip code is not within the list already. 
            //If item is in the list, throw a warning that it has already been add. If item is not in the list already, add search results to the list.
            if (!City_Search_List.ContainsKey(lbl_zip.Text))
            {
                City_Search_List.Add(lbl_zip.Text, lbl_city.Text + "<br/>" + lbl_long_and_lat.Text);

                //update the ViewState to have the latest version of the City_Search_List.
                ViewState["City_List"] = City_Search_List;

                //call function to print the contents of the City_Search_List.
                Print_List();

                lbl_all_alerts.Visible = false;
                btn_add_to_list.Visible = false;
                div_city_info.Style.Add("visibility", "hidden");
            }
            else
            {
                lbl_all_alerts.Text = "The ZIP code has already been added to the list.";
                lbl_all_alerts.Visible = true;
            }

            btn_add_to_list.Visible = false;
            div_city_info.Style.Add("visibility", "hidden");
        }

        /// <summary>
        /// Checks the zip code textbox for any bad inputs such as no zip code, zip code without 5 numbers, and zip code with letter and/or symbols
        /// </summary>
        /// <returns><c>true</c>, if zip code tb was checked, <c>false</c> otherwise.</returns>
        protected Boolean Check_ZipCode_TB()
        {
            bool goodZipCode_YN = false;
            int zipcode;

            if (tb_enterZip.Text.Length == 5)
            {
                if (Int32.TryParse(tb_enterZip.Text, out zipcode))
                {
                    goodZipCode_YN = true;
                    lbl_all_alerts.Visible = false;
                }
                else
                {
                    //ZIP code with letter and/or symbols error catcher

                    lbl_all_alerts.Text = "Please make sure the ZIP code you entered is a 5 digit number with no letters and/or no symbols.";
                    lbl_all_alerts.Visible = true;
                    div_city_info.Style.Add("visibility", "hidden");
                    btn_add_to_list.Visible = false;
                }
            }
            else
            {
                //Empty Textbox error catcher
                if (tb_enterZip.Text == "")
                {
                    lbl_all_alerts.Text = "Please make sure you have entered a ZIP code before submitting.";
                    lbl_all_alerts.Visible = true;
                    div_city_info.Style.Add("visibility", "hidden");
                    btn_add_to_list.Visible = false;
                }
                else
                {
                    //ZIP codes that do not have 5 digits error catcher.

                    lbl_all_alerts.Text = "Please make sure your ZIP code has 5 digits.";
                    lbl_all_alerts.Visible = true;
                    div_city_info.Style.Add("visibility", "hidden");
                    btn_add_to_list.Visible = false;
                }
            }

            return goodZipCode_YN;
        }

        /// <summary>
        /// Prints the City_Search_List that contains all the added ZIP code searches.
        /// </summary>
        protected void Print_List()
        {
            string searchHistory = "";
            lbl_city_list.Visible = true;

            if (City_Search_List.Count <= 0)
            {
                //empty list returns a warning saying nothing was added.
                lbl_city_list.Text = "No ZIP code Search History";
            }
            else
            {
                //if there is something in the list, it loops through the list and prints out all values of the list.
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in City_Search_List)
                {
                    i += 1;
                    searchHistory += "<hr/><u>" + i + ".</u>" + "<br/>ZIP code: " + kvp.Key + "<br/>&emsp;" + kvp.Value + "<br/>";
                }

                lbl_city_list.Text = searchHistory;
            }
        }

        //the parsed JSON contains double quotes, so we want to get rid of those. This function replaces those double quotes with nothing.
        protected string CleanUp_JSON(string jsonValue)
        {
            return jsonValue.Replace("\"", "");
        }
    }
}
