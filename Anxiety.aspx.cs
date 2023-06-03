using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;


namespace licenta_test
{
    public partial class Anxiety : System.Web.UI.Page
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "a5wIauZVZA8AuMsrk92rH1zuGPYN23GcgnwUArjE",
            BasePath = "https://licenta-f7244-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        private int total_points;
        protected void Page_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                LoadQuizQuestions();
            }
               

        }
        
        protected void submitButton_Click(object sender, EventArgs e)
        {
            CalculateTotalPoints();
        }
        private async void LoadQuizQuestions()
        {
            FirebaseResponse questionsResponse = await client.GetAsync("Quiz2");
            var questions = questionsResponse.ResultAs<Dictionary<object, Question>>();

            int questionCount = 0;
            foreach (var question in questions)
            {
                if (questionCount >= 10)
                    break;

                object questionKey = question.Key;
                Question questionObject = question.Value;
                object questionText = question.Value.Text;

                Panel questionContainer = new Panel();
                questionContainer.CssClass = "question-container";

                Label questionLabel = new Label();
                questionLabel.Text = questionText.ToString();

                questionContainer.Controls.Add(questionLabel);

                FirebaseResponse answersResponse = await client.GetAsync("Answers");
                var answers = answersResponse.ResultAs<Dictionary<string, Answer>>();

                int answerCount = 0;
                foreach (var answer in answers)
                {
                    if (answerCount >= 4)
                        break;

                    string answerKey = answer.Key;
                    Answer answerObject = answer.Value;
                    object answerText = answerObject.Value.ToString();
                    int points = answerObject.Points;

                    RadioButton choiceRadioButton = new RadioButton();
                    choiceRadioButton.GroupName = "question_" + questionKey;
                    choiceRadioButton.Text = answerText.ToString();
                    choiceRadioButton.Attributes.Add("data-points", points.ToString());

                    questionContainer.Controls.Add(choiceRadioButton);

                    answerCount++;
                }

                quizContainer.Controls.Add(questionContainer);

                questionCount++;
            }
        }

        private void CalculateTotalPoints()
        {
            total_points = 0;

            foreach (Control questionContainer in quizContainer.Controls)
            {
                foreach (Control choiceControl in questionContainer.Controls)
                {
                    if (choiceControl is RadioButton)
                    {
                        RadioButton choiceRadioButton = (RadioButton)choiceControl;

                        if (choiceRadioButton.Checked)
                        {
                            int points = Convert.ToInt32(choiceRadioButton.Attributes["data-points"]);
                            total_points += points;
                            break;
                        }
                    }
                }

                // Afișează scorul sau efectuează alte acțiuni în funcție de nevoile tale
                // Exemplu:
                Response.Write("Score: " + total_points);
            }
        }
    }
}