import React, {createContext, useContext, useState, useEffect } from "react";
import User from '../models/User'


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
    const [admins, setAdmins] = useState([]);

    useEffect(() => {
        //TODO: response to server admin's list, convert and save
        setAdmins([])
    }, [])

    useEffect(() => {
        if (window.Telegram?.WebApp)
        {
            const tg = window.Telegram.WebApp;

            tg.ready();
            tg.expand()

            const initData = tg.initDataUnsafe;
            const userData = initData.user;

            if (userData)
            {
                const userRole = admins.includes(userData.id) ? 'admin' : 'user'

                const userInfo = new User(); //TODO: add user params after editing User.js
                setUser(userInfo);
                console.log('User authentificated', userInfo);
            }
            else console.log('No user data available');
        }
        else console.warn('Telegram WebApp not available - running in development mode');

        setLoading(false);
    }, [admins])

    function logout()
    {
        if (window.Telegram?.WebApp) window.Telegram.WebApp.close();
        else setUser(null);
    }

    const value = {
        user,
        loading,
        logout,
        isAuthenticated: !!user,
        isAdmin: user?.isAdmin || false
    }

    return (
        <TelegramAuthContext.Provider value={value}>
            {children}
        </TelegramAuthContext.Provider>
    )
}