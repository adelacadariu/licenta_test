using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Apis.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace licenta_test
{
    public partial class Diary : System.Web.UI.Page
    {
        int lastEntryId, newEntryId;
       
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "a5wIauZVZA8AuMsrk92rH1zuGPYN23GcgnwUArjE",
            BasePath = "https://licenta-f7244-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (client != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["username"]))
                {
                    string username = Request.QueryString["username"];

                    IFirebaseClient client = new FireSharp.FirebaseClient(config);

                    // Accesați baza de date Firebase pentru a obține numele și adresa de email corespunzătoare username-ului
                    FirebaseResponse response = client.Get("Users/" + username);
                    User existinguser = response.ResultAs<User>();

                    // Verificați dacă s-a găsit un psiholog corespunzător în baza de date Firebase
                    if (existinguser != null)
                    {
                        submitButton.Enabled = false;
                    }

                }
  
            }
        }

        protected async void submitButton_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(entryTitleTextBox.Text) && !string.IsNullOrEmpty(entryTextBox.Text))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["username"]))
                {
                    string username = Request.QueryString["username"];
                    IFirebaseClient client = new FireSharp.FirebaseClient(config);
                    // Accesați baza de date Firebase pentru a obține numele și adresa de email corespunzătoare username-ului
                    FirebaseResponse userResponse = await client.GetAsync("Users/" + username.ToString());
                    User existingUser = userResponse.ResultAs<User>();


                    FirebaseResponse lastEntryIdResponse = await client.GetAsync("Journals/" + username + "/lastEntryId");
                    int lastEntryId;
                    int newEntryId;

                    if (lastEntryIdResponse == null || !int.TryParse(lastEntryIdResponse.Body, out lastEntryId))
                    {
                        lastEntryId = 0;
                        newEntryId = lastEntryId + 1;
                    }
                    else
                    {
                        newEntryId = lastEntryId + 1;
                    }

                    // Incrementați ID-ul de intrare

                    FirebaseResponse response = await client.GetAsync("Journals/" + username);
                    Journal existingJournal = response.ResultAs<Journal>();

                    if (existingJournal == null || (existingJournal.Title.ToString() != entryTitleTextBox.Text))
                    {
                    

                        Journal newJournal = new Journal
                        {
                            Name = existingUser.Name.ToString(),
                            Entry_Id = newEntryId,
                            Title = entryTitleTextBox.Text,
                            Text = entryTextBox.Text
                        };

                        // Salvare în Firebase
                        SetResponse saveResponse = await client.SetAsync("Journals/" + username + "/entries/" + newEntryId, newJournal);

                        if (saveResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // Actualizarea ultimului ID de intrare pentru utilizatorul curent
                            SetResponse updateEntryIdResponse = await client.SetAsync("Journals/" + username + "/lastEntryId", newEntryId);

                            // Resetați formularul
                            entryTextBox.Text = string.Empty;
                            entryTitleTextBox.Text = string.Empty;
                        }
                        else
                        {
                            // Salvarea a eșuat
                            string alert = "alert(\"Salvarea a eșuat!\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", alert, true);
                        }
                    }
                    else
                    {
                        // Utilizatorul nu este autentificat
                        string alert = "alert(\"Utilizatorul nu este autentificat!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", alert, true);
                    }
                }
                else
                {
                    submitButton.Enabled= false;
                }
            }
            else
            {
                // Câmpurile trebuie completate
                string emptyFieldsScript = "alert(\"Vă rugăm completați toate câmpurile!\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", emptyFieldsScript, true);
            }

        }
    }
}