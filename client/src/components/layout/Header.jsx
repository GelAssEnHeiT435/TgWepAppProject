import {  useNavigate, useLocation } from 'react-router-dom';
import React, { useState, useEffect } from "react";
import { useCatalog } from "../../contexts/CatalogContext";
import { useTelegramAuth } from "../../contexts/TelegramAuthContext";

import { BasketLine } from "../common/BasketLine";
import { MenuOutlined } from "../common/MenuOutlined";
import { CatalogLine } from "../common/CatalogLine";
import { GiftLine } from '../common/GiftLine';
import { AdministratorLine } from '../common/AdministratorLine';
import { ProfileOutlined } from '../common/ProfileOutlined';
import { InfoOutlined } from '../common/InfoOutlined';
import { LogoutOutlined } from '../common/LogoutOutlined';

import "../../assets/styles/Header.css"

function Header()
{
    const location = useLocation();
    const navigate = useNavigate();
    const {isAdmin, isAuthenticated, logout} = useTelegramAuth();
    const {getTotalItems} = useCatalog();
    const [menuIsActive, setMenuIsActive] = useState(false);

    const totalItems = getTotalItems();
    const showBasket = isAuthenticated && location.pathname === '/';

    let header_text = '';
    switch(location.pathname){
        case '/': 
            header_text = 'Каталог';
            break;
        case '/gifts':
            header_text = 'Розыгрыши';
            break;
        case '/profile':
            header_text = 'Профиль';
            break;
        case '/admin':
            header_text = 'Разработчик';
            break;
        case '/about':
            header_text = 'О нас';
            break;
    }
    if(!isAuthenticated) header_text = 'Ошибка'

    return(
        <header className="catalog-header">
                <h4 className="header-display">{header_text}</h4>
                <div className="header-buttons-container">
                    {showBasket && (
                        <button className="header-button">
                            <BasketLine />
                            {totalItems > 0 && (
                                <span className="basket-badge">
                                    {totalItems > 99 ? '99+' : totalItems}
                                </span>
                            )}
                            <p>Корзина</p>
                        </button>
                    )}

                    <button className="header-button" onClick={() => setMenuIsActive(!menuIsActive)}>
                        <MenuOutlined />
                        <p>Меню</p>
                    </button>

                    {menuIsActive && (
                        <div className='dropdown-menu'>
                            <button onClick={() => { navigate('/profile'); setMenuIsActive(false); }}> 
                                <ProfileOutlined /> Профиль
                            </button>

                            <button onClick={() => { navigate('/'); setMenuIsActive(false); }}> 
                                <CatalogLine /> Каталог
                            </button>

                            <button onClick={() => { navigate('/gifts'); setMenuIsActive(false); }}> 
                                <GiftLine /> Розыгрыши и акции
                            </button>

                            {isAdmin && (
                                <button onClick={() => { navigate('/admin'); setMenuIsActive(false); }}> 
                                    <AdministratorLine /> Админ-панель
                                </button>
                            )}

                            <button onClick={() => { navigate('/about'); setMenuIsActive(false); }}> 
                                <InfoOutlined /> О нас
                            </button>

                            <div className="menu-divider"></div>

                            <button onClick={() => { logout(); setMenuIsActive(false); }} 
                                    className="logout-btn"> 
                                <LogoutOutlined /> Выйти
                            </button>
                        </div>
                    )}
                </div>
            </header>
    )
}

export default Header;