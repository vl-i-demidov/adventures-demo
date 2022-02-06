using System;
using System.Collections.Generic;
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
    ///     Provides methods to manage steps
    /// </summary>
    [Route("api/v1/adventures/{adventureId:guid}/steps")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class StepsController : ControllerBase
    {
        private readonly IAdventureService _adventureService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes controller.
        /// </summary>
        public StepsController(IAdventureService adventureService, IMapper mapper)
        {
            _adventureService = adventureService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Find all steps of adventure by Id
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <returns>List of steps</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(ListStepsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ListSteps(Guid adventureId)
        {
            var steps = await _adventureService.ListStepsAsync(adventureId);
            var dtoSteps = _mapper.Map<List<StepDto>>(steps);

            return Ok(new ListStepsResponse { Steps = dtoSteps });
        }

        /// <summary>
        ///     Find adventure by Id
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        /// <returns>Step</returns>
        [HttpGet("{stepId:guid}")]
        [ProducesResponseType(typeof(StepDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get(Guid adventureId, Guid stepId)
        {
            var step = await _adventureService.GetStepAsync(stepId);
            var dto = _mapper.Map<StepDto>(step);

            return Ok(dto);
        }

        /// <summary>
        ///     Create step
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="request">Create request</param>
        /// <returns>Create response</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(CreateStepResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create(Guid adventureId, CreateStepRequest request)
        {
            var id = await _adventureService.CreateStepAsync(adventureId, request.Text);

            return CreatedAtAction(nameof(Get), new { adventureId, stepId = id },
                new CreateStepResponse { Id = id });
        }

        /// <summary>
        ///     Update step
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        /// <param name="request">Update request</param>
        [HttpPatch("{stepId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Update(Guid adventureId, Guid stepId, UpdateStepRequest request)
        {
            await _adventureService.UpdateStepAsync(stepId, request.Text);

            return Ok();
        }

        /// <summary>
        ///     Delete step
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        [HttpDelete("{stepId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Delete(Guid adventureId, Guid stepId)
        {
            await _adventureService.DeleteStepAsync(stepId);

            return Ok();
        }
    }
}