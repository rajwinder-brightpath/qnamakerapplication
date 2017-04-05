using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;

namespace QnAMakerDialog.Sample
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        internal static IDialog<object> MakeRoot()
        {
            
            return Chain.From(() => new Dialogs.QnADialog("16c5a1e70abf426aa3b3f97604b292da", "5ab0bc00-b599-4bbd-bc9d-ff81611949aa"));
         
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonstring =json.Serialize(activity);
             

            //using (var db = new QnaApplicationEntities())
            //{

            //    IQueryable <qnadialogtable> ret = from x in db.qnadialogtables
            //                select x;


            //    ret.ToList();


            //}
            if (activity.Type == ActivityTypes.Message)
            {
                try
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    Activity replyMessage = activity.CreateReply(jsonstring);
                    
                    await connector.Conversations.ReplyToActivityAsync(replyMessage);
                    //await Conversation.SendAsync(activity, MakeRoot);
                    
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
              
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}