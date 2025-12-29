import apiClient from './ApiClient';

const CLAIM_ROLE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"

let _accessToken = null;
let _role = null;
let refreshTimeout = null;

export const getAccessToken = () => _accessToken;
export const setAccessToken = (token) => _accessToken = token;

export const getRole = () => _role;
export const setRole = (token) => 
{
    const jsonData = JSON.parse(atob(token.split('.')[1]))
    console.log(jsonData);
    _role = jsonData[CLAIM_ROLE] || undefined;

}

const getTokenExpiry = () => {
    try {
        const payload = JSON.parse(atob(_accessToken.split('.')[1]));
        return payload.exp * 1000;
    }
    catch {
        return null;
    }
};

export const isTokenExpiringSoon = (bufferedSeconds = 60) => {
    const expiry = getTokenExpiry()
    return expiry ? Date.now() > expiry - bufferedSeconds * 1000 : true;
}


const clearRefreshTimeout = () => {
    if(refreshTimeout) clearTimeout(refreshTimeout);
};

const scheduleTokenRefresh = () => {
    clearRefreshTimeout();
    const expiry = getTokenExpiry()

    if (!expiry) return;

    const buffer = 60 * 1000; // time before the update: 1 minute
    const timeout = expiry - Date.now() - buffer; // remaining time

    if (timeout > 0) {
        refreshTimeout = setTimeout(() => {
            refreshTokenSilently()
        }, timeout)
    }
};

const refreshTokenSilently = async () => {
    try {
        const response = await apiClient.post(`/auth/refresh`);
        _accessToken = response.data;
        scheduleTokenRefresh();
    }
    catch {
        _accessToken = null;
        authLogout()
    }
};


export const authLogin = (token) => {
    setAccessToken(token);
    setRole(token);
    console.log(getRole());
    scheduleTokenRefresh();
};

export const authLogout = () => {
    clearRefreshTimeout();
    _accessToken = null;
    window?.Telegram?.WebApp.close();
};

