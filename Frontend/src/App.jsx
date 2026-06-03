import { useState } from "react";
import Login from "./components/Login";
import GameBoard from "./components/GameBoard";
import "./App.css";

function App() {
  const [username, setUsername] = useState("");

  return (
    <div className="app-container">
      {username === "" ? (
        <Login onLogin={setUsername} />
      ) : (
        <GameBoard username={username} onLogout={() => setUsername("")} />
      )}
    </div>
  );
}

export default App;
