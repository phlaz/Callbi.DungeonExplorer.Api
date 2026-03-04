import axiosClient, { serverUrl } from "./axiosClient";

const API_URL = serverUrl + "/auth";

export async function register(email: string, password: string) {
    const response = await axiosClient.post(`${API_URL}/register`, { Email: email, Password: password });
    const token = response.data.token;
    localStorage.setItem("jwt", token);
    return token;
}

// Login: POST credentials, get JWT
export async function login(email : string, password : string) {
    const response = await axiosClient.post(`${API_URL}/login`, {
        email,
        password
    });

    const token = response.data.token;
    localStorage.setItem("jwt", token);
    return token;
}
