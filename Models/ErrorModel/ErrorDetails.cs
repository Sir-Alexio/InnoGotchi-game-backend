using InnoGotchi_backend.Models.Enums;
using System.Text.Json;

namespace InnoGotchi_backend.Models.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
