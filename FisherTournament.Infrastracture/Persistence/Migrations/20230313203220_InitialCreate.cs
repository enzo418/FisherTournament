﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FisherTournament.Infrastracture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Location_City = table.Column<string>(type: "TEXT", nullable: false),
                    Location_State = table.Column<string>(type: "TEXT", nullable: false),
                    Location_Country = table.Column<string>(type: "TEXT", nullable: false),
                    Location_Place = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentCompetitionsIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentCompetitionsIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentCompetitionsIds_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fishers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fishers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionParticipations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FisherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalScore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionParticipations", x => new { x.Id, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_CompetitionParticipations_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipations_Fishers_FisherId",
                        column: x => x.FisherId,
                        principalTable: "Fishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentInscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FisherId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentInscription", x => new { x.Id, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_TournamentInscription_Fishers_FisherId",
                        column: x => x.FisherId,
                        principalTable: "Fishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentInscription_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionParticipationFishCaught",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetitionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitionParticipationId = table.Column<int>(type: "INTEGER", nullable: false),
                    FisherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionParticipationFishCaught", x => new { x.Id, x.CompetitionParticipationId, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_CompetitionParticipationFishCaught_CompetitionParticipations_CompetitionParticipationId_CompetitionId",
                        columns: x => new { x.CompetitionParticipationId, x.CompetitionId },
                        principalTable: "CompetitionParticipations",
                        principalColumns: new[] { "Id", "CompetitionId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionParticipationFishCaught_Fishers_FisherId",
                        column: x => x.FisherId,
                        principalTable: "Fishers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipationFishCaught_CompetitionParticipationId_CompetitionId",
                table: "CompetitionParticipationFishCaught",
                columns: new[] { "CompetitionParticipationId", "CompetitionId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipationFishCaught_FisherId",
                table: "CompetitionParticipationFishCaught",
                column: "FisherId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipations_CompetitionId",
                table: "CompetitionParticipations",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionParticipations_FisherId",
                table: "CompetitionParticipations",
                column: "FisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_TournamentId",
                table: "Competitions",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Fishers_UserId",
                table: "Fishers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentCompetitionsIds_TournamentId",
                table: "TournamentCompetitionsIds",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInscription_FisherId",
                table: "TournamentInscription",
                column: "FisherId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInscription_TournamentId",
                table: "TournamentInscription",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionParticipationFishCaught");

            migrationBuilder.DropTable(
                name: "TournamentCompetitionsIds");

            migrationBuilder.DropTable(
                name: "TournamentInscription");

            migrationBuilder.DropTable(
                name: "CompetitionParticipations");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Fishers");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
