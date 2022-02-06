using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Domain;
using Demo.Adventures.Logic.Contracts;
using Demo.Adventures.Logic.Implementation;
using Demo.Adventures.Tests.Mocks.Database;
using NUnit.Framework;

namespace Demo.Adventures.Tests.Unit
{
    // only positive tests included
    // todo: negative tests
    [Category("Unit")]
    public class GameServiceTests
    {
        [Test]
        public async Task PlayedGame_HasValidDecisionTree()
        {
            var adventureRepo = new AdventureRepositoryMock();
            var stepRepo = new StepRepositoryMock();
            var gameRepo = new GameRepositoryMock();

            var adventureService = new AdventureService(adventureRepo, stepRepo);
            var gameService = new GameService(gameRepo, adventureService);

            // create adventure
            var (adventure, steps) = await CreateAdventureAsync(adventureService);
            var (step1, step22, step31) = (steps[0], steps[1], steps[2]);

            // play game
            var userId = Guid.NewGuid();
            var gameId = await gameService.StartGameAsync(adventure.Id, userId);

            // assert game object is correct
            var game = await gameService.GetGameAsync(gameId);
            Assert.AreEqual(adventure.Id, game.AdventureId, nameof(game.AdventureId));
            Assert.AreEqual(userId, game.UserId, nameof(game.UserId));
            Assert.IsEmpty(game.SelectedOptions, nameof(game.SelectedOptions));

            // start selecting options
            // user will go step1 > step22 > step31

            // select option of the first step
            var step1Option2Result = await gameService.SelectOptionAsync(gameId, step1.Id, step1.Options[1].Id);
            AssertStepsAreEqual(step22, step1Option2Result);

            // select option of the second step
            var step22Option1Result = await gameService.SelectOptionAsync(gameId, step22.Id, step22.Options[0].Id);
            AssertStepsAreEqual(step31, step22Option1Result);

            // the game should over - new step has no options
            Assert.Zero(step22Option1Result.Options.Count);

            // check played game
            var fullGame = await gameService.GetFullGameAsync(gameId);

            // assert adventure and game are correct
            AssertAdventuresAreEqual(adventure, fullGame.adventure);
            AssertGamesAreEqual(game, fullGame.game);

            // assert "decision tree" is correct
            Assert.AreEqual(3, fullGame.gameSteps.Count);

            // first step is step1+option2
            var gameStep1 = fullGame.gameSteps[0];
            AssertStepsAreEqual(step1, gameStep1.Step);
            Assert.AreEqual(step1.Options[1].Id, gameStep1.SelectedOptionId);

            // second step is step22+option1
            var gameStep2 = fullGame.gameSteps[1];
            AssertStepsAreEqual(step22, gameStep2.Step);
            Assert.AreEqual(step22.Options[0].Id, gameStep2.SelectedOptionId);

            // third step is step31, has no options
            var gameStep3 = fullGame.gameSteps[2];
            AssertStepsAreEqual(step31, gameStep3.Step);
            Assert.IsNull(gameStep3.SelectedOptionId);
        }

        private void AssertStepsAreEqual(Step expected, Step actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, nameof(Step.Id));
            Assert.AreEqual(expected.AdventureId, actual.AdventureId, nameof(Step.AdventureId));
            Assert.AreEqual(expected.Text, actual.Text, nameof(Step.Text));
            Assert.AreEqual(expected.Options.Count, actual.Options.Count, "Step Options count");
        }

        private void AssertAdventuresAreEqual(Adventure expected, Adventure actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, nameof(Adventure.Id));
            Assert.AreEqual(expected.Title, actual.Title, nameof(Adventure.Id));
            Assert.AreEqual(expected.FirstStepId, actual.FirstStepId, nameof(Adventure.FirstStepId));
        }

        private void AssertGamesAreEqual(Game expected, Game actual)
        {
            Assert.AreEqual(expected.Id, actual.Id, nameof(Game.Id));
            Assert.AreEqual(expected.AdventureId, actual.AdventureId, nameof(Game.AdventureId));
            Assert.AreEqual(expected.UserId, actual.UserId, nameof(Game.UserId));

            // selected options are not checked
        }

        private async Task<(Adventure, List<Step>)> CreateAdventureAsync(IAdventureService adventureService)
        {
            // create an adventure with the following structure

            //          step1
            //         /     \
            //      step21   step22
            //              /      \
            //          step31     step32

            var adventureId = await adventureService.CreateAdventureAsync("Test Adventure");

            // first (root) step with 2 options
            var step1Id = await adventureService.CreateStepAsync(adventureId, "step1");
            var step1Opt1Id = await adventureService.CreateOptionAsync(step1Id, "step1.option1", null);
            var step1Opt2Id = await adventureService.CreateOptionAsync(step1Id, "step1.option2", null);

            await adventureService.UpdateAdventureAsync(adventureId, null, step1Id);

            // 2nd level of steps

            // step 1 without options, first option of root step points to it
            var step21Id = await adventureService.CreateStepAsync(adventureId, "step21");
            await adventureService.UpdateOptionAsync(step1Id, step1Opt1Id, null, step21Id);

            // step 2 with options, second option of root step points to it
            var step22Id = await adventureService.CreateStepAsync(adventureId, "step22");
            var step22Opt1Id = await adventureService.CreateOptionAsync(step22Id, "step22.option1", null);
            var step22Opt2Id = await adventureService.CreateOptionAsync(step22Id, "step22.option2", null);

            await adventureService.UpdateOptionAsync(step1Id, step1Opt2Id, null, step22Id);

            // 3rd level of steps

            // step 1 without options
            var step31Id = await adventureService.CreateStepAsync(adventureId, "step31");
            await adventureService.UpdateOptionAsync(step22Id, step22Opt1Id, null, step31Id);

            // step 2 without options
            var step32Id = await adventureService.CreateStepAsync(adventureId, "step32");
            await adventureService.UpdateOptionAsync(step22Id, step22Opt2Id, null, step32Id);

            // result objects
            var adventure = await adventureService.GetAdventureAsync(adventureId);
            var steps = await adventureService.ListStepsAsync(adventure.Id);

            // find steps that will be selected by the user
            var step1 = steps.Find(s => s.Id == step1Id);
            var step22 = steps.Find(s => s.Id == step22Id);
            var step31 = steps.Find(s => s.Id == step31Id);
            var gameSteps = new List<Step>(new[] { step1, step22, step31 });

            return (adventure, gameSteps);
        }
    }
}