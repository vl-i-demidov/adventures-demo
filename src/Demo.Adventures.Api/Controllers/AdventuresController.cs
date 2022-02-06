using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Adventures.Api.Contracts.Adventures;
using Demo.Adventures.Logic.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Adventures.Api.Controllers
{
    /// <summary>
    ///     Provides methods to manage adventures
    /// </summary>
    [Route("api/v1/adventures")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class AdventuresController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes controller.
        /// </summary>
        public AdventuresController(IAdventureService adventureService, IMapper mapper)
        {
            _adventureService = adventureService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Find adventure by Id
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <returns>Adventure</returns>
        [HttpGet("{adventureId:guid}")]
        [ProducesResponseType(typeof(AdventureDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get(Guid adventureId)
        {
            var adventure = await _adventureService.GetAdventureAsync(adventureId);
            var dto = _mapper.Map<AdventureDto>(adventure);

            return Ok(dto);
        }

        /// <summary>
        ///     Create adventure
        /// </summary>
        /// <param name="request">Create request</param>
        /// <returns>Create response</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(CreateAdventureResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create(CreateAdventureRequest request)
        {
            var id = await _adventureService.CreateAdventureAsync(request.Title);

            return CreatedAtAction(nameof(Get), new { adventureId = id }, new CreateAdventureResponse { Id = id });
        }

        /// <summary>
        ///     Update adventure
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="request">Update request</param>
        [HttpPatch("{adventureId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Update(Guid adventureId, UpdateAdventureRequest request)
        {
            await _adventureService.UpdateAdventureAsync(adventureId, request.Title, request.FirstStepId);

            return Ok();
        }
    }
}