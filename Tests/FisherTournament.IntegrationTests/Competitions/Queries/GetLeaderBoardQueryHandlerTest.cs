using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using FisherTournament.Application.Competitions.Queries.GetLeaderBoard;
using FisherTournament.Domain.CompetitionAggregate.Entities;

namespace FisherTournament.IntegrationTests.Competitions.Queries
{
    public class GetLeaderBoardQueryHandlerTest : BaseUseCaseTest
    {
        public GetLeaderBoardQueryHandlerTest(UseCaseTestsFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Handler_Should_ReturnLeaderBoard()
        {
            // Arrange
            using var context = _fixture.Context;
            var fisher1 = context.PrepareFisher("First1", "Last1");
            var fisher2 = context.PrepareFisher("First2", "Last2");
            var fisher3 = context.PrepareFisher("First3", "Last3");
            var fisher4 = context.PrepareFisher("First4", "Last4");

            var tournament = context.PrepareAdd(Tournament.Create(
                "Test Tournament",
                _fixture.DateTimeProvider.Now,
                _fixture.DateTimeProvider.Now.AddDays(2)));

            var categoryPrimary = tournament.AddCategory("Primary").Value;
            var categorySecondary = tournament.AddCategory("Secondary").Value;

            await context.SaveChangesAsync();

            var inscriptionResults = new List<ErrorOr<Success>>()
            {
                tournament.AddInscription(fisher1.Id, categoryPrimary.Id, _fixture.DateTimeProvider),
                tournament.AddInscription(fisher2.Id, categoryPrimary.Id, _fixture.DateTimeProvider),
                tournament.AddInscription(fisher3.Id, categorySecondary.Id, _fixture.DateTimeProvider),
                tournament.AddInscription(fisher4.Id, categorySecondary.Id, _fixture.DateTimeProvider)
            };

            inscriptionResults.Should().NotContain(r => r.IsError);

            await context.SaveChangesAsync();

            var competition = await context.WithAsync(Competition.Create(
                _fixture.DateTimeProvider.Now,
                tournament.Id,
                Location.Create("Test City", "Test State", "Test Country", "Test Place")),
                beforeSave: comp =>
                {
                    var results = new List<ErrorOr<Success>>()
                    {
                        comp.AddScore(fisher1.Id, 1, _fixture.DateTimeProvider),
                        comp.AddScore(fisher1.Id, 10, _fixture.DateTimeProvider),
                        comp.AddScore(fisher2.Id, 200, _fixture.DateTimeProvider),
                        comp.AddScore(fisher3.Id, 100, _fixture.DateTimeProvider),
                        comp.AddScore(fisher4.Id, 5, _fixture.DateTimeProvider)
                    };

                    results.Should().NotContain(r => r.IsError);
                });

            // Act
            var result = await _fixture.SendAsync(new GetLeaderBoardQuery(competition.Id.ToString()));

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value!.Should().HaveCount(2);

            var primaryCategory = result.Value.FirstOrDefault(c => c.Id == categoryPrimary.Id && c.Name == categoryPrimary.Name);
            primaryCategory.Should().NotBeNull();

            primaryCategory!.LeaderBoard.Should().HaveCount(2);

            primaryCategory.LeaderBoard.First().FisherId.Should().Be(fisher2.Id);
            primaryCategory.LeaderBoard.First().TotalScore.Should().Be(200);

            primaryCategory.LeaderBoard.Last().FisherId.Should().Be(fisher1.Id);
            primaryCategory.LeaderBoard.Last().TotalScore.Should().Be(11);

            var secondaryCategory = result.Value.FirstOrDefault(c => c.Id == categorySecondary.Id && c.Name == categorySecondary.Name);
            secondaryCategory.Should().NotBeNull();
            secondaryCategory.LeaderBoard.Should().HaveCount(2);
            secondaryCategory!.LeaderBoard.First().FisherId.Should().Be(fisher3.Id);
            secondaryCategory.LeaderBoard.First().TotalScore.Should().Be(100);
        }

        [Fact]
        public async Task Handler_Should_ReturnEmptyLeaderBoard()
        {
            // Arrange
            // Act
            var result = await _fixture.SendAsync(new GetLeaderBoardQuery(Guid.NewGuid().ToString()));

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value!.Should().BeEmpty();
        }
    }
}