namespace Uk.Parliament
{
    [DataContract]
    public class Response
    {
        [DataMember("links")]
        public Links Links {get;set;}

        [DataMember("data")]
        public List<DataObject> Data {get;set;}
    }

    [DataContract]
    public class Links
    {
        [DataMember("self")]
        public string Self {get;set;}

        [DataMember("first")]
        public string First {get;set;}

        [DataMember("last")]
        public string Last {get;set;}

        [DataMember("next")]
        public string Next {get;set;}

        [DataMember("prev")]
        public string Previous {get;set;}
    }

    [DataContract]
    public class PetitionAttributes
    {
        [DataMember("action")]
        public string Self {get;set;}

        [DataMember("background")]
        public string First {get;set;}

        [DataMember("additional_details")]
        public string Last {get;set;}

        [DataMember("state")]
        public PetitionState State {get;set;}

        [DataMember("signature_count")]
        public int SignatureCount {get;set;}

        [DataMember("created_at")]
        public DateTime CreatedAtUtc {get;set;}

        [DataMember("updated_at")]
        public DateTime? UpdatedAtUtc {get;set;}

        [DataMember("open_at")]
        public DateTime OpenedAtUtc {get;set;}

        [DataMember("closed_at")]
        public DateTime? ClosedAtUtc {get;set;}
    }
}