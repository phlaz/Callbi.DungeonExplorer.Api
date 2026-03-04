// src/App.tsx
import { useState } from "react";
import DungeonForm from "./components/DungeonForm";
import DungeonGrid from "./components/DungeonGrid";
import { streamPath, saveObstaclesToApi } from "./api/dungeonService";
import LoginForm from "./components/LoginForm";

function App() {
    const [dungeon, setDungeon] = useState<any>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [path, setPath] = useState<any[]>([]);
    const [token, setToken] = useState<string | null>(
        localStorage.getItem("jwt")
    );

    const handleCreated = (dungeon: any) => {
        const clearedDungeon = { ...dungeon, obstacles: [] };
        setDungeon(clearedDungeon);
        setPath([]);
        setErrorMessage(null);
    };

    const handlePath = async () => {
        if (!dungeon) return;

        setPath([]);
        setErrorMessage(null);

        try {
            for await (const position of streamPath(dungeon.id)) {
                await new Promise((resolve) => setTimeout(resolve, 300));
                setPath((prev) => [...prev, { x: position.x, y: position.y }]);
            }
            console.log("Path complete");
        } catch (err: any) {
            setErrorMessage(err.message);
        }
    };

    const handleLogout = () => {
        localStorage.removeItem("jwt");
        setToken(null);
        setDungeon(null);
        setPath([]);
    };

    return (
        <div className="app-content">
            <h1>Dungeon Explorer</h1>

            {!token ? (
                <LoginForm onLoginSuccess={(jwt) => setToken(jwt)} />
            ) : (
                <>
                    <button onClick={handleLogout} style={{ marginBottom: "10px" }}>
                        Logout
                    </button>

                    <DungeonForm onCreated={handleCreated} />
                    <button onClick={handlePath} disabled={!dungeon}>
                        Compute Path
                    </button>
                    {errorMessage && (
                        <div style={{ color: "red", marginTop: "10px" }}>{errorMessage}</div>
                    )}
                    <DungeonGrid
                        dungeon={dungeon}
                        path={path}
                        onSaveObstacles={async (obstacles) => {
                            console.log("Saving obstacles:", obstacles);
                            await saveObstaclesToApi(dungeon.id, obstacles);
                        }}
                    />
                </>
            )}
        </div>
    );
}

export default App;