// src/components/LoginForm.tsx
import React, { useState } from "react";
import { login, register } from "../api/authService";

function LoginForm({ onLoginSuccess }: { onLoginSuccess: (token: string) => void }) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const token = await login(email, password);
            onLoginSuccess(token);
        } catch (err: any) {
            if (err.response) {
                setError(err.response.data?.error || "Login failed");
            } else {
                setError("Network error");
            }
        }

    };

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const token = await register(email, password);
            onLoginSuccess(token);
            setError("User registered");
        } catch (err: any) {
            if (err.response) {
                setError(err.response.data[0].description || "Registration failed");
            } else {
                setError("Network error");
            }
        }
    };

    return (
        <form>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <div style={{ marginTop: "10px" }}>
                <button onClick={handleLogin}>Login</button>
                <button onClick={handleRegister} style={{ marginLeft: "10px" }}>
                    Register
                </button>
            </div>
            {error && <p style={{ color: "red" }}>{error}</p>}
        </form>
    );
}

export default LoginForm;