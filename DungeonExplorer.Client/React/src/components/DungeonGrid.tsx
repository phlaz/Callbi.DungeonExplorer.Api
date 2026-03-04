import React, { useState, useEffect } from "react";

export default function DungeonGrid({ dungeon, path, onObstaclesChanged, onSaveObstacles }: {
    dungeon: any;
    path: any[];
    onObstaclesChanged?: (obstacles: any[]) => void;
    onSaveObstacles?: (obstacles: any[]) => void;
}) {
    if (!dungeon) return null;

    useEffect(() => {
        setObstacles(dungeon?.obstacles ?? []);
    }, [dungeon]);

    // Keep obstacles in local state so you can add/remove them interactively
    const [obstacles, setObstacles] = useState(dungeon.obstacles || []);
    
    const cells = [];
    for (let y = 0; y < dungeon.height; y++) {
        for (let x = 0; x < dungeon.width; x++) {
            const isStart = dungeon.start.x === x && dungeon.start.y === y;
            const isGoal = dungeon.goal.x === x && dungeon.goal.y === y;
            const isObstacle = obstacles.some((o: any) => o.x === x && o.y === y);
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
            else if (isObstacle) style.backgroundColor = "black";
            else if (isPath) style.backgroundColor = "blue";

            cells.push(
                <div
                    key={`${x}-${y}`}
                    style={style}
                    onClick={() => toggleObstacle(x, y)}
                ></div>
            );
        }
        cells.push(<div key={`row-${y}`} style={{ clear: "both" }}></div>);
    }

    const toggleObstacle = (x: number, y: number) => {
        const exists = obstacles.some((o: any) => o.x === x && o.y === y);
        let newObstacles;
        if (exists) {
            newObstacles = obstacles.filter((o: any) => !(o.x === x && o.y === y));
        } else {
            newObstacles = [...obstacles, { x, y }];
        }
        setObstacles(newObstacles);
        if (onObstaclesChanged) onObstaclesChanged(newObstacles); // notify parent if needed
    };


    const handleSave = () => {
        if (onSaveObstacles) {
            onSaveObstacles(obstacles);   // <-- call parent with updated obstacles
        }
    };

    return <div>{cells}<br/><b>Click the grid to add and remove obstacles. Dont forget to Save!</b><br/><button onClick={handleSave} style={{ marginTop: "10px" }}>
        Save Obstacles
    </button>
</div>;
}