using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly INationalParkRepository _nationalRepo;
        public readonly IMapper _mapper;

        public TrailsController(INationalParkRepository nationalRepo, IMapper mapper)
        {
            _nationalRepo = nationalRepo;
            _mapper = mapper;
        }
        /// <summary>
        ///// Get list of national parks.
        ///// </summary>
        ///// <returns></returns>

        //[HttpGet]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesDefaultResponseType]
        //public IActionResult GetNationalParks()
        //{
        //    var nationParkList = _nationalRepo.GetNationalParks();

        //    var nationParkDTO = new List<NationalParkDTO>();

        //    foreach (var nationPark in nationParkList)
        //    {
        //        nationParkDTO.Add(_mapper.Map<NationalParkDTO>(nationPark));

        //    }

        //    return Ok(nationParkDTO);
        //}

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
        //    var nationalParkObj = _nationalRepo.GetNationalPark(nationalParkId);
        //    if (nationalParkObj == null)
        //    {
        //        return NotFound();
        //    }
        //    var nationalParkDTO = _mapper.Map<NationalParkDTO>(nationalParkObj);

        //    //if we do not use the automapper _mapper, we must to convert NationalPark into nationalParkDTO like this
        //    //var nationalParkDTO = new NationalPark() 
        //    //{
        //    //    Created = nationalParkObj.Created,
        //    //    Established = nationalParkObj.Established,
        //    //    Id = nationalParkObj.Id,
        //    //    Name = nationalParkObj.Name,
        //    //    State = nationalParkObj.State,
        //    //};

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
        //    if (_nationalRepo.NationalParkExists(natParkDTO.Name))
        //    {
        //        ModelState.AddModelError("", "Nation Park already exists!");
        //        return StatusCode(404, ModelState);
        //    }
        //    //if (!ModelState.IsValid) {
        //    //    return BadRequest(ModelState);
        //    //}

        //    var nationalParkObj = _mapper.Map<NationalPark>(natParkDTO);

        //    if (!_nationalRepo.CreateNationalPark(nationalParkObj))
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
        //    //if (_nationalRepo.NationalParkExists(natParkDTO.Name))
        //    //{
        //    //    ModelState.AddModelError("", "Nation Park already exists!");
        //    //    return StatusCode(404, ModelState);
        //    //}
        //    var nationalParkObj = _mapper.Map<NationalPark>(natParkDTO);
        //    if (!_nationalRepo.UpdateNationalPark(nationalParkObj))
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
        //    if (!_nationalRepo.NationalParkExists(nationalParkId))
        //    {
        //        return NotFound();
        //    }
        //    var nationalParkObj = _nationalRepo.GetNationalPark(nationalParkId);
        //    if (!_nationalRepo.DeleteNationalPark(nationalParkObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when trying to delete the record {nationalParkObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }
        //    return NoContent();
        //}
    }
}
