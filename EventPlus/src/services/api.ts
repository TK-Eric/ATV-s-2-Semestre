import axios from "axios";

export const api = axios.create({
  baseURL: "https://localhost:7021/api/",
  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  }
});