using System.Threading.Tasks;
using Demo.Adventures.Api.Contracts.Adventures;
using Demo.Adventures.Api.Contracts.Games;
using NUnit.Framework;

namespace Demo.Adventures.Tests.E2E
{
    public class GameApiTests : BaseTestFixture
    {
        [Test]
        public async Task PlayedGame_IsValid()
        {
            var client = GetClient();
            var advClient = client.Adventures;
            var gameClient = client.Games;

            // create the simplest possible adventure
            //      step1
            //        |
            //      step2

            var adventureId = await advClient.CreateAdventureAsync("Adventure");
            var step1Id = await advClient.CreateStepAsync(adventureId, new CreateStepRequest { Text = "Step1.Start" });
            var step2Id = await advClient.CreateStepAsync(adventureId, new CreateStepRequest { Text = "Step2.End" });
            var optionId = await advClient.CreateOptionAsync(adventureId, step1Id,
                new CreateOptionRequest { Text = "Option", NextStepId = step2Id });
            await advClient.UpdateAdventureAsync(adventureId, new UpdateAdventureRequest { FirstStepId = step1Id });

            // play game

            // start
            var gameId = await gameClient.StartGameAsync(new StartGameRequest { AdventureId = adventureId });
            var gameResp = await gameClient.GetGameAsync(gameId);

            Assert.AreEqual(gameId, gameResp.GameId);
            Assert.AreEqual(adventureId, gameResp.Adventure.Id);
            Assert.IsEmpty(gameResp.Steps);

            // select option
            var selectOptionResp = await gameClient.SelectOptionAsync(gameId,
                new SelectOptionRequest { StepId = step1Id, OptionId = optionId });

            Assert.AreEqual(step2Id, selectOptionResp.NextStep.Id);

            // final decision tree
            gameResp = await gameClient.GetGameAsync(gameId);

            Assert.AreEqual(2, gameResp.Steps.Count);
            Assert.AreEqual(optionId, gameResp.Steps[0].SelectedOptionId);
            Assert.IsNull(gameResp.Steps[1].SelectedOptionId);
        }
    }
}