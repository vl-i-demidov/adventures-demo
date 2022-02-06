using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Adventures.Api.Contracts.Adventures;
using Demo.Adventures.Api.Contracts.Games;
using Demo.Adventures.Logic.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Adventures.Api.Controllers
{
    /// <summary>
    ///     Provides methods to play a game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes controller.
        /// </summary>
        public GamesController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Start game
        /// </summary>
        /// <param name="request">Start game request</param>
        /// <returns>Start game response</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(StartGameResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Start(StartGameRequest request)
        {
            var id = await _gameService.StartGameAsync(request.AdventureId, GetUserId());

            return CreatedAtAction(nameof(Get), new { gameId = id },
                new StartGameResponse { GameId = id });
        }

        /// <summary>
        ///     Select option
        /// </summary>
        /// <param name="gameId">Game id</param>
        /// <param name="request">Select option request</param>
        /// <returns>Select option response</returns>
        [HttpPost("{gameId:guid}/select")]
        [ProducesResponseType(typeof(SelectOptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> SelectOption(Guid gameId, SelectOptionRequest request)
        {
            var nextStep = await _gameService.SelectOptionAsync(gameId, request.StepId, request.OptionId);
            var nextStepDto = _mapper.Map<StepDto>(nextStep);

            return Ok(new SelectOptionResponse { NextStep = nextStepDto });
        }

        /// <summary>
        ///     Find game by Id.
        /// </summary>
        /// <param name="gameId">Game Id.</param>
        /// <returns>Game (including selected options)</returns>
        [HttpGet("{gameId:guid}")]
        [ProducesResponseType(typeof(GetGameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Get(Guid gameId)
        {
            var (game, adventure, gameSteps) = await _gameService.GetFullGameAsync(gameId);

            var response = new GetGameResponse
            {
                GameId = game.Id,
                UserId = game.UserId,
                Adventure = _mapper.Map<AdventureDto>(adventure),
                Steps = _mapper.Map<List<GameStepDto>>(gameSteps)
            };

            return Ok(response);
        }

        // assume the service is behind some API Gateway that is responsible for Authentication
        // and puts userId into a predefined http header
        private Guid GetUserId()
        {
            return Guid.Empty;
        }
    }
}