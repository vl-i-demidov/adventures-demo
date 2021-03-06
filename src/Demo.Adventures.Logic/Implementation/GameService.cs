using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;
using Demo.Adventures.Logic.Contracts;

namespace Demo.Adventures.Logic.Implementation
{
    public class GameService : IGameService
    {
        private readonly IAdventureService _adventureService;
        private readonly IGameRepository _repo;

        public GameService(IGameRepository repo, IAdventureService adventureService)
        {
            _repo = repo;
            _adventureService = adventureService;
        }

        public async Task<Guid> StartGameAsync(Guid adventureId, Guid userId)
        {
            var game = Game.Create(adventureId, userId);

            await _repo.CreateGameAsync(game);
            return game.Id;
        }

        public async Task<Step> SelectOptionAsync(Guid gameId, Guid stepId, Guid optionId)
        {
            var currentStep = await _adventureService.GetStepAsync(stepId);

            // todo: check option exists
            var selectedOption = currentStep.Options.First(o => o.Id == optionId);

            var nextStep = await _adventureService.GetStepAsync(selectedOption.NextStepId);

            var option = new SelectedOption(currentStep.Id, optionId);
            await _repo.AddSelectedOptionAsync(gameId, option);

            return nextStep;
        }

        public Task<Game> GetGameAsync(Guid gameId)
        {
            return _repo.GetGameAsync(gameId);
        }

        public async Task<(Game game, Adventure adventure, List<GameStep> gameSteps)> GetFullGameAsync(Guid gameId)
        {
            var game = await GetGameAsync(gameId);
            var adventure = await _adventureService.GetAdventureAsync(game.AdventureId);
            var allSteps = await _adventureService.ListStepsAsync(game.AdventureId);

            // steps where user selected an option
            var gameSteps = game.SelectedOptions.Join(allSteps, selOpt => selOpt.StepId, step => step.Id,
                (selOpt, step) => new GameStep(step, selOpt.OptionId)).ToList();

            if (gameSteps.Any())
            {
                // resolve last step - the last selected option points to this step
                // todo: check consistency (do not use First())
                var lastStepWithOptions = gameSteps.Last();
                var lastSelectedOption = lastStepWithOptions.Step.Options
                    .First(o => o.Id == lastStepWithOptions.SelectedOptionId);
                var lastStep = allSteps.First(s => s.Id == lastSelectedOption.NextStepId);
                gameSteps.Add(new GameStep(lastStep));
            }

            return (game, adventure, gameSteps);
        }
    }
}