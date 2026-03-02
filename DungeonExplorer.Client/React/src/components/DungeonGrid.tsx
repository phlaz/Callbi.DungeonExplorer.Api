import React, { useState } from "react";

export default function DungeonGrid({ dungeon, path, onWallsChanged, onSaveWalls }: {
    dungeon: any;
    path: any[];
    onWallsChanged?: (walls: any[]) => void;
    onSaveWalls?: (walls: any[]) => void;
}) {
    if (!dungeon) return null;

    // Keep walls in local state so you can add/remove them interactively
    const [walls, setWalls] = useState(dungeon.walls || []);
    
    const cells = [];
    for (let y = 0; y < dungeon.height; y++) {
        for (let x = 0; x < dungeon.width; x++) {
            const isStart = dungeon.startPosition.x === x && dungeon.startPosition.y === y;
            const isGoal = dungeon.goal.x === x && dungeon.goal.y === y;
            const isWall = walls.some((w: any) => w.x === x && w.y === y);
            const isPath = Array.isArray(path) && path.some((p: any) => p.x === x && p.y === y);

            let style: React.CSSProperties = {
                width: 30,
                height: 30,
                border: "1px solid #ccc",
                display: "inline-block",
                backgroundColor: "#eee",
                cursor: "pointer"
            };

            if (isStart) style.backgroundColor = "green";
            else if (isGoal) style.backgroundColor = "gold";
            else if (isWall) style.backgroundColor = "black";
            else if (isPath) style.backgroundColor = "blue";

            cells.push(
                <div
                    key={`${x}-${y}`}
                    style={style}
                    onClick={() => toggleWall(x, y)}
                ></div>
            );
        }
        cells.push(<div key={`row-${y}`} style={{ clear: "both" }}></div>);
    }

    const toggleWall = (x: number, y: number) => {
        const exists = walls.some((w: any) => w.x === x && w.y === y);
        let newWalls;
        if (exists) {
            newWalls = walls.filter((w: any) => !(w.x === x && w.y === y));
        } else {
            newWalls = [...walls, { x, y }];
        }
        setWalls(newWalls);
        if (onWallsChanged) onWallsChanged(newWalls); // notify parent if needed
    };


    const handleSave = () => {
        if (onSaveWalls) {
            onSaveWalls(walls);   // <-- call parent with updated walls
        }
    };

    return <div>{cells}<button onClick={handleSave} style={{ marginTop: "10px" }}>
        Save Grid
    </button>
</div>;
}