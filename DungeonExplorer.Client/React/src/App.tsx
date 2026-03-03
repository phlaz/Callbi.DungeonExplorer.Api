// src/App.tsx
import { useState } from "react";
import DungeonForm from "./components/DungeonForm";
import DungeonGrid from "./components/DungeonGrid";
import { streamPath } from "./api/dungeonService";
import { saveWallsToApi } from "./api/dungeonService";

function App() {
  const [dungeon, setDungeon] = useState<any>(null);
  const [path, setPath] = useState<any[]>([]);

  const handleCreated = (d: any) => setDungeon(d);

    const handlePath = async () => {
        if (!dungeon) return;

        setPath([]); // reset path before streaming

        for await (const position of streamPath(dungeon.id)) {
            // Add a delay before plotting each step
            await new Promise(resolve => setTimeout(resolve, 250)); // 300ms delay

            console.log("Got position:", position);
            setPath(prev => [...prev, position]); // update incrementally
        }

        console.log("Path complete");
    };


  return (
    <div>
      <DungeonForm onCreated={handleCreated} />
      <button onClick={handlePath} disabled={!dungeon}>Compute Path</button>
          <DungeonGrid dungeon={dungeon} path={path} onSaveWalls={async (walls) => {
              console.log("Saving walls:", walls);
              await saveWallsToApi(dungeon.id, walls);
          }}
/>
    </div>
  );
}

export default App;