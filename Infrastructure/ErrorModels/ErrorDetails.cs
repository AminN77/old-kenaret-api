using Newtonsoft.Json;

namespace Infrastructure.ErrorModels
{
    public class ErrorDetails
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}