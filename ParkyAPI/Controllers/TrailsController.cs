using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }
        //public TrailsController(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}
        /// <summary>
        /// Get list of trails.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
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

        public IActionResult GetTrails()
        {
            var trailList = _trailRepo.GetTrails();

            var trailDTO = new List<TrailDTO>();

            foreach (var trail in trailList)
            {
                trailDTO.Add(_mapper.Map<TrailDTO>(trail));

            }

            return Ok(trailDTO);
        }

        /// <summary>
        /// Get individual trial
        /// </summary>
        /// <param name="trailId">The Id of trail </param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetTrail(int trailId)
        {
            var trailObj = _trailRepo.GetTrail(trailId);
            if (trailObj == null)
            {
                return NotFound();
            }
            var trailDTO = _mapper.Map<TrailDTO>(trailObj);

            //if we do not use the automapper _mapper, we must to convert NationalPark into nationalParkDTO like this
            //var nationalParkDTO = new NationalPark() 
            //{
            //    Created = nationalParkObj.Created,
            //    Established = nationalParkObj.Established,
            //    Id = nationalParkObj.Id,
            //    Name = nationalParkObj.Name,
            //    State = nationalParkObj.State,
            //};

            return Ok(trailDTO);
        }

        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var trailObjList = _trailRepo.GetTrailInNationalPark(nationalParkId);
            if (trailObjList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDTO>();
            foreach (var obj in trailObjList)
            {
                
                objDto.Add(_mapper.Map<TrailDTO>(obj));
            }
            

            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type =typeof(List<TrailDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDTO trailCreateDTO)
        {
            if (trailCreateDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailCreateDTO.Name))
            {
                ModelState.AddModelError("", "Trail already exists!");
                return StatusCode(404, ModelState);
            }
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}

            var trailObj = _mapper.Map<Trail>(trailCreateDTO);

            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when trying to saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        } 

        [HttpPatch("{trailId:int}", Name ="Get")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDTO trailUpdateDTO)
        {
            if (trailUpdateDTO == null || trailId!= trailUpdateDTO.Id)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailUpdateDTO);
            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when trying to updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name ="DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when trying to delete the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
