using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Google.Apis.Requests.BatchRequest;

namespace licenta_test
{
    public partial class Depression : System.Web.UI.Page
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

        }
        protected async void LoadQuizQuestions()
        {
            FirebaseResponse questionsResponse = await client.GetAsync("Quiz_1");
            Dictionary<string, Question> questions = questionsResponse.ResultAs<Dictionary<string, Question>>();


            int questionCount = 0;
            foreach (var question in questions)
            {
                if (questionCount >= 10)
                    break;

                string questionKey = question.Key;
                Question questionObject = question.Value;
                string questionText = questionObject.Text;

                Panel questionContainer = new Panel();
                questionContainer.CssClass = "question-container";

                Label questionLabel = new Label();
                questionLabel.Text = questionText;

                questionContainer.Controls.Add(questionLabel);

                FirebaseResponse answersResponse = await client.GetAsync("Answers");
                Dictionary<string, Answer> answers = answersResponse.ResultAs<Dictionary<string, Answer>>();

                int answerCount = 0;
                foreach (var answer in answers)
                {
                    if (answerCount >= 4)
                        break;

                    string answerKey = answer.Key;
                    Answer answerObject = answer.Value;
                    string answerText = answerObject.Value;
                    int points = answerObject.Points;

                    RadioButton choiceRadioButton = new RadioButton();
                    choiceRadioButton.GroupName = "question_" + questionKey;
                    choiceRadioButton.Text = answerText;
                    choiceRadioButton.Attributes.Add("data-points", points.ToString());

                    questionContainer.Controls.Add(choiceRadioButton);

                    answerCount++;
                }

                quizContainer.Controls.Add(questionContainer);

                questionCount++;
            }
        }


    }
}