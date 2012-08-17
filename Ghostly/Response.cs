namespace Ghostly
{
    public class Response
    {
        public Response(int code, string message, string body)
        {
            this.code = code;
            this.message = message;
            this.body = body;
        }

        public int code { get; set; }
        public string message { get; set; }
        public string body { get; set; }
    }
}