using InnoGotchi_backend.Models.Enums;
using System;
namespace InnoGotchi_backend.Models.Entity
{
    public class CustomExeption : Exception
    {
        public CustomExeption(string message) : base(message)
        {
        }
        public StatusCode StatusCode { get; set; }
    }
}
