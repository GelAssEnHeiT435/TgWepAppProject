import { useTelegramAuth } from "../contexts/TelegramAuthContext";
import React, { useState, useEffect } from "react";
import ProductCard from "../components/common/ProductCard";
import '../assets/styles/Catalog.css'

function Catalog()
{
    const {user, logout} = useTelegramAuth();

    const products = [
        { id: 1, name: 'Товар 1', price: 1000, description: 'Описание товара 1' },
        { id: 2, name: 'Товар 2', price: 2000, description: 'Описание товара 2' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
        { id: 3, name: 'Товар 3', price: 1500, description: 'Описание товара 3' },
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
                    products.map(product => (<ProductCard name={product.name} 
                                                          price={product.price}/>))
                }
            </div>
        </div>
    )
}

export default Catalog;