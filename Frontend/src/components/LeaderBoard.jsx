import { useEffect, useState } from "react";
import axios from "axios";
import * as signalR from "@microsoft/signalr";
import "./LeaderBoard.css";

function LeaderBoard() {
  const [leaderboardData, setLeaderboardData] = useState([]);

  useEffect(() => {
    // Fetch initial leaderboard
    axios
      .get("http://localhost:5231/api/game/leaderboard")
      .then((res) => setLeaderboardData(res.data))
      .catch((err) => console.error("Error fetching initial leaderboard", err));

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5231/leaderboardHub")
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("Connected to SignalR!"))
      .catch((err) => console.error("Error SignalR: ", err));

    // Fixed typo: UpdateLeaderboar -> UpdateLeaderboard
    connection.on("UpdateLeaderboard", (newData) => {
      console.log("Leaderboard updated via SignalR: ", newData);
      setLeaderboardData(newData);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div className="leaderboard-wrapper">
      <h3>Live Leaderboard (Ranked by Duration)</h3>

      {leaderboardData.length === 0 ? (
        <p className="no-data">There are no finished games yet.</p>
      ) : (
        <table className="leaderboard-table">
          <thead>
            <tr>
              <th>Rank</th>
              <th>Player</th>
              <th>Date and Time</th>
              <th>Tries</th>
              <th>Duration (s)</th>
              <th>Animal</th>
            </tr>
          </thead>
          <tbody>
            {leaderboardData.map((game, index) => (
              <tr key={index}>
                <td>#{index + 1}</td>
                <td>{game.playerName}</td>
                <td>{new Date(game.timestamp).toLocaleString()}</td>
                <td>{game.attempts}</td>
                <td>{game.durationInSeconds}</td>
                <td>{game.animalName}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default LeaderBoard;
