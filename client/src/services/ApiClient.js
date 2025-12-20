import axios from 'axios';
import config from '../config';
import { getAccessToken, setAccessToken, authLogout } from './Auth'

const apiClient = axios.create({
  baseURL: config.apiBaseUrl,
  withCredentials: true,
});

apiClient.interceptors.request.use(
  config => {
    if (getAccessToken()) {
      config.headers.Authorization = `Bearer ${getAccessToken()}`;
    }
    return config;
  }
);

apiClient.interceptors.response.use(
  response => response,
  async (error) => {
    const originalRequest = error.config;

    if(error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const response = await apiClient.post('/auth/refresh');
        setAccessToken(response.data);
        originalRequest.headers.Authorization = `Bearer ${response.data}`;
        return apiClient(originalRequest);
      }
      catch {
        authLogout();
      }
    }
    return Promise.reject(error);
  }
)

export default apiClient;