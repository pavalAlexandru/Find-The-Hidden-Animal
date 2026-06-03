import { useState } from "react";
import Login from "./components/Login";
import GameBoard from "./components/GameBoard";
import LeaderBoard from "./components/LeaderBoard";
import AddConfig from "./components/AddConfig";
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

      <hr style={{ margin: "30px 0", border: "1px solid #eee" }} />
      <LeaderBoard />

      <hr style={{ margin: "30px 0", border: "1px solid #eee" }} />
      <AddConfig />
    </div>
  );
}

export default App;
