import axios from "axios";

const API_URL = "http://localhost:8080/api/dungeons";

export async function createDungeon(map: any) {
  const response = await axios.post(API_URL, map);
  return response.data;
}

export async function getDungeon(id: string) {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
}

//Streaming version of getPath
export async function* streamPath(id: string) {
    const response = await fetch(`http://localhost:8080/api/dungeons/${id}/path`);

    if (!response.body) {
        throw new Error("No response body");
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder("utf-8");
    let buffer = "";

    while (true) {
        const { done, value } = await reader.read();
        if (done) break;

        buffer += decoder.decode(value, { stream: true });

        // Split on newlines (NDJSON format)
        let pathResponse = buffer;
        const lines = buffer.split("\n");

        buffer = lines.pop()!; // keep incomplete line

        for (const line of lines) {
            if (line.trim().length > 0) {
                yield JSON.parse(line); // yield each position immediately
            }
        }
    }
}

export async function saveWallsToApi(id: number, obstacles: any[]) {
    const response = await axios.patch(`${API_URL}/${id}/obstacles`, obstacles);
    return response.data;
}