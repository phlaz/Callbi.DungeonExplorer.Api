export default function DungeonGrid({ dungeon, path }: { dungeon: any, path: any[] }) {
    if (!dungeon) return null;

    const cells = [];
    for (let y = 0; y < dungeon.height; y++) {
        for (let x = 0; x < dungeon.width; x++) {
            const isStart = dungeon.startPosition.x === x && dungeon.startPosition.y === y;
            const isGoal = dungeon.goal.x === x && dungeon.goal.y === y;
            const isWall = dungeon.walls.some((w: any) => w.x === x && w.y === y);
            const isPath = Array.isArray(path) && path.some((p: any) => p.x === x && p.y === y);

            let style: React.CSSProperties = {
                width: 30,
                height: 30,
                border: "1px solid #ccc",
                display: "inline-block"
            };

            if (isStart) style = { ...style, backgroundColor: "green" };
            else if (isGoal) style = { ...style, backgroundColor: "gold" };
            else if (isWall) style = { ...style, backgroundColor: "black" };
            else if (isPath) style = { ...style, backgroundColor: "blue" };

            cells.push(<div key={`${x}-${y}`} style={style}></div>);
        }
        cells.push(<div key={`row-${y}`} style={{ clear: "both" }}></div>);
    }

    return <div>{cells}</div>;
}