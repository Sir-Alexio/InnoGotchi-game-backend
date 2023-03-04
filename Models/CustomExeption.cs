using System;
namespace InnoGotchi_backend.Models
{
    public class CustomExeption: Exception
    {
        public CustomExeption(string message):base(message)
        {
        }
        public int StatusCode { get; set; }
    }
}
