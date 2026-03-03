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
    const response = await fetch(`${API_URL}/${id}/path`);

    if (!response.body) {
        throw new Error("No response body");
    }

    // Handle non-200 responses (ApiError JSON)
    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.details || "Unknown error");
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder("utf-8");
    let buffer = "";

    while (true) {
        const { done, value } = await reader.read();
        if (done) break;

        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split("\n");
        buffer = lines.pop()!;

        for (const line of lines) {
            if (line.trim().length > 0) {
                yield JSON.parse(line);
            }
        }
    }
}

export async function saveWallsToApi(id: number, obstacles: any[]) {
    const response = await axios.patch(`${API_URL}/${id}/obstacles`, obstacles);
    return response.data;
}