import { useState, useEffect } from "react";
import axios from "axios";
import "./GameBoard.css";

function GameBoard({ username, onLogout }) {
  const cells = Array.from({ length: 12 }, (_, index) => index);
  const [clickedCells, setClickedCells] = useState([]);
  const [message, setMessage] = useState("Loading game session...");

  const [sessionId, setSessionId] = useState(null);
  const [gameOver, setGameOver] = useState(false);
  const [animalData, setAnimalData] = useState(null);

  // Start the session when the component mounts
  useEffect(() => {
    const startGame = async () => {
      try {
        const response = await axios.post(
          "http://localhost:5231/api/game/start",
          { username },
        );
        setSessionId(response.data.sessionId);
        setMessage("Game Started! Make your guess."); // Good UX
      } catch (error) {
        console.error("Failed to start game:", error);
        setMessage(
          "CRITICAL ERROR: Failed to start session on backend. Check terminal/database.",
        );
      }
    };
    startGame();
  }, [username]);

  const handleCellClick = async (index) => {
    if (gameOver || clickedCells.includes(index)) return;
    if (!sessionId) return; // Wait for the session to load!

    const newClickedCells = [...clickedCells, index];
    setClickedCells(newClickedCells);

    const row = Math.floor(index / 4);
    const col = index % 4;

    try {
      const response = await axios.post(
        "http://localhost:5231/api/game/guess",
        {
          sessionId: sessionId,
          row: row,
          column: col,
        },
      );

      const data = response.data;

      if (data.status === "win") {
        setGameOver(true);
        setAnimalData(data.animal);
        setMessage(`You won! Rank: ${data.rank} | Tries: ${data.tries}`);
      } else if (data.status === "loss") {
        setGameOver(true);
        setAnimalData(data.animal);
        setMessage(
          `You lost! The animal was at ${data.animal.row}, ${data.animal.column}.`,
        );
      } else {
        setMessage(data.message);
      }
    } catch (error) {
      console.error("Axios Error: ", error);
      setMessage("Error on server communication");
    }
  };

  return (
    <div className="game-board-wrapper">
      <div className="game-header">
        <h2>Good Luck, {username}!</h2>
        {!gameOver && <p>Tries: {clickedCells.length} / 3</p>}
        {message && <h3 style={{ color: "#e74c3c" }}>{message}</h3>}
      </div>

      <div className="grid-3x4">
        {cells.map((index) => {
          const row = Math.floor(index / 4);
          const col = index % 4;
          const isAnimalCell =
            animalData && animalData.row === row && animalData.column === col;

          return (
            <button
              key={index}
              className={`grid-cell ${clickedCells.includes(index) ? "clicked" : ""}`}
              onClick={() => handleCellClick(index)}
              disabled={clickedCells.includes(index) || gameOver}
              style={
                isAnimalCell
                  ? {
                      backgroundColor: "white",
                      backgroundImage: `url(${animalData.imageUrl})`,
                      backgroundSize: "cover",
                    }
                  : {}
              }
            >
              {!isAnimalCell && clickedCells.includes(index) ? "X" : ""}
              {!isAnimalCell && !clickedCells.includes(index) ? "?" : ""}
            </button>
          );
        })}
      </div>

      <button className="logout-btn" onClick={onLogout}>
        Reset / Logout
      </button>
    </div>
  );
}

export default GameBoard;
