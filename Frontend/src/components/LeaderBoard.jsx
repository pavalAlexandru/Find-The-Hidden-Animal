import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function Leaderboard() {
  const [leaderboardData, setLeaderboardData] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5231/leaderboardHub")
      .withAutomaticReconnect().build;

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

  return <div>...</div>;
}
