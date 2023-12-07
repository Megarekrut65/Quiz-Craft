using Newtonsoft.Json;

namespace Global.Data
{
    public class Error
    {
        public string Detail { get; set; }

        public static Error FromMessage(string message)
        {
            Error error = JsonConvert.DeserializeObject<Error>(message);
            return error ?? new Error{Detail=message};
        }
    }
}