import { useState } from "react";
import "./GameBoard.css";

function GameBoard({ username, onLogout }) {
  const cells = Array.from({ length: 12 }, (_, index) => index);
  const [clickedCells, setClickedCells] = useState([]);

  const handleCellClick = (index) => {
    if (!clickedCells.includes(index) && clickedCells.length < 3) {
      setClickedCells([...clickedCells, index]);
    }
  };

  return (
    <div className="game-board-wrapper">
      <div className="game-header">
        <h2>Good Luck, {username}!</h2>
        <p>Tries: {clickedCells.length} / 3</p>
      </div>

      <div className="grid-3x4">
        {cells.map((index) => (
          <button
            key={index}
            className={`grid-cell ${clickedCells.includes(index) ? "clicked" : ""}`}
            onClick={() => handleCellClick(index)}
            disabled={clickedCells.includes(index) || clickedCells.length >= 3}
          >
            {clickedCells.includes(index) ? "WRONG" : "?"}
          </button>
        ))}
      </div>

      <button className="logout-btn" onClick={onLogout}>
        Logout
      </button>
    </div>
  );
}

export default GameBoard;
