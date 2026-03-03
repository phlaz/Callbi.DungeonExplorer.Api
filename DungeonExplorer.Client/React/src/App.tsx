// src/App.tsx
import { useState } from "react";
import DungeonForm from "./components/DungeonForm";
import DungeonGrid from "./components/DungeonGrid";
import { streamPath } from "./api/dungeonService";
import { saveWallsToApi } from "./api/dungeonService";

function App() {
  const [dungeon, setDungeon] = useState<any>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [path, setPath] = useState<any[]>([]);

  const handleCreated = (d: any) => setDungeon(d);

    const handlePath = async () => {
        if (!dungeon) return;

        setPath([]);
        setErrorMessage(null); // clear any previous error

        try {
            for await (const position of streamPath(dungeon.id)) {
                await new Promise(resolve => setTimeout(resolve, 300)); // delay for animation
                setPath(prev => [...prev, { x: position.x, y: position.y }]);
            }
            console.log("Path complete");
        } catch (err: any) {
            setErrorMessage(err.message); // store error message in state
        }
    };

    


  return (
    <div>
        <DungeonForm onCreated={handleCreated} />
        <button onClick={handlePath} disabled={!dungeon}>Compute Path</button>
        {
            errorMessage && (
                <div style={{ color: "red", marginTop: "10px" }}>
                    {errorMessage}
                </div>
            )
        }
        <DungeonGrid dungeon={dungeon} path={path} onSaveWalls={async (walls) => {
            console.log("Saving walls:", walls);
            await saveWallsToApi(dungeon.id, walls);
        }}
/>
    </div>
  );
}

export default App;