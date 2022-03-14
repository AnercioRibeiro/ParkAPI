using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
   //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;
        public readonly INationalParkRepository _nationRepo;
        public NationalParksV2Controller(INationalParkRepository nationRepo, IMapper mapper)
        {
            _nationRepo = nationRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalParks()
        {
            var obj = _nationRepo.GetNationalParks().FirstOrDefault();

        

            return Ok(_mapper.Map<NationalParkDTO>(obj));
        }

        ///// <summary>
        ///// Get individual national park
        ///// </summary>
        ///// <param name="nationalParkId">The Id of national park</param>
        ///// <returns></returns>
        //[HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        //[ProducesResponseType(200, Type = typeof(NationalParkDTO))]
        //[ProducesResponseType(404)]
        //[ProducesDefaultResponseType]
        //public IActionResult GetNationalPark(int nationalParkId)
        //{
        //    var nationalParkObj = _unitOfWork.NationalPark.GetNationalPark(nationalParkId);
        //    if (nationalParkObj == null)
        //    {
        //        return NotFound();
        //    }
        //    var nationalParkDTO = _mapper.Map<NationalParkDTO>(nationalParkObj);

        //    return Ok(nationalParkDTO);
        //}

        //[HttpPost]
        //[ProducesResponseType(201, Type =typeof(List<NationalParkDTO>))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult CreateNationalPark([FromBody] NationalParkDTO natParkDTO)
        //{
        //    if (natParkDTO == null)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (_unitOfWork.NationalPark.NationalParkExists(natParkDTO.Name))
        //    {
        //        ModelState.AddModelError("", "Nation Park already exists!");
        //        return StatusCode(404, ModelState);
        //    }

        //    var nationalParkObj = _mapper.Map<NationalPark>(natParkDTO);

        //    if (!_unitOfWork.NationalPark.CreateNationalPark(nationalParkObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when trying to saving the record {nationalParkObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        //} 

        //[HttpPatch("{nationalParkId:int}", Name ="GetNationalPark")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDTO natParkDTO)
        //{
        //    if (natParkDTO == null || nationalParkId!= natParkDTO.Id)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var nationalParkObj = _mapper.Map<NationalPark>(natParkDTO);
        //    if (!_unitOfWork.NationalPark.UpdateNationalPark(nationalParkObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when trying to updating the record {nationalParkObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}

        //[HttpDelete("{nationalParkId:int}", Name ="DeleteNationalPark")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult DeleteNationalPark(int nationalParkId)
        //{
        //    if (!_unitOfWork.NationalPark.NationalParkExists(nationalParkId))
        //    {
        //        return NotFound();
        //    }
        //    var nationalParkObj = _unitOfWork.NationalPark.GetNationalPark(nationalParkId);
        //    if (!_unitOfWork.NationalPark.DeleteNationalPark(nationalParkObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when trying to delete the record {nationalParkObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}
    }
}
