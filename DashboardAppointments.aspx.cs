using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace licenta_test
{
    public partial class DashboardAppointments : System.Web.UI.Page
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "a5wIauZVZA8AuMsrk92rH1zuGPYN23GcgnwUArjE",
            BasePath = "https://licenta-f7244-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDataFromFirebase();

        }
        protected void LoadDataFromFirebase()
        {
            // Inițializați configurația Firebase
            var config = new FirebaseConfig
            {
                AuthSecret = "a5wIauZVZA8AuMsrk92rH1zuGPYN23GcgnwUArjE",
                BasePath = "https://licenta-f7244-default-rtdb.firebaseio.com/"
            };

            // Inițializați clientul Firebase
            var client = new FireSharp.FirebaseClient(config);

            // Obțineți referința la colecția "Appointments" din Firebase
            var response = client.Get("Appointments");
            var appointments = response.ResultAs<Dictionary<string, App>>();

            // Parcurgeți fiecare înregistrare din colecție și adăugați datele în tabel
            foreach (var appointment in appointments)
            {
                var name = appointment.Value.Name;
                var email = appointment.Value.Email;
                var doctor = appointment.Value.Doctor;
                var phone = appointment.Value.Phone;
                var date = appointment.Value.Date;
                var time = appointment.Value.Time;

                var row = new TableRow();
                row.Cells.Add(new TableCell() { Text = name.ToString() });
                row.Cells.Add(new TableCell() { Text = email.ToString() });
                row.Cells.Add(new TableCell() { Text = doctor.ToString() });
                row.Cells.Add(new TableCell() { Text = phone.ToString() });
                row.Cells.Add(new TableCell() { Text = date.ToString() });
                row.Cells.Add(new TableCell() { Text = time.ToString() });

                appointmentsTable.Rows.Add(row);
            }
        }
    }
    
}