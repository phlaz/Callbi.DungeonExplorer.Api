import { useState } from "react";
import { createDungeon } from "../api/dungeonService";

export default function DungeonForm({ onCreated }: { onCreated: (dungeon: any) => void }) {
  const [width, setWidth] = useState(10);
  const [height, setHeight] = useState(10);
  const [startX, setStartX] = useState(0);
  const [startY, setStartY] = useState(0);
  const [goalX, setGoalX] = useState(9);
  const [goalY, setGoalY] = useState(9);

  const handleSubmit = async (e: any) => {
    e.preventDefault();
    const dungeon = {
      width,
      height,
      start: { x: startX, y: startY },
      goal: { x: goalX, y: goalY },
        walls: [
            { x: 1, y: 1 }, { x: 2, y: 1 }, { x: 3, y: 1 },
            { x: 4, y: 1 }, { x: 4, y: 1 }, { x: 4, y: 1 },
            { x: 6, y: 1 }, { x: 6, y: 2 }, { x: 6, y: 3 }, { x: 6, y: 4 },
            { x: 0, y: 6 }, { x: 1, y: 6 }, { x: 2, y: 6 },
            { x: 0, y: 8 }, { x: 1, y: 8 }, { x: 2, y: 8 },
        ] // hardcoded for demo
    };
    const result = await createDungeon(dungeon);
    onCreated(result);
  };

  return (
      <form onSubmit={handleSubmit} action="javascript:void(0)">
      <h3>Create Dungeon</h3>
      <label>Width:   <input type="number" value={width}  onChange={e => setWidth(+e.target.value)}  /></label>
      <label>Height:  <input type="number" value={height} onChange={e => setHeight(+e.target.value)} /></label>
      <label>Start X: <input type="number" value={startX} onChange={e => setStartX(+e.target.value)} /></label>
      <label>Start Y: <input type="number" value={startY} onChange={e => setStartY(+e.target.value)} /></label>
      <label>Goal X:  <input type="number" value={goalX}  onChange={e => setGoalX(+e.target.value)}  /></label>
      <label>Goal Y:  <input type="number" value={goalY}  onChange={e => setGoalY(+e.target.value)}  /></label>
      <button type="submit">Create</button>
    </form>
  );
}