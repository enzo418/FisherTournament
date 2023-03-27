using FisherTournament.Application.Competitions.Commands.AddScore;
using FisherTournament.Application.Tournaments.Commands.AddInscription;
using FisherTournament.Domain.CompetitionAggregate;
using FisherTournament.Domain.CompetitionAggregate.Entities;

namespace FisherTournament.IntegrationTests.Competitions.Commands
{
    public class AddScoreHandlerTest : BaseUseCaseTest
    {
        public AddScoreHandlerTest(UseCaseTestsFixture fixture) : base(fixture)
        {
        }

        // [Fact]
        // public async Task Handler_Should_AddScore()
        // {
        //     // 
        //     var tournament = await _fixture.AddAsync(Tournament.Create(
        //         "Test Tournament",
        //         _fixture.DateTimeProvider.Now.AddDays(1),
        //         _fixture.DateTimeProvider.Now.AddDays(5)));

        //     var competition = await _fixture.AddAsync(Competition.Create(
        //         _fixture.DateTimeProvider.Now.AddDays(1),
        //         tournament.Id,
        //         Location.Create("Test City", "Test State", "Test Country", "Test Place")));

        //     var user = await _fixture.AddAsync(User.Create("First", "Last"));
        //     var fisher = await _fixture.AddAsync(Fisher.Create(user.Id));

        //     var addInscriptionCommand = new AddInscriptionCommand(tournament.Id.ToString(), fisher.Id.ToString());
        //     var addScoreCommand = new AddScoreCommand(fisher.Id.ToString(), competition.Id.ToString(), 10);

        //     // 
        //     var inscriptionResult = await _fixture.SendAsync(addInscriptionCommand);
        //     var result = await _fixture.SendAsync(addScoreCommand);
        //     var competitionWithScore = await _fixture.FindAsync<Competition>(competition.Id);

        //     // 
        //     inscriptionResult.IsError.Should().BeFalse();
        //     result.IsError.Should().BeFalse();
        //     competitionWithScore.Should().NotBeNull();
        //     competitionWithScore!.Participations.Should().HaveCount(1);
        //     competitionWithScore.Participations.First().TotalScore.Should().Be(10);
        // }


        [Fact]
        public async Task Handler_Should_NotAddScore_When_FisherDoesntExist()
        {
            // 
            using var context = _fixture.Context;
            var tournament = await context.WithAsync(Tournament.Create(
                "Test Tournament",
                _fixture.DateTimeProvider.Now.AddDays(1),
                _fixture.DateTimeProvider.Now.AddDays(5)));

            var competition = await context.WithAsync(Competition.Create(
                _fixture.DateTimeProvider.Now.AddDays(1),
                tournament.Id,
                Location.Create("Test City", "Test State", "Test Country", "Test Place")));

            var fisher = Fisher.Create(Guid.NewGuid());

            var command = new AddScoreCommand(fisher.Id.ToString(), competition.Id.ToString(), 10);

            // 
            var result = await _fixture.SendAsync(command);

            // 
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Fishers.NotFound);
        }

        [Fact]
        public async Task Handler_Should_NotAddScore_When_CompetitionDoesntExist()
        {
            // 
            using var context = _fixture.Context;
            var fisher = await context.WithFisherAsync("First", "Last");
            var competition = Competition.Create(
                _fixture.DateTimeProvider.Now.AddDays(1),
                Guid.NewGuid(),
                Location.Create("Test City", "Test State", "Test Country", "Test Place"));

            var command = new AddScoreCommand(fisher.Id.ToString(), competition.Id.ToString(), 10);

            // 
            var result = await _fixture.SendAsync(command);

            // 
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Competitions.NotFound);
        }

        [Fact]
        public async Task Handler_Should_NotAddScore_When_FisherIsNotEnrolled()
        {
            // 
            using var context = _fixture.Context;
            var tournament = await context.WithAsync(Tournament.Create(
                "Test Tournament",
                _fixture.DateTimeProvider.Now.AddDays(1),
                _fixture.DateTimeProvider.Now.AddDays(5)));

            var competition = await context.WithAsync(Competition.Create(
                _fixture.DateTimeProvider.Now.AddDays(1),
                tournament.Id,
                Location.Create("Test City", "Test State", "Test Country", "Test Place")));

            var fisher = await context.WithFisherAsync("First", "Last");

            var command = new AddScoreCommand(fisher.Id.ToString(), competition.Id.ToString(), 10);

            // 
            var result = await _fixture.SendAsync(command);

            // 
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Tournaments.NotEnrolled);
        }
    }
}