// src/api/axiosInstance.ts
import axios from "axios";

export const serverUrl = "http://localhost:8080/api";

const api = axios.create({
    baseURL: serverUrl,
});

// Add interceptor to attach token
api.interceptors.request.use((config) => {
    const token = localStorage.getItem("jwt");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default api;