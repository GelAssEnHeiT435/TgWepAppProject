import { useTelegramAuth } from "../contexts/TelegramAuthContext";
import { useCatalog } from "../contexts/CatalogContext";
import React, { useState, useEffect } from "react";
import ProductCard from "../components/layout/ProductCard";
import { BasketLine } from "../components/common/BasketLine";
import { MenuOutlined } from "../components/common/MenuOutlined"
import logo from "../assets/icons/flower1.jpg";
import '../assets/styles/Catalog.css';


function Catalog()
{
    const {user, logout} = useTelegramAuth();
    const {getTotalItems} = useCatalog();
    const totalItems = getTotalItems();

    const products = [
        { id: 1, name: 'Товар 1', price: "2000 ₽", photo: logo },
        { id: 2, name: 'Товар 2', price: "2000 ₽", photo: logo },
        { id: 3, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 4, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 5, name: 'Товар 3', price: "2000 ₽", photo: logo, quantity: 10 },
        { id: 6, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 7, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 8, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 9, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 10, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 11, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 12, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 13, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 14, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 15, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 16, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 17, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 18, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 19, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 20, name: 'Товар 3', price: "2000 ₽", photo: logo },
        { id: 21, name: 'Товар 3', price: "2000 ₽", photo: logo },
    ];

    useEffect(() => {
        //TODO: response to server for getting product's list
        console.log("get products")
    }, [])

    return (
        <div className="catalog-container">
            <div className="product-grid">
            {
                products.map(product => (<ProductCard 
                    key={product.id}
                    id={product.id}
                    photo={product.photo}
                    name={product.name} 
                    price={product.price}
                    quantity={product.quantity}/>))
            }
            </div>
        </div>
    )
}

export default Catalog;