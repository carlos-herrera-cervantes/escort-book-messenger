using Newtonsoft.Json;

namespace EscortBookMessenger.Models;

public class RequestorsMessage
{
    #region snippet_Properties

    [JsonProperty("to")]
    public string To { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    #endregion

    #region snippet_Deconstructors

    public void Deconstruct(out string to, out string subject, out string body)
    {
        to = To;
        subject = Subject;
        body = Body;
    }

    #endregion
}
