import { useLocation } from 'react-router-dom';
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
    const {user, isAdmin} = useTelegramAuth();
    const {getTotalItems} = useCatalog();
    const [menuIsActive, setMenuIsActive] = useState(false);

    const totalItems = getTotalItems();
    const showBasket = location.pathname === '/';

    return(
        <header className="catalog-header">
                <h4 className="header-display">Каталог</h4>
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
                            <button> <ProfileOutlined /> Профиль</button>
                            <button> <CatalogLine /> Каталог</button>
                            <button> <GiftLine /> Розыгрыши и акции</button>
                            {!user?.isAdmin && (
                                <button> <AdministratorLine /> Админ-панель</button>
                            )}
                            <button> <InfoOutlined /> О нас</button>
                            <div className="menu-divider"></div>
                            <button className="logout-btn"> <LogoutOutlined /> Выйти</button>
                        </div>
                    )}
                </div>
            </header>
    )
}

export default Header;