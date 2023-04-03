import http from "k6/http";
import { sleep } from "k6";

export const options = {
    vus: 2,
    duration: "1m",
};

// 🚧 BUILDING
  
export default function () {
    const BASE_URL = "http://localhost:5244"; // make sure this is not production
    const tournamentId = "B84D7572-D177-40AD-843D-1CF8EF662B51";
    const competitionID = "49BE8785-675A-4368-BC3C-5BF6E97C26BD";
    const responses = http.batch([
      ["GET", `${BASE_URL}/tournaments/${tournamentId}/competitions/${competitionID}/leaderboard`],
    ]);
    sleep(1);
}
  