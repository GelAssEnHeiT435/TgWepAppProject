import { useState, useEffect } from "react";
import { useLocation } from 'react-router-dom';
import { useTelegramAuth } from "../contexts/TelegramAuthContext";
import { useCatalog } from "../contexts/CatalogContext";

import ProductCard from "../components/layout/ProductCard";
import logo from "../assets/icons/flower1.jpg";

import '../assets/styles/Catalog.css';

function Catalog()
{
    const {user, logout} = useTelegramAuth();
    const {products, getTotalItems, loadCatalog} = useCatalog();
    const totalItems = getTotalItems();
    const location = useLocation();

    useEffect(() => {
        loadCatalog();
    }, [location.pathname]);

    return (
        <div className="catalog-container">
            <div className="product-grid">
            {
                products.map(product => {
                return <ProductCard 
                    key={product.id}
                    id={product.id}
                    photo={product.photo}
                    name={product.name} 
                    price={product.price}
                    quantity={product.quantity}/>})
            }
            </div>
        </div>
    )
}

export default Catalog;