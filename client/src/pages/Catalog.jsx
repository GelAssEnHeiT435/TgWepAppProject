import { useTelegramAuth } from "../contexts/TelegramAuthContext";
import React, { useState, useEffect } from "react";
import ProductCard from "../components/common/ProductCard";
import '../assets/styles/Catalog.css'

function Catalog()
{
    const {user, logout} = useTelegramAuth();

    const products = [
        { id: 1, name: 'Товар 12145263634573457745757547', price: 1000, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 2, name: 'Товар 2', price: 2000, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
        { id: 3, name: 'Товар 3', price: 1500, photo: "D:/my_works/TgWepAppProject/client/src/assets/icons/logo192.png" },
    ];

    useEffect(() => {
        //TODO: response to server for getting product's list
        console.log("get products")
    }, [])

    return (
        <div className="catalog-container">
            <div className="catalog-header">
                <h4>Каталог</h4>
            </div>

            <div className="product-grid">
            {
                products.map(product => (<ProductCard photo={product.photo}
                                                      name={product.name} 
                                                      price={product.price}/>))
            }
            </div>
        </div>
    )
}

export default Catalog;