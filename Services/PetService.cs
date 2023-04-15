using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_backend.Services
{
    public class PetService : IPetService
    {
        private readonly IRepositoryManager _repository;
        public PetService(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public StatusCode CreatePet(Pet pet)
        {
            if (pet == null)
            {
                return StatusCode.DoesNotExist;
            }
            else if (_repository.Pet.GetByCondition(x => x.PetName == pet.PetName, false).FirstOrDefault() != null)
            {
                return StatusCode.IsAlredyExist;
            }

            _repository.Pet.Create(pet);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }

            return StatusCode.Ok;
        }
    }
}
