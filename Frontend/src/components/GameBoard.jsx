// import { useState } from "react";
// import axios from "axios";
// import "./GameBoard.css";
//
// function GameBoard({ username, onLogout }) {
//   const cells = Array.from({ length: 12 }, (_, index) => index);
//   const [clickedCells, setClickedCells] = useState([]);
//
//   const [message, setMessage] = useState("");
//
//   const handleCellClick = async (index) => {
//     if (!clickedCells.includes(index) && clickedCells.length < 3) {
//       const newClickedCells = [...clickedCells, index];
//       setClickedCells(newClickedCells);
//
//       const row = Math.floor(index / 4);
//       const col = index % 4;
//
//       try {
//         const response = await axios.post("http://localhost:5231/api/game/guess", {
//           username: username,
//           row: row,
//           column: col,
//           attemptNumber: newClickedCells.length,
//         });
//
//         setMessage(response.data.message);
//       } catch (error) {
//         console.error("Axios Error: ", error);
//         setMessage("Error on server communication");
//       }
//     }
//   };
//
//   return (
//     <div className="game-board-wrapper">
//       <div className="game-header">
//         <h2>Good Luck, {username}!</h2>
//         <p>Tries: {clickedCells.length} / 3</p>
//         {message && <h3 style={{ color: "#e74c3c" }}>{message}</h3>}
//       </div>
//
//       <div className="grid-3x4">
//         {cells.map((index) => (
//           <button
//             key={index}
//             className={`grid-cell ${clickedCells.includes(index) ? "clicked" : ""}`}
//             onClick={() => handleCellClick(index)}
//             disabled={clickedCells.includes(index) || clickedCells.length >= 3}
//           >
//             {clickedCells.includes(index) ? "X" : "?"}
//           </button>
//         ))}
//       </div>
//
//       <button className="logout-btn" onClick={onLogout}>
//         Logout
//       </button>
//     </div>
//   );
// }
//
// export default GameBoard;
import { useState } from "react";
import axios from "axios";
import "./GameBoard.css";

function GameBoard({ username, onLogout }) {
  const cells = Array.from({ length: 12 }, (_, index) => index);
  const [clickedCells, setClickedCells] = useState([]);

  const [message, setMessage] = useState("");

  const handleCellClick = async (index) => {
    if (!clickedCells.includes(index) && clickedCells.length < 3) {
      const newClickedCells = [...clickedCells, index];
      setClickedCells(newClickedCells);

      const row = Math.floor(index / 4);
      const col = index % 4;

      try {
        // AICI ESTE MODIFICAREA: am adăugat http://
        const response = await axios.post(
          "http://localhost:5231/api/game/guess",
          {
            username: username,
            row: row,
            column: col,
            attemptNumber: newClickedCells.length,
          },
        );

        setMessage(response.data.message);
      } catch (error) {
        console.error("Axios Error: ", error);
        setMessage("Error on server communication");
      }
    }
  };

  return (
    <div className="game-board-wrapper">
      <div className="game-header">
        <h2>Good Luck, {username}!</h2>
        <p>Tries: {clickedCells.length} / 3</p>
        {message && <h3 style={{ color: "#e74c3c" }}>{message}</h3>}
      </div>

      <div className="grid-3x4">
        {cells.map((index) => (
          <button
            key={index}
            className={`grid-cell ${clickedCells.includes(index) ? "clicked" : ""}`}
            onClick={() => handleCellClick(index)}
            disabled={clickedCells.includes(index) || clickedCells.length >= 3}
          >
            {clickedCells.includes(index) ? "X" : "?"}
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
