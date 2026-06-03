import { useState } from "react";
import "./App.css";

function App() {
  const [username, setUsername] = useState("");

  const [inputValue, setInputValue] = useState("");

  const handleStartGame = (e) => {
    e.preventDefault();
    if (inputValue.trim() !== "") {
      setUsername(inputValue.trim());
    }
  };

  return (
    <div className="app-container">
      <h1>Find The Hidden Animal</h1>

      {username === "" ? (
        <div className="login-screen">
          <p>Enter your nickname to start:</p>
          <form onSubmit={handleStartGame}>
            <input
              type="text"
              placeholder="Your name..."
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              required
            />
            <button type="submit">Start</button>
          </form>
        </div>
      ) : (
        <div className="game-screen">
          <h3>Player: {username}</h3>
          <p>Game Table...</p>
          <button onClick={() => setUsername("")}>Leave</button>
        </div>
      )}
    </div>
  );
}

export default App;
