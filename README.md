# Adventures Demo

![ci workflow](https://github.com/vl-i-demidov/adventures-demo/actions/workflows/ci.yaml/badge.svg)

This web application allows a player to choose their own adventure by picking from multiple choices in order to progress to the next set of choices, until they get to one of the endings.

## Design
The structure of an adventure is represented as a tree, where each node is a `Step` and every edge is an `Option`. `Step` can have any number of `Option`s. `Option`s can link only to one ("next") `Step`. If `Step` doesn't have any `Option`s, it is considered as a final adventure step.
When a user starts to play, they create a `Game`. `Game` is linked to the adventure and consists of an array of `Option`s that were chosen during the game.

## Models
There is a few main objects that describe adventure.
`Adventure` - contains description and pointer to the first (root) `Step`.
```json
{
    "Id":"1c2bf383-657a-455b-b303-546b45ffb39e",
    "Title":"My Adventure",
    "FirstStepId": "c8c5f880-1334-4bd2-918f-90fd0bb4ef63"
}
```
`Step` - a question with `Option`s or a final adventure result.
``` json
{
    "Id": "c8c5f880-1334-4bd2-918f-90fd0bb4ef63",
    "AdventureId": "1c2bf383-657a-455b-b303-546b45ffb39e",
    "Text": "To be or not to be?",
    "Options": [{
            "Id": "8d971152-9304-49e5-bdb7-2cf88ec608ec",
            "Text": "To be",
            "NextStepId": "7b9e3682-e18c-4d0b-ae54-fca967cf029c"
        }, {
            "Id": "eecd42b3-776b-4b05-8acd-b0101e730e20",
            "Text": "Not to be",
            "NextStepId": "b8ff48d0-2ecf-465c-9761-3b58eeaeb202"
        }, {
            "Id": "d1b98624-4ab2-4b16-8418-cfeee59faab4",
            "Text": "Who knows...",
            "NextStepId": "b8ff48d0-2ecf-465c-9761-3b58eeaeb202"
        }
    ]
}
```
`Game` describes a game that a user is playing or played.
```json
{
    "Id": "eed8f6d7-3bbb-4d49-832d-551bd814f271",
    "AdventureId": "1c2bf383-657a-455b-b303-546b45ffb39e",
    "UserId": "656dfe24-b317-4ac0-9216-481ab3fb43e3",
    "SelectedOptions": [{
            "StepId": "c8c5f880-1334-4bd2-918f-90fd0bb4ef63",
            "OptionId": "8d971152-9304-49e5-bdb7-2cf88ec608e"
        }, {
            "StepId": "7b9e3682-e18c-4d0b-ae54-fca967cf029c",
            "OptionId": "677a33c8-b781-4bf0-b2f6-cd7a81fa8a55"
        }
    ]
}
```
These 3 models are stored as is in the database.

## REST API

REST API provides basic CRUD operations for `Adventure`s, `Step`s and `Option`s. These endpoints can be used on a "Create Adventure" page of the frontend application.

![Adventures Api](https://db3pap004files.storage.live.com/y4m-TPNJWXt5HuWw7aMo9L6cvd5GqoWjhlIfGEkCtOF2uaKvdqOR95Rg0u7M5FtWxt-O9YDWIjiFHzafDzqPGNkPAS2ikBLbfOouXDDrpmQjiVTxwKpvJ55PiOGwHQyuHv2QoLlD1j08Dq40nuywZ_9aEdI-InwZZWFkqLZx7Iw_shAW1B06vtesRatAKFXYytrOCI_vlkeaFYwoTFZVVP8oA/adventures-demo.png?psid=1&width=813&height=213)
  
![Steps Api](https://db3pap004files.storage.live.com/y4mLjynUjYLfoiKTU9fT_fvxemQMECv1-EKoDsV-mt_Tj9k2uxZeQhCGLTUsVFePrZGrjs0NQi330nk4K0PuY-wTb09zqkj_j_5czx6LNAMljkP6Ko4x6Ala6QbjzHs-vtCkm6P9eWsva_gHU-f7gDDHTy_bcEisrdG0sU-iWxqHEk7Bl50dQadWFhjJjq1eJe0c64-IxUUKdRTyqQoRao45A/steps-api.png?psid=1&width=818&height=322)

![Options Api](https://db3pap004files.storage.live.com/y4mMYqrxEOWDo-VypCA-voeSb4knRBNkqcAT0JwH-BouOKsk0CwtittdTOIQCyXUAF-bTX2sruIYQwV4pvCJtXbSuniO3CZrvVlOApxzSvIvRRZtXzS5xgYWqOfyd-5buukFRzzw7TydgUZTFcPr-q7lvgdO6RmqMgv6wtfCL0_Narx8ng7DWFqnjPw0OP6H1p5bzfbpt6KFw2G0EIRYqb6PA/options-api.png?psid=1&width=815&height=236)


"Take an Adventure" and "Decision tree" pages would also use API to manage `Game`s. In particular, `GET /games/{gameId}` returns all steps that the user made during the game including selected options.


![Game Api](https://db3pap004files.storage.live.com/y4mLPTBjyBnx2Re_WXLLPqE4L11E9-0HurmFSPeKhpnZ_myjtfCmA_0vCb8KjZ8xMcQsgfNf1awBhG4Be-vDpkWtByBb7m1g1sfoxNCgKTHBxKV44PQBy5ibTCylXPZI48Win0Vf5i4D_gue78QeWgVFKWkQsoeO8Pe1c4xuRdtufiniRNUcg-oAuKcwZhg4ltZ929BiyxhbnwfmNlffKTsGA/games-api.png?psid=1&width=768&height=203)
  
## Solution Structure

- **Demo.Adventures.Domain** - domain entities.
- **Demo.Adventures.Logic** - business logic.
- **Demo.Adventures.Database** - repository contracts and their MongoDB implementation.
- **Demo.Adventures.Common** - classes that are used by multiple projects (currently only exceptions).
- **Demo.Adventures.Api** - HTTP server.
- **Demo.Adventures.Api.Client** - a simple HTTP client used mainly for testing.
- **Demo.Adventures.Api.Contracts** - HTTP contracts.

## Tests
Only several basic tests exist that cover main functionality.
Since business logic is very straightforward, unit tests cover only the main use case - playing a game and retrieving results.
End2End tests check main CRUD operations of the API and also simulate a simple game.

## Database
MongoDB was chosen as a database because this NoSQL document db meets the non-strict requirements (no need for ACID transactions or complex joins) and easy to use (no migrations, schemas, etc).

## How to run
To run solution use `/docker-compose/start.ps1` script. To stop solution use corresponding `stop.ps1` script.

## Continuous Integration
A simple ci-pipeiline runs on each push to master branch.

## TODO
#### General
 - Check adventure validity.
 - Allow to delete adventure.
 - Add userId to adventure.
 - Add some data consistency checks.
 - Use cancellation tokens.
 - Add logging.
 - Negative tests.

#### Testing
A more scalable way to test the solution would be to implement
 - Unit tests - check business logic in-memory (already implemented).
 - Integration/Component tests - check actual database via repository or business logic classes.
 - E2E in-memory tests - use [ASP.NET Core TestServer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-6.0) and mocked repositories to run E2E tests in-memory.