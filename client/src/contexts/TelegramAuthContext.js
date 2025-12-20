import React, {createContext, useContext, useState, useEffect } from "react";
import axios from 'axios';

import {authLogin, getAccessToken, authLogout} from '../services/Auth'
import User from '../models/User'
import config from '../config'


const TelegramAuthContext = createContext();

export function useTelegramAuth()
{
    const context = useContext(TelegramAuthContext)
    if (!context) throw new Error('useTelegramAuth must be used within TelegramAuthProvider');
    return context
}

export function TelegramAuthProvider({children})
{
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const initializeAuth = async () => {
            try {
                if (window.Telegram?.WebApp) {
                    const tg = window.Telegram.WebApp;
                    tg.ready();
                    tg.expand();

                    const initData = tg.initData;

                    // if (!initData) {
                    //     console.warn('No initData');
                    //     setLoading(false);
                    //     return;
                    // }

                    const userId = 397567122;

                    const response = await axios.post(
                        `${config.apiBaseUrl}/auth/login`,
                        userId, 
                        {
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            withCredentials: true
                        }
                    );
                    authLogin(response.data);
                    setIsAuthenticated(true);
                } 
                else {
                // development mock
                    setUser({ id: 123, name: 'Dev User', role: 'user' });
                    setIsAuthenticated(true);
                }
            } 
            catch (error) {
                console.error('Auth failed:', error);
            } 
            finally {
                setLoading(false);
            }
        };

        const timer = setTimeout(initializeAuth, 500);
        return () => clearTimeout(timer)
    }, [])

    function logout()
    {
        authLogout(); 
        setUser(null);
        setIsAuthenticated(false);
        if (window.Telegram?.WebApp) {
            window.Telegram.WebApp.close();
        }
    }

    const value = {
        user,
        loading,
        logout,
        isAuthenticated: true, //!!user,
        isAdmin: true //user?.isAdmin || false
    }

    return (
        <TelegramAuthContext.Provider value={value}>
            {children}
        </TelegramAuthContext.Provider>
    )
}