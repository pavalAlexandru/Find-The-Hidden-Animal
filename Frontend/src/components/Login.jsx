import { useState } from "react";
import "./Login.css";

function Login({ onLogin }) {
  const [inputValue, setInputValue] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    if (inputValue.trim() !== "") {
      onLogin(inputValue.trim());
    }
  };

  return (
    <div className="login-wrapper">
      <h2>Find The Hidden Animal</h2>
      <p>Enter your nickname to start:</p>
      <form onSubmit={handleSubmit} className="login-form">
        <input
          type="text"
          placeholder="Your name..."
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          required
        />
        <button type="submit">START</button>
      </form>
    </div>
  );
}

export default Login;
