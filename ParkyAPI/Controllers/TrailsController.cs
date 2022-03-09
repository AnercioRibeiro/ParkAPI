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
        //private readonly INationalParkRepository _nationalRepo;
        public readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //public NationalParksController(INationalParkRepository nationalRepo, IMapper mapper)
        //{
        //    _nationalRepo = nationalRepo;
        //    _mapper = mapper;
        //}
        public TrailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
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
            var trailList = _unitOfWork.Trail.GetTrails();

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
        public IActionResult GetTrail(int trailId)
        {
            var trailObj = _unitOfWork.Trail.GetTrail(trailId);
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
            if (_unitOfWork.Trail.TrailExists(trailCreateDTO.Name))
            {
                ModelState.AddModelError("", "Trail already exists!");
                return StatusCode(404, ModelState);
            }
            //if (!ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}

            var trailObj = _mapper.Map<Trail>(trailCreateDTO);

            if (!_unitOfWork.Trail.CreateTrail(trailObj))
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
            if (!_unitOfWork.Trail.UpdateTrail(trailObj))
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
            if (!_unitOfWork.Trail.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _unitOfWork.Trail.GetTrail(trailId);
            if (!_unitOfWork.Trail.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when trying to delete the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
