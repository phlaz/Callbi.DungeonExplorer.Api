// src/App.tsx
import { useState } from "react";
import DungeonForm from "./components/DungeonForm";
import DungeonGrid from "./components/DungeonGrid";
import { getPath } from "./api/dungeonService";
import { saveWallsToApi } from "./api/dungeonService";

function App() {
  const [dungeon, setDungeon] = useState<any>(null);
  const [path, setPath] = useState<any[]>([]);

  const handleCreated = (d: any) => setDungeon(d);

  const handlePath = async () => {
    if (!dungeon) return;
    const result = await getPath(dungeon.id);
    
    console.log("Path result:", result);
    setPath(result.path);
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