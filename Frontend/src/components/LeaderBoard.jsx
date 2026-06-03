import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function LeaderBoard() {
  const [leaderboardData, setLeaderboardData] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5231/leaderboardHub")
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("Connected to SignalR!"))
      .catch((err) => console.error("Error SignalR: ", err));
    connection.on("UpdateLeaderboar", (newData) => {
      console.log("Leaderboard updated: ", newData);
      setLeaderboardData(newData);
    });

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div className="leaderboard-wrapper">
      <h3>Live Leaderboard</h3>

      {leaderboardData.length === 0 ? (
        <p className="no-data">There are no finished games</p>
      ) : (
        <table className="leaderboard-table">
          <thead>
            <tr>
              <th>Player</th>
              <th>Date and Time</th>
              <th>Tries</th>
              <th>Animal</th>
            </tr>
          </thead>
          <tbody>
            {leaderboardData.map((game, index) => (
              <tr key={index}>
                <td>{game.playerName}</td>
                <td>{new Date(game.timestamp).toLocaleString()}</td>
                <td>{game.attempts}</td>
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
