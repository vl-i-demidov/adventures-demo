using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Demo.Adventures.Api.Contracts.Adventures;
using Demo.Adventures.Logic.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Adventures.Api.Controllers
{
    /// <summary>
    /// Provides methods to manage step options
    /// </summary>
    [Route("api/v1/adventures/{adventureId:guid}/steps/{stepId:guid}/options")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class OptionsController : ControllerBase
    {
        private readonly IAdventureService _adventureService;

        /// <summary>
        /// Initializes controller.
        /// </summary>
        public OptionsController(IAdventureService adventureService)
        {
            _adventureService = adventureService;
        }

        /// <summary>
        /// Create step option
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        /// <param name="request">Create request</param>
        /// <returns>Create response</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(CreateOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Create(Guid adventureId, Guid stepId, CreateOptionRequest request)
        {
            var id = await _adventureService.CreateOptionAsync(stepId, request.Text, request.NextStepId);

            return Ok(new CreateOptionResponse { Id = id });
        }

        /// <summary>
        /// Update step option
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        /// <param name="optionId">Option id</param>
        /// <param name="request">Update request</param>
        [HttpPatch("{optionId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Update(Guid adventureId, Guid stepId, Guid optionId,
            UpdateOptionRequest request)
        {
            await _adventureService.UpdateOptionAsync(stepId, optionId, request.Text, request.NextStepId);

            return Ok();
        }

        /// <summary>
        /// Delete step option
        /// </summary>
        /// <param name="adventureId">Adventure Id</param>
        /// <param name="stepId">Step Id</param>
        /// <param name="optionId">Option id</param>
        [HttpDelete("{optionId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Delete(Guid adventureId, Guid stepId, Guid optionId)
        {
            await _adventureService.DeleteOptionAsync(stepId, optionId);

            return Ok();
        }
    }
}