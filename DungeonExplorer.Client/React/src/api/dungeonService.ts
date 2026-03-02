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

export async function getPath(id: string) {
  const response = await axios.get(`${API_URL}/${id}/path`);
  return response.data;
}

export async function saveWallsToApi(id: number, obstacles: any[]) {
    const response = await axios.patch(`${API_URL}/${id}/obstacles`, obstacles);
    return response.data;
}