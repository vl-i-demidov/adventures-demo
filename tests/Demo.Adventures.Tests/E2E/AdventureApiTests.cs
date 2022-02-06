using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Adventures.Api.Contracts.Adventures;
using NUnit.Framework;

namespace Demo.Adventures.Tests.E2E
{
    // negative tests not included
    public class AdventureApiTests : BaseTestFixture
    {
        [Test]
        public async Task CreateUpdateAdventure_IsCorrect()
        {
            var client = GetClient().Adventures;

            // create adventure
            var title = "Adventure";
            var adventureId = await client.CreateAdventureAsync("Adventure");
            var adventure = await client.GetAdventureAsync(adventureId);
            Assert.AreEqual(title, adventure.Title);

            // update title
            title = "Adventure 2.0";
            await client.UpdateAdventureAsync(adventureId, new UpdateAdventureRequest { Title = title });
            adventure = await client.GetAdventureAsync(adventureId);
            Assert.AreEqual(title, adventure.Title);
        }

        [Test]
        public async Task CreateUpdateDeleteStep_IsCorrect()
        {
            var client = GetClient().Adventures;

            // create adventure
            var adventureId = await client.CreateAdventureAsync("Adventure");

            // create step
            var stepText = "Step";
            var stepId = await client.CreateStepAsync(adventureId, new CreateStepRequest { Text = stepText });
            var step = await client.GetStepAsync(adventureId, stepId);
            Assert.AreEqual(stepText, step.Text);

            // update step text
            stepText = "Step 2.0";
            await client.UpdateStepAsync(adventureId, stepId, new UpdateStepRequest { Text = stepText });
            step = await client.GetStepAsync(adventureId, stepId);
            Assert.AreEqual(stepText, step.Text);

            // delete step
            await client.DeleteStepAsync(adventureId, stepId);

            var exception = Assert.ThrowsAsync<HttpRequestException>(() => client.GetStepAsync(adventureId, stepId));
            AssertStatusCode(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task CreateUpdateDeleteOption_IsCorrect()
        {
            var client = GetClient().Adventures;

            // create adventure
            var adventureId = await client.CreateAdventureAsync("Adventure");

            // create steps
            var stepId = await client.CreateStepAsync(adventureId, new CreateStepRequest { Text = "Step" });
            var step2Id = await client.CreateStepAsync(adventureId, new CreateStepRequest { Text = "Step2" });

            // create option
            var optionText = "Option";
            await client.CreateOptionAsync(adventureId, stepId, new CreateOptionRequest { Text = optionText });
            var step = await client.GetStepAsync(adventureId, stepId);

            Assert.AreEqual(1, step.Options.Count);

            var option = step.Options[0];
            Assert.AreEqual(optionText, option.Text);
            Assert.AreEqual(Guid.Empty, option.NextStepId);

            // update option text and nextStepId
            optionText = "Option 2.0";
            await client.UpdateOptionAsync(adventureId, stepId, step.Options[0].Id,
                new UpdateOptionRequest { Text = optionText, NextStepId = step2Id });

            step = await client.GetStepAsync(adventureId, stepId);
            Assert.AreEqual(1, step.Options.Count);

            option = step.Options[0];
            Assert.AreEqual(optionText, option.Text);
            Assert.AreEqual(step2Id, option.NextStepId);

            // delete option
            await client.DeleteOptionAsync(adventureId, stepId, option.Id);
            step = await client.GetStepAsync(adventureId, stepId);
            Assert.IsEmpty(step.Options);
        }
    }
}